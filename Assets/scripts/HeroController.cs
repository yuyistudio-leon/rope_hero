using UnityEngine;
using System.Collections;
using LyLib;
using UnityEngine.UI;

public class HeroController : MonoBehaviour {
    public static HeroController instance;
    public HeroController()
    {
        instance = this;
    }

    public GameObject pivot_point_object;
    public GameObject point_light;
    public GameObject start_tip;
    public GameObject explode_ps_prefab, rope_insert_ps_prefab;
    public GameObject target_point_prefab;
    public GameObject finish_point_prefab;
    public LineRenderer line_render;
    public AudioClip break_sound, eat_sound, finish_sound;
    public Transform target_indicator;

    public GameObject pieces_prefab;
    public float angle_speed = 360;

    bool started = false;
    float radius = -1, delta_pos = -1;
    bool key_down = false;
    SceneConfig scene_config;
    GameObject last_point;
    bool game_done = false;
    Vector3 origin_pos;
    float time_start;
    float max_speed;
    private float last_time_trigger_enter = 0;
    const float FREEZE_TIME = 0.5f;
    float freeze_time_left = FREEZE_TIME;

    public void Unfreeze()
    {
        freeze_time_left = FREEZE_TIME;
    }
    public void Freeze()
    {
        freeze_time_left = 999999999;
    }
    // 重新开始一关时调用，用来重置游戏
    public void Reset()
    {
        scene_config.Reset();
        if (start_tip != null)
        {
            start_tip.SetActive(true);
        }
        gameObject.SetActive(true);
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        GetComponent<HeroController>().enabled = true;
        // 重置状态
        started = false;
        freeze_time_left = FREEZE_TIME;
        key_down = false;
        game_done = false;
        // 重置transform
        transform.position = new Vector3(0, 10, 0);
        transform.localRotation = Quaternion.identity;
        // 删除
        if (last_point != null)
        {
            Destroy(last_point);
        }
        // 重新开始
        Camera.main.gameObject.transform.position = new Vector3(20, 10, 0);
        Start();
    }
    void GenPoint()
    {
        var next_point = scene_config.NextTarget();
        if (next_point == Vector3.zero)
        {
            // 生成终点
            last_point = GameObject.Instantiate(finish_point_prefab);
            last_point.transform.position = scene_config.finish_point.position;
        }
        else
        {
            // 生成目标点
            last_point = GameObject.Instantiate(target_point_prefab);
            last_point.transform.position = next_point;
        }
        last_point.name = "target:" + Random.value;
    }
    void OnCollisionEnter(Collision co)
    {
        GetComponent<HeroController>().Die();
    }
	// Use this for initialization
    public void OnTriggerEnter(Collider co)
    {
        if ((Time.timeSinceLevelLoad - last_time_trigger_enter) < 0.15f)
        {
            return;
        }
        last_time_trigger_enter = Time.timeSinceLevelLoad;

        var ps = GameObject.Instantiate(explode_ps_prefab);
        ps.transform.position = co.transform.position;
        Destroy(co.gameObject);

        if (co.CompareTag("TargetPoint"))
        {
            //GameObject.Destroy(co.gameObject);
            SoundManager.instance.PlayOneShot(eat_sound);
            GenPoint();
        }
        else if (co.CompareTag("FinishPoint"))
        {
            //GameObject.Destroy(co.gameObject);
            SoundManager.instance.PlayOneShot(eat_sound);
            GameWin();
        }
        else
        {
            Debug.LogError("unknown tag: " + co.tag + ", name: " + co.name);
        }
    }
    public void Start()
    {
        if (radius < 0)
        {
            radius = Vector3.Distance(pivot_point_object.transform.position, transform.position);
            delta_pos = radius / Mathf.Sqrt(2);
        }
        scene_config = LoadLevel.map.GetComponent<SceneConfig>();

        start_tip.transform.Find("Banner").Find("LevelText").GetComponent<Text>().text = "Level " + LoadLevel.level;
        start_tip.transform.Find("TipText").GetComponent<Text>().text = scene_config.level_tip;

        point_light.SetActive(scene_config.use_light);
        angle_speed = scene_config.hero_angle_speed;
        max_speed = angle_speed / 180 * Mathf.PI * radius;
        origin_pos = transform.position;
        HideRope();
        GenPoint();
	}
    public float GetSpeed()
    {
        if (key_down)
        {
            // 得到线速度
            var speed = GetRotationSpeed() / 180 * Mathf.PI * radius;
            return speed;
        }
        else
        {
            return GetComponent<Rigidbody>().velocity.magnitude;
        }
    }
    void NewPivotPoint(int sign)
    {
        if (!started) return;
        var pivot_point = transform.position;
        pivot_point.y += delta_pos;
        pivot_point.x += delta_pos * sign;
        angle_speed = Mathf.Abs(angle_speed) * sign;
        pivot_point_object.transform.position = pivot_point;
        GameObject.Instantiate(rope_insert_ps_prefab).transform.position = pivot_point;
    }
	// Update is called once per frame
	void Update () 
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameWin();
        }
	}
    public void GameWin()
    {
        HideRope();
        game_done = true;
        GetComponent<HeroController>().enabled = false;
        float seconds_used = Time.timeSinceLevelLoad - time_start;
        DATA.RecordLevelFinished(seconds_used);
        GameObject.FindObjectOfType<LoadLevel>().NextLevel();
    }
    public void Die()
    {
        if (game_done) { return; }

        SoundManager.instance.PlayOneShot(break_sound);

        gameObject.SetActive(false);
        GetComponent<HeroController>().HideRope();

        var pieces = GameObject.Instantiate(pieces_prefab);
        pieces.SetActive(true);
        pieces.transform.position = transform.position;
        pieces.transform.rotation = transform.rotation;
        pieces.transform.SetParent(null);

        GameObject.FindObjectOfType<LoadLevel>().ReloadLevel();
    }
    float GetRotationSpeed()
    {
        var pivot_point = pivot_point_object.transform.position;
        return angle_speed * (0.5f + (pivot_point.y - transform.position.y + radius) / radius * 0.25f);
    }
    void Rotate(Vector3 pivot_point)
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;

        //Debug.Log(rotation_speed);
        transform.RotateAround(pivot_point, new Vector3(0, 0, 1), GetRotationSpeed() * Time.deltaTime);
        line_render.SetPosition(1, transform.position);
    }
    public void HideRope()
    {
        line_render.enabled = false;
        pivot_point_object.SetActive(false);
    }
    void ShowRope()
    {
        line_render.enabled = true;
        pivot_point_object.SetActive(true);
    }
    void UpdateTargetIndicator()
    {
        if (last_point)
        {
            var target_vector = last_point.transform.position - transform.position;
            target_indicator.rotation = Quaternion.FromToRotation(Vector3.up, target_vector);
        }
    }

    private KeyCode last_single_touch_key = KeyCode.Escape;
    KeyCode GetTouchKey(Touch touch)
    {
        if (touch.position.x < Screen.width / 2)
        {
            return KeyCode.A;
        }
        else
        {
            return KeyCode.S;
        }
    }
    bool GetKey_Mobile(out KeyCode key)
    {
        if (Input.touchCount == 0)
        {
            key = KeyCode.Escape;
            return false;
        }
        if (Input.touchCount == 2)
        {
            var key1 = GetTouchKey(Input.touches[0]);
            var key2 = GetTouchKey(Input.touches[1]);
            if (key1 == last_single_touch_key)
            {
                key = key2;
            }
            else
            {
                key = key1;
            }
        }
        else
        {
            key = GetTouchKey(Input.touches[0]);
            last_single_touch_key = key;
        }
        return true;
    }
    bool GetKey(out KeyCode key)
    {
        if (GetKey_Mobile(out key))
        {
            return true;
        }
        else
        {
            return GetKey_Keyboard(out key);
        }
    }
    bool GetKey_Keyboard(out KeyCode key)
    {
        if (Input.GetKey(KeyCode.A))
        {
            key = KeyCode.A;
            return true;
        }
        if (Input.GetKey(KeyCode.S))
        {
            key = KeyCode.S;
            return true;
        }
        key = KeyCode.Escape;
        return false;
    }

    private KeyCode last_key_pressed = KeyCode.Escape;
    void FixedUpdate()
    {
        KeyCode key;
        var pivot_point = pivot_point_object.transform.position;
        if (!started)
        {
            if (GetKey(out key) && freeze_time_left < 0)
            {
                started = true;
                //GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 10);
                start_tip.SetActive(false);
                time_start = Time.timeSinceLevelLoad;
            }
            else
            {
                freeze_time_left -= Time.deltaTime;
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                transform.position = origin_pos;
                return;
            }
        }


        UpdateTargetIndicator();

        var body = GetComponent<Rigidbody>();

        if (GetKey(out key))
        {
            if (!key_down || last_key_pressed != key)
            {
                key_down = true;
                last_key_pressed = key;
                NewPivotPoint(key == KeyCode.A ? -1 : 1);
                line_render.SetPosition(0, pivot_point_object.transform.position);
                ShowRope();
            }
            Rotate(pivot_point);
        }
        else
        {
            if (key_down)
            {
                // 得到旋转的切线方向向量
                var ori_vec = transform.position - pivot_point;
                var ver_vec = ori_vec;
                float tmp = ver_vec.x;
                ver_vec.x = - ver_vec.y;
                ver_vec.y = tmp;

                // 得到线速度
                var speed = GetSpeed();
                GetComponent<Rigidbody>().velocity = ver_vec.normalized.Multi(speed);
                key_down = false;
                HideRope();
            }
            if (body.velocity.magnitude > max_speed)
            {
                float factor = max_speed / body.velocity.magnitude;
                body.velocity = body.velocity.Multi(factor);
            }
        }
    }
}
