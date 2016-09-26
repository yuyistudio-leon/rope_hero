using UnityEngine;
using System.Collections;

public class Rotate_Pingpong : MonoBehaviour {
    public Transform target;
    public float from_angle = -20, to_angle = 20;
    public float duration = 3;

	// Use this for initialization
	void Start () {
        if (target == null) {
            target = transform;
        }
        Quaternion init_rot = target.rotation;
        (new Tween.Float()).From(from_angle).To(to_angle)
            .OnUpdate(
                v => target.rotation = init_rot * Quaternion.AngleAxis(v, Vector3.up)
            )
            .PingPong()
            .Ease(Tween.EaseType.linear)
            .Time(duration)
            .Start();
	}
}
