using UnityEngine;
using System.Collections;
using LyLib;

public class Translate : MonoBehaviour {
    Tween.TweenBase tween, tween_saved;
    Vector3 start_pos, end_pos;
    float loop_gap_timer = 0;

    public Tween.EaseType ease_type = Tween.EaseType.easeInOutCubic;
    public Tween.LoopType loop_type = Tween.LoopType.pingPong;
    public float duration = 2f;
    public float loop_time_gap = 0f;
    public float start_delay = 0f;
    public bool auto_start = true;
    public Vector3 delta_pos;
    
	// Use this for initialization
	void Start () {
        start_pos = transform.position;
        end_pos = start_pos + delta_pos;
        if (auto_start)
        {
            StartCoroutine(StartDelay());
        }
	}
    IEnumerator StartDelay()
    {
        yield return new WaitForSeconds(start_delay);
        StartTranslation();
    }
    void StartTranslation()
    {
        tween = new Tween.Float().Time(duration).OnUpdate(OnTweenUpdate).Ease(ease_type).Loop(loop_type).OnComplete(OnTweenComplete);
    }

    void OnTweenComplete()
    {
        loop_gap_timer = loop_time_gap;
    }
    void OnTweenUpdate(float v)
    {
        transform.position = Vector3.Lerp(start_pos, end_pos, v);
    }
	// Update is called once per frame
    void Update()
    {
        if (loop_gap_timer > 0)
        {
            loop_gap_timer -= Time.deltaTime;
            return;
        }
        if (tween != null)
        {
            tween.OnUpdate();
        }
    }
}
