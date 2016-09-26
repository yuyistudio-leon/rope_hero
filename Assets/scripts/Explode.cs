using UnityEngine;
using System.Collections;
using LyLib;

public class Explode : MonoBehaviour {

	// Use this for initialization
	void Start () {
        var force = (transform.position - transform.parent.position).normalized + Random.insideUnitSphere.Multi(0.8f);
        GetComponent<Rigidbody>().AddForce(force.Multi(10), ForceMode.Impulse);
	}
	
}
