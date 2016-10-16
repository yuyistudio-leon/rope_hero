using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using UnityStandardAssets.ImageEffects;

class CONST
{
    public static string BLOOM_IMAGE_EFFECT_KEY = "__bloom_image_effect__";
}
public class MainmenuHandler : MonoBehaviour {
    public static Color global_light_color;
    public static  float global_light_strength = -1;

    public static int page_no = 0;
    public GameObject level_selector;
    public GameObject main_menu_panel;
    public GameObject contact_info_panel;
    public Sprite finished_sprite, default_sprite;
    public Transform camera_transform;
    public Transform first_camera_transform;
    public Transform second_camera_transform;

    Transform camera_target_transform;

    void SetupLevelButtons()
    {
        int start_index = page_no * DATA.PAGE_LEVEL_COUNT;
        var buttons = level_selector.transform.FindChild("LevelSelector");
        for (int i = 0; i < buttons.transform.childCount; ++i)
        {
            var child = buttons.transform.GetChild(i);
            int level_index = start_index + i + 1;
            if (level_index > DATA.LEVEL_COUNT)
            {
                child.gameObject.SetActive(false);
            }
            else
            {
                child.gameObject.SetActive(true);
                float finish_seconds = DATA.IsLevelFinished(level_index.ToString());
                child.FindChild("Name").GetComponent<Text>().text = "Level " + level_index;
                child.GetComponent<LevelClickHandler>().level_index = level_index;
                if (finish_seconds > 0)
                {
                    int minutes = Convert.ToInt32(finish_seconds) / 60;
                    int seconds = Convert.ToInt32(finish_seconds) - minutes * 60;
                    string minutes_str = minutes.ToString();
                    if (minutes_str.Length == 1)
                    {
                        minutes_str = "0" + minutes_str;
                    }
                    string seconds_str = seconds.ToString();
                    if (seconds_str.Length == 1)
                    {
                        seconds_str = "0" + seconds_str;
                    }
                    child.FindChild("Time").GetComponent<Text>().text = minutes_str + ":" + seconds_str;
                    child.GetComponent<Image>().sprite = finished_sprite;
                }
                else
                {
                    child.GetComponent<Image>().sprite = default_sprite;
                    child.FindChild("Time").GetComponent<Text>().text = "00:00";
                }
            }
        }
    }
    public void OnTap(string btn)
    {
        SoundManager.instance.PlayButtonClickSound();
        if (btn == "main_start")
        {
            SetupLevelButtons();
            level_selector.SetActive(true);
            main_menu_panel.SetActive(false);
            camera_target_transform = second_camera_transform;
        }
        else if (btn == "main_setting")
        {
            Debug.Log("not support");
        }
        else if (btn == "main_sound")
        {
            SoundManager.instance.FlipSoundState();
        }
        else if (btn == "main_bloom")
        {
            bool enabled = false;
            if (PlayerPrefs.HasKey(CONST.BLOOM_IMAGE_EFFECT_KEY))
            {
                enabled = PlayerPrefs.GetInt(CONST.BLOOM_IMAGE_EFFECT_KEY) == 1;
                enabled = !enabled;
            }
            else
            {
                enabled = true;
            }
            Camera.main.GetComponent<BloomOptimized>().enabled = enabled;
            PlayerPrefs.SetInt(CONST.BLOOM_IMAGE_EFFECT_KEY, enabled ? 1 : 0);
        }
        else if (btn == "main_contact")
        {
            contact_info_panel.SetActive(true);
            main_menu_panel.SetActive(false);
        }
        else if (btn == "contact_back")
        {
            contact_info_panel.SetActive(false);
            main_menu_panel.SetActive(true);
        }
        else if (btn == "main_exit")
        {
            Application.Quit();
        }
        else if (btn == "selector_back")
        {
            level_selector.SetActive(false);
            main_menu_panel.SetActive(true);
            camera_target_transform = first_camera_transform;
        }
        else if (btn == "selector_pre")
        {
            if (page_no == 0)
            {
                return;
            }
            page_no--;
            SetupLevelButtons();
        }
        else if (btn == "selector_next")
        {
            // 计算下一页的第一个index
            int first_index = (page_no + 1) * DATA.PAGE_LEVEL_COUNT;
            if (first_index >= DATA.LEVEL_COUNT)
            {
                return;
            }
            page_no++;
            SetupLevelButtons();
        }
           
        else
        {
            Debug.Log(btn);
        }
    }
    void Awake()
    {
        if (global_light_strength < 0)
        {
            global_light_strength = RenderSettings.ambientIntensity;
            global_light_color = RenderSettings.ambientLight;
        }
        else
        {
            RenderSettings.ambientIntensity = global_light_strength;
            RenderSettings.ambientLight = global_light_color;
        }
        // apply settings
        bool enabled = false;
        if (PlayerPrefs.HasKey(CONST.BLOOM_IMAGE_EFFECT_KEY))
        {
            enabled = PlayerPrefs.GetInt(CONST.BLOOM_IMAGE_EFFECT_KEY) == 1;
        }
        Camera.main.GetComponent<BloomOptimized>().enabled = enabled;
    }
	// Use this for initialization
	void Start () {
        camera_target_transform = first_camera_transform;
        level_selector.SetActive(false);
        contact_info_panel.SetActive(false);
        int first_level = DATA.GetFirstUnfinishedLevel();
        if (first_level < 0)
        {
            page_no = 0;
        }
        else
        {
            page_no = (first_level - 1) / DATA.PAGE_LEVEL_COUNT;
        }
	}
	
	// Update is called once per frame
	void Update () {
        float speed = 5;
        camera_transform.position = Vector3.Lerp(camera_transform.position, camera_target_transform.position, speed * Time.deltaTime);
        camera_transform.rotation = Quaternion.Lerp(camera_transform.rotation, camera_target_transform.rotation, speed * Time.deltaTime);
	}
}
