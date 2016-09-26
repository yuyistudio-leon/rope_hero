using UnityEngine;
using System.Collections;

public class BKMusicController : MonoBehaviour {
	void Start () {
        GetComponent<AudioSource>().enabled = SoundManager.instance.IsSoundOn();
	}
}
