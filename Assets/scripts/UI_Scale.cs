using UnityEngine;
using System.Collections;
using LyLib;

public class UI_Scale : MonoBehaviour {
    private Tween.TweenBase scale_tween;
	// Use this for initialization
	void Start () {
        scale_tween = (new Tween.Float()).OnUpdate(OnScaleUpdate).From(0.9f).To(1f).PingPong().Time(0.6f);
	}

    public void OnScaleUpdate(float v)
    {
        transform.localScale = new Vector3(v, v, v);
    }
	// Update is called once per frame
	void Update () {
        scale_tween.OnUpdate();
	}
}
