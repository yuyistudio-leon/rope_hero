using UnityEngine;
using System.Collections;
using LyLib;

public class LoadLevel : MonoBehaviour {
    Tween.TweenBase tween;
    public GameObject dynamic_object_prefab;
    void Start()
    {
        if (GameObject.Find("Dynamic") == null)
        {
            GameObject.Instantiate(dynamic_object_prefab);
        }
    }
    public void ReloadLevel()
    {
        tween = new Tween.Timer().DoAfter(2, () =>
        {
            Scene_LoadAsync.LoadWithTransition(Application.loadedLevelName, "Reloading ...");
        });
    }
    public void NextLevel()
    {
        tween = new Tween.Timer().DoAfter(2, () =>
        {
            int next_level = int.Parse(Application.loadedLevelName) + 1;
            if (next_level > DATA.LEVEL_COUNT)
            {
                Scene_LoadAsync.LoadWithTransition("menu", "Loading menu ...");
            }
            else
            {
                Scene_LoadAsync.LoadWithTransition(next_level.ToString(), "Loading level " + next_level + " ...");
            }
        });
    }
    // Update is called once per frame
    void Update()
    {
        if (tween != null)
        {
            tween.OnUpdate();
        }
    }
}
