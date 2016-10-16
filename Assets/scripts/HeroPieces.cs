using UnityEngine;
using System.Collections;

public class HeroPieces : MonoBehaviour {
    public GameObject piece_prefab_obj;
    float size = 3f;
	// Use this for initialization
	void Start () {
        if (!piece_prefab_obj)
        {
            return;
        }
        float piece_length = 1f / size + (Random.value - 0.5f) * 2 * 0.06f;
        for (int i = 0; i < size; ++i)
        for (int j = 0; j < size; ++j)
        for (int k = 0; k < size; ++k)
        {
            var piece = GameObject.Instantiate(piece_prefab_obj);
            piece.transform.SetParent(transform, false);
            piece.transform.localScale = new Vector3(piece_length, piece_length, piece_length);
            piece.transform.localPosition = new Vector3((i - size / 2) * piece_length, (j - size / 2) * piece_length, (k - size / 2) * piece_length);
        }
	}
}
