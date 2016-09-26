using UnityEngine;
using System.Collections;
using LyLib;

public class HeroController : MonoBehaviour {

    public GameObject pivot_point_object;
    public GameObject explode_ps_prefab;
    public GameObject target_point_prefab;
    public GameObject finish_point_prefab;
    public LineRenderer line_render;
    public AudioClip break_sound, eat_sound, finish_sound;
    public Transform target_indicator;

    public GameObject pieces_prefab;
    public float angle_speed = 360;

    bool started = false;
    float radius = 0;
    bool key_down = false;
    SceneConfig scene_config;
    GameObject last_point;
    bool game_done = false;
    Vector3 origin_pos;
    float time_start;

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
    void Start()
    {
        radius = Vector3.Distance(pivot_point_object.transform.position, transform.position);
        scene_config = GameObject.FindObjectOfType<SceneConfig>();
        transform.parent.FindChild("PointLight").gameObject.SetActive(scene_config.use_light);
        angle_speed = scene_config.hero_angle_speed;
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
        pivot_point.y += 6;
        pivot_point.z += 6 * sign;
        angle_speed = Mathf.Abs(angle_speed) * sign;
        pivot_point_object.transform.position = pivot_point;
        radius = Vector3.Distance(pivot_point, transform.position);
        if (radius < 2)
        {
            Debug.LogError("!!");
        }
    }
	// Update is called once per frame
	void Update () 
    {
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
        return angle_speed * (1 + (pivot_point.y - transform.position.y) / radius * 0.5f);
    }
    void Rotate(Vector3 pivot_point)
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;

        //Debug.Log(rotation_speed);
        transform.RotateAround(pivot_point, new Vector3(-1, 0, 0), GetRotationSpeed() * Time.deltaTime);
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
        var target_vector = last_point.transform.position - transform.position;
        target_indicator.rotation = Quaternion.FromToRotation(Vector3.up, target_vector);
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
            if (GetKey(out key))
            {
                started = true;
                //GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 10);
                GameObject.Find("StartTip").SetActive(false);
                time_start = Time.timeSinceLevelLoad;
            }
            else
            {
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                transform.position = origin_pos;
                return;
            }
        }

        UpdateTargetIndicator();

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
                var ver_vec = transform.position - pivot_point;
                float tmp = ver_vec.z;
                ver_vec.z = -ver_vec.y;
                ver_vec.y = tmp;

                // 得到线速度
                var speed = GetSpeed();
                GetComponent<Rigidbody>().velocity = ver_vec.normalized.Multi(speed);

                key_down = false;
                HideRope();

            }
        }
    }
}
