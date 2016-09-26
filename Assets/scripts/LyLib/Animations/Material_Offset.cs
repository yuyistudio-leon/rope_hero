using UnityEngine;
using System.Collections;

/*
 * 用来制作移动的背景
 */
public class Material_Offset : MonoBehaviour {

	public bool use_shared_material = true;
	public float x_speed = 0.4f;
	public float y_speed = 0.4f;

	Material mat;
	Vector2 offset;

	void Start () {
		if (use_shared_material) {
			mat = GetComponent<MeshRenderer> ().sharedMaterial;
		} else {
			mat = GetComponent<MeshRenderer> ().material;
		}
		offset = mat.mainTextureOffset;
	}
	
	void Update () {
		offset.x += x_speed * Time.deltaTime;
		offset.y += y_speed * Time.deltaTime;
		mat.SetTextureOffset("_MainTex", offset);
	}
}
