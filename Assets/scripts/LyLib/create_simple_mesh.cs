using UnityEngine;
using System.Collections;

/*
 * 放到一个空物体上，指定一个texture，然后将创建一个plane mesh来显示这个texture。
 * */
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class create_simple_mesh : MonoBehaviour {
	public Texture2D texture;
	// Use this for initialization
	void Start () {
		gameObject.AddComponent<MeshRenderer> ();
		MeshFilter meshFilter = GetComponent<MeshFilter> ();
		MeshRenderer renderer = GetComponent<MeshRenderer> ();

		if (meshFilter == null) {	
			gameObject.AddComponent<MeshFilter> ();
			meshFilter = GetComponent<MeshFilter> ();
		}

		// 创建一个朝向观察者的平面，由两个三角形组成。观察者朝向（0,0,1）方向。
		Vector3[] vertices = new Vector3[]{
			new Vector3(-1,1,-1),// left top
			new Vector3(-1,-1,-1),// left bottom
			new Vector3(1,-1,-1),// right bottom
			new Vector3(1,1,-1)// right top
		};
		Vector2[] uv = new Vector2[]{
			new Vector2 (0, 1),
			new Vector2 (0, 0),
			new Vector2 (1, 0),
			new Vector2 (1, 1)
		};
		Vector3[] normals = new Vector3[]{
			new Vector3 (0, 0, -1),
			new Vector3 (0, 0, -1),
			new Vector3 (0, 0, -1),
			new Vector3 (0, 0, -1)
		};
		int[] indices = new int[]{
			2,1,0,// 顺时针是正面
			3,2,0,
		};

		Mesh newMesh = new Mesh ();
		newMesh.vertices = vertices;
		newMesh.uv = uv;
		newMesh.triangles = indices;
		newMesh.normals = normals;

		meshFilter.mesh = newMesh;

		// 赋予材质
		renderer.materials = new Material[]{
			new Material(Shader.Find("Transparent/Diffuse"))
		};
		renderer.materials [0].mainTexture = texture;
	}
	
	// Update is called once per frame
	void Update () {
		// 材质滚动
		MeshRenderer renderer = GetComponent<MeshRenderer> ();
		renderer.material.SetTextureOffset("_MainTex", new Vector2(Time.time,0));
	}
}
