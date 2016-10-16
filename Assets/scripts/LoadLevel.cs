using UnityEngine;
using System.Collections;
using LyLib;
using UnityStandardAssets.ImageEffects;

public class LoadLevel : MonoBehaviour {
    public static int level = 1;
    public static GameObject map;// 当前地图
    private bool load_next_level;
    public GameObject story_panel;
    public bool dont_create_map = false;

    bool image_on = true;
    void Awake()
    {
        if (PlayerPrefs.HasKey(CONST.BLOOM_IMAGE_EFFECT_KEY))
        {
            Camera.main.GetComponent<BloomOptimized>().enabled = PlayerPrefs.GetInt(CONST.BLOOM_IMAGE_EFFECT_KEY) == 1;
        }
        if (dont_create_map)
        {
            map = GameObject.FindObjectOfType<SceneConfig>().gameObject;
            if (!map)
            {
                Debug.LogError("!");
            }
            return;
        }
        if (!map)
        {
            var res_name = "Levels/" + level;
            map = GameObject.Instantiate(Resources.Load(res_name, typeof(GameObject)) as GameObject);
            if (!map)
            {
                Debug.LogError("failed to load " + res_name + " as GameObject");
            }

            // story starts
            if (level == 1)
            {
                Debug.Log("story starts");
                HeroController.instance.Freeze();
            }
            else
            {
                GameObject.Destroy(story_panel);
            }
        }
    }
    IEnumerator ResetLevel(bool wait = true)
    {
        if (wait)
        {
            yield return new WaitForSeconds(1);
        }
        if (load_next_level)
        {
            level += 1;
            if (level > DATA.LEVEL_COUNT)
            {
                Scene_LoadAsync.LoadWithTransition("menu", "Loading menu ...");
            }
            else
            {
                if (map)
                {
                    Debug.Log("destroy old map");
                    Destroy(map);
                }
                var res_name = "Levels/" + level;
                Debug.Log("loading map " + res_name);
                var level_prefab = Resources.Load(res_name, typeof(GameObject)) as GameObject;
                if (level_prefab == null)
                {
                    Scene_LoadAsync.LoadWithTransition("menu", "Loading menu ...");
                    map = null;
                }
                else
                {
                    map = GameObject.Instantiate(level_prefab);
                }
            }
        }
        if (map != null)
        {
            HeroController.instance.Reset();
        }
    }
    public void ReloadLevel()
    {
        load_next_level = false;
        StartCoroutine(ResetLevel());
    }
    public void NextLevel()
    {
        Debug.Log("next level");
        load_next_level = true;
        StartCoroutine(ResetLevel());
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("next leve loading...");
            load_next_level = false;
            StartCoroutine(ResetLevel(false));
        }
    }
}
