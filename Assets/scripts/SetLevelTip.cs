using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SetLevelTip : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponent<Text>().text = "Level  " + LoadLevel.level;
	}
}
