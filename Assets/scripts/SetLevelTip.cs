using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SetLevelTip : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponent<Text>().text = "Level  " + Application.loadedLevelName;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
