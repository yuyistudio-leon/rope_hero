using UnityEngine;
using System.Collections;
using LyLib;

public class Translate_Auto : MonoBehaviour {
    public Vector3 speed_dir = Vector3.forward;
    public float speed = 1;

	// Update is called once per frame
	void FixedUpdate () {
        Vector3 pos = transform.position + speed_dir.normalized.Multi(speed * Time.fixedDeltaTime);
        transform.position = pos;
	}
}
