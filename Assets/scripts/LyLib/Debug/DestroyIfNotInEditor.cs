using UnityEngine;
using System.Collections;

public class DestroyIfNotInEditor : MonoBehaviour
{

	// Use this for initialization
	void Start () {
        if (Application.platform != RuntimePlatform.WindowsEditor)
        {
            Destroy(gameObject);
        }
	}
}
