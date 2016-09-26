using UnityEngine;
using System.Collections;

public class Rotate_SmoothVertical : MonoBehaviour
{
    public float target_angle = 0;
    public float speed_factor = 3;

    private float current_angle = 0;

	// Update is called once per frame
	void FixedUpdate () {
        if (Mathf.Abs(target_angle - current_angle) > 180)
        {
            target_angle -= 360;
        }
        current_angle = Mathf.Lerp(current_angle, target_angle, Time.fixedDeltaTime * speed_factor);
        transform.rotation = Quaternion.AngleAxis(current_angle, Vector3.up);
	}
}
