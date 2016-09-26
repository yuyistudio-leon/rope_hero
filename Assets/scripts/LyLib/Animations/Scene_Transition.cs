using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using LyLib;

/*
 * 添加到摄像机或者别的物体上，指定所需的一个用来遮罩的CanvasGroup。
 * （转场的时候, StartTransition被Scene_LoadAsync自动调用。）
 */
public class Scene_Transition : MonoBehaviour {
    float fade_in_duration = 0.2f;
    float fade_out_duration = 0.3f;
    public CanvasGroup cavans_group;
    public Tween.EaseType fade_in_ease_type = Tween.EaseType.easeInOutSine, fade_out_ease_type = Tween.EaseType.easeOutSine;
    public bool fade_in_on_load = true;
    public static Scene_Transition instance;
    public Tween.TweenBase tween;

    public Scene_Transition()
    {
        instance = this;
    }

    bool running = false;
    public void Start()
    {
        if (fade_in_on_load)
        {
            FadeIn();
        }
    }
    void Update()
    {
        if (tween != null)
        {
            tween.OnUpdate();
        }
    }
    public void FadeIn()
    {
        cavans_group.alpha = 1;
        tween = new Tween.Float().From(1).To(0).Time(fade_in_duration).OnUpdate(_Scene_Transition__OnUpdate)
            .Ease(fade_in_ease_type);
    }

	// Use this for initialization
	public void StartTransition () {
        if (running) return;
        running = true;
        cavans_group.alpha = 0;
        tween = new Tween.Float().From(0).To(1).Time(fade_out_duration).OnComplete(_Scene_Transition__OnCameraFadeComplete)
            .OnUpdate(_Scene_Transition__OnUpdate).Ease(fade_out_ease_type);
	}
	
	// Update is called once per frame
	public void _Scene_Transition__OnCameraFadeComplete () {
        Scene_LoadAsync.EnterLoadingLevel();
	}
    // Update is called once per frame
    public void _Scene_Transition__OnUpdate(float value)
    {
        cavans_group.alpha = value;
    }
}
