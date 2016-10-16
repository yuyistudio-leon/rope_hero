using UnityEngine;
using System.Collections;

public class FillCanvasGroupdAlpha : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponent<CanvasGroup>().alpha = 1;
	}
}
