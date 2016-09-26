using UnityEngine;
using System.Collections;
using LyLib;

[RequireComponent(typeof(Camera))]
public class Camera_BkColorChange : MonoBehaviour
{
    public Camera cam;
    public float color_min = 0.1f;
    public float color_max = 1.0f;
	public Color current_color = Color.black;
    public bool start_on_load = true;
    public float change_duration = 2;

    private Tween.TweenBase tween;
	private Color next_color;

	// Use this for initialization
	void Start () {
        if (cam == null)
    		cam = GetComponent<Camera> ();
        if (cam == null)
            cam = Camera.main;
        if (start_on_load)
    		ChangeColor ();
	}
	public void UpdateCallback(float percent){
        cam.backgroundColor = Color.Lerp(current_color, next_color, percent);
	}
	public void Callback(){
		current_color = next_color;
		ChangeColor ();
	}
    void Update()
    {
        if (tween != null)
        {
            tween.OnUpdate();
        }
    }
	void ChangeColor(){
        next_color = next_color.Randomize(color_min, color_max);
        tween = new Tween.Float()
            .From(0)
            .To(1)
            .Ease(Tween.EaseType.linear)
            .Time(change_duration)
            .OnComplete(Callback)
            .OnUpdate(UpdateCallback);
	}
    
}
