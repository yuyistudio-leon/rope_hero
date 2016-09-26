using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class TextEffect : MonoBehaviour {
	public Text text;
	public float lerp_factor = 0.5f;

	private int counter = 0;
	private RectTransform rt;
	private float scale_min = 1, scale_max = 2, scale_aim = 1;
	// Use this for initialization
	void Start () {
		rt = text.GetComponent<RectTransform> ();
	}
	public void Hit(){
		counter ++;
		scale_aim = scale_max;
		text.text = "Hit " + counter;
	}
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			Hit ();
		}
		if (Mathf.Abs (scale_aim - scale_max) < 0.001f) {
			if (Mathf.Abs (scale_max - rt.localScale.x) < 0.001f) {
				scale_aim = scale_min;
			}
		}
		rt.localScale = Vector3.Lerp (rt.localScale, new Vector3(scale_aim, scale_aim), lerp_factor);
	}
}
