using UnityEngine;
using System.Collections;
using UnityEngine.UI;
/*
 * 控制Text的淡入淡出
 */
public class UI_Text_Fade : MonoBehaviour {
    private Vector3 pos, init_pos;
    private RectTransform rect_transform;
    private Text text;
    private float speed = 20;
    public event Callback event_on_complete;
    public CanvasScaler canvas_scaler;

    void Start()
    {
        rect_transform = GetComponent<RectTransform>();
        init_pos = rect_transform.position;
        pos = init_pos;
        speed *= canvas_scaler.scaleFactor;
        text = GetComponent<Text>();
    }
    public void FadeInOut(string str, float in_time, float out_time,
        float gap_time = 0)
    {
        pos = init_pos;
        text.text = str;

        CallbackFloat onupdate_pos = (float v) =>
        {
            pos.y -= Time.deltaTime * speed;
            rect_transform.position = pos;
        };
        CallbackFloat onupdate = (float v) => {
            text.color = new Color(1, 1, 1, 1 - Mathf.Abs(1 - v));
            onupdate_pos(v);
        };
        new Tween.Float().From(0).To(1)
            .OnUpdate(onupdate)
            .Ease(Tween.EaseType.easeInSine)
            .Time(in_time)
            .After(
                 new Tween.Float().From(1).To(2)
                    .Time(gap_time)
                    .OnUpdate(onupdate_pos)
                    .After(
                        new Tween.Float().From(1).To(2)
                            .OnUpdate(onupdate)
                            .Ease(Tween.EaseType.easeOutSine)
                            .Time(out_time)
                            .OnComplete(() =>
                            {
                                if (event_on_complete != null)
                                {
                                    event_on_complete();
                                }
                            })
                    )
            )
            .Start();
    }
}
