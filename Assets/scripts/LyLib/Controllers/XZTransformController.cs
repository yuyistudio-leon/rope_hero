using UnityEngine;
using System.Collections;
using LyLib;

public class XZTransformController : MonoBehaviour {
	public float speed = 3;
	private Vector3 control_dir;

	// Update is called once per frame
	void Update () {

		float dx = 0, dz = 0;
		if (Input.GetKey(KeyCode.UpArrow)) {
			dz = 1;
		} else if (Input.GetKey(KeyCode.DownArrow)) {
			dz = -1;
		}
		if (Input.GetKey(KeyCode.LeftArrow)) {
			dx = -1;
		} else if (Input.GetKey(KeyCode.RightArrow)) {
			dx = 1;
		}
		control_dir = new Vector3 (dx, 0, dz);
	}
	void FixedUpdate() 
    {
        transform.Translate(control_dir.Multi(speed * Time.deltaTime));
	}
}
