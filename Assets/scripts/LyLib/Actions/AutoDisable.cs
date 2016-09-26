using UnityEngine;
using System.Collections;

public class AutoDisable : MonoBehaviour {
	void Start () {
        StartCoroutine(DisableDelayed());
    }
    IEnumerator DisableDelayed()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        gameObject.SetActive(false);
	}
}
