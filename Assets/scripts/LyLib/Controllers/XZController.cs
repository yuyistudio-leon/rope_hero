using UnityEngine;
using System.Collections;

public class XZController : MonoBehaviour {
	public Rigidbody body;
	public float max_speed = 3;
	public float force = 100;

	private Vector3 control_dir;

    void Start()
    {
        if (body == null)
        {
            body = GetComponent<Rigidbody>();
        }
    }
    public void MoveToDir(Vector2 dir)
    {
        control_dir = new Vector3(dir.x, 0, dir.y);
    }
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
	void FixedUpdate() {
		if (body.velocity.magnitude < max_speed ||  Vector3.Dot(body.velocity, control_dir) < 0) {
			body.AddForce (control_dir.normalized * force);
		}
	}
    public Vector3 GetDir()
    {
        return control_dir;
    }
}
