using UnityEngine;
using System.Collections;
using LyLib;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class Story : MonoBehaviour {
    CanvasGroup canvas_group;
    Tween.TweenBase tween;
    float last_v;
    int index = 0;

    public Tween.EaseType ease_type = Tween.EaseType.linear;
    public AudioSource bk_audio;
    public AudioClip story_bk;
    public float duration = 2;
    public Image image;
    public Text text;
    public Sprite[] images;
    public string[] texts;
    public GameObject root_panel;

    AudioClip origin_clip;
	// Use this for initialization
	void Start () {
        canvas_group = GetComponent<CanvasGroup>();
        tween = new Tween.Float().Time(duration).OnUpdate(OnFloatUpdate).OnComplete(OnFloatComplete).PingPong().Ease(ease_type);
        canvas_group.alpha = 0;

        index = -1;
        ChangeStory();
        origin_clip = bk_audio.clip;
        bk_audio.clip = story_bk;
	}
    void ChangeStory()
    {
        index += 1;
        if (index >= images.Length)
        {
            tween = null;
            if (root_panel)
            {
                image.color = new Color(0, 0, 0, 0);
                text.text = "Loading ...";
                image.transform.parent.GetComponent<Image>().color = new Color(0, 0, 0, 0);
                canvas_group.alpha = 1;
                tween = new Tween.Timer().DoAfter(1, delegate()
                {
                    tween = null;
                    Destroy(root_panel);
                    bk_audio.clip = origin_clip;
                    HeroController.instance.Unfreeze();
                });
            }
            return;
        }
        image.sprite = images[index];
        text.text = texts[index];
    }
    void OnFloatUpdate(float v)
    {
        last_v = v;
        canvas_group.alpha = v;
    }
    void OnFloatComplete()
    {
        if (last_v < 0.01f)
        {
            ChangeStory();
        }
    }
	// Update is called once per frame
	void Update () {
        if (tween != null)
        {
            tween.OnUpdate();
        }
	}
}
