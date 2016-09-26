using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/*
 * 使用方法：
 * 新建一个scene，调节成全黑，命名为Loading。
 * 然后再其他scene中调用 LoadWithTransition(next_level)。
 * 其他scene需要添加Scene_Transition组件，会被自动调用来进行转场。
 */
public class Scene_LoadAsync : MonoBehaviour {
    private static string next_level = "";
    public static string tip_text = "";
    public Text tip_text_view;

    private static string last_level_name;
    public static string LastLevelName
    {
        get
        {
            return last_level_name;
        }
    }

    public static void EnterLoadingLevel()
    {
        Application.LoadLevel("Loading");
    }
    public static void LoadWithTransition(string level, string tip = "Loading ...")
    {
        last_level_name = Application.loadedLevelName;
        tip_text = tip;
        next_level = level;
        if (Scene_Transition.instance == null)
        {
            Application.LoadLevel("Loading");
        }
        else 
        {
            Scene_Transition.instance.StartTransition();
        }
    }

	// Use this for initialization
	void Start () {
        StartCoroutine(LoadAsync());
        if (tip_text != null)
        {
            tip_text_view.text = Scene_LoadAsync.tip_text;
        }
	}
	
	// Update is called once per frame
	IEnumerator LoadAsync () {
        var v = Application.LoadLevelAsync(next_level);
        yield return v;
	}
}
