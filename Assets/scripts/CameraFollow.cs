using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
	public float follow_speed_factor = 7.0f;
	public GameObject target;

    public HeroController hero_controller;

	private Vector3 pos_offset;
	void Start () {
	}

	void FixedUpdate () {
        if (target)
        {
            var offset = pos_offset;
            offset.x = offset.x * (Mathf.Abs(hero_controller.GetSpeed()) * 0.04f + 0.5f);
            Vector3 aim_pos = target.transform.position + offset;
            transform.position = Vector3.Lerp(transform.position, aim_pos, Time.deltaTime * follow_speed_factor);
        }
        else
        {
            target = GameObject.Find("Hero");
            pos_offset = transform.position - target.transform.position;
            hero_controller = target.GetComponent<HeroController>();
        }
	}
}
