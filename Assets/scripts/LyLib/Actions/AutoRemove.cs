using UnityEngine;
using System.Collections;

public class AutoRemove : MonoBehaviour {
	public float delay = 5.0f;
	// Use this for initialization
	void Start () {
		StartCoroutine (DestroyGameObject ());
	}
	IEnumerator DestroyGameObject(){
		yield return new WaitForSeconds (delay);
		Destroy (gameObject);
	}
}
