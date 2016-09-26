using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
合并所有孩子节点！
*/
public class combine_mesh : MonoBehaviour {

	// Use this for initialization
	void Start2 () {
		// get children mesh filters
		MeshFilter[] allMeshFilters = GetComponentsInChildren<MeshFilter>();
		MeshFilter[] meshFilters = new MeshFilter[allMeshFilters.Length - 1];
		for (int i = 0; i < meshFilters.Length; ++i) {
			meshFilters [i] = allMeshFilters [i + 1];
		}
		// assign mesh filters to combine array
		CombineInstance[] combine = new CombineInstance[meshFilters.Length];
        float factor = 2;
		for (int i = 0; i < meshFilters.Length; ++i){
			print (meshFilters[i].name);
			combine[i].mesh = meshFilters[i].mesh;
			combine[i].transform = Matrix4x4.TRS(new Vector3(i * factor, 0, 0), new Quaternion(0,0,0,1), new Vector3(1,1,1));
            //.SetActive(false);

            meshFilters[i].GetComponent<MeshRenderer>().enabled = false;
		}
		// combine all the meshes
		Mesh mesh = new Mesh();
		mesh.CombineMeshes(combine, true, true);
		// assign the result mesh to current game object
		GetComponent<MeshFilter> ().mesh = mesh;

		gameObject.SetActive (true);
	}
	void Start ()
	{
		//---------------- 先获取材质 -------------------------
		//获取自身和所有子物体中所有MeshRenderer组件
		MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();  
		//新建材质球数组
		Material[] mats = new Material[meshRenderers.Length];  
		for (int i = 0; i < meshRenderers.Length; i++) {
			//生成材质球数组 
			mats[i] = meshRenderers[i].sharedMaterial;   
		}
		//---------------- 合并 Mesh -------------------------
		//获取自身和所有子物体中所有MeshFilter组件
		MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();  
		CombineInstance[] combine = new CombineInstance[meshFilters.Length];   
		for (int i = 0; i < meshFilters.Length; i++) {
			combine[i].mesh = meshFilters[i].sharedMesh;
			//矩阵(Matrix)自身空间坐标的点转换成世界空间坐标的点 
			combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
			meshFilters[i].gameObject.SetActive(false);
		} 
		//为新的整体新建一个mesh
		transform.GetComponent<MeshFilter>().mesh = new Mesh(); 
		//合并Mesh. 第二个false参数, 表示并不合并为一个网格, 而是一个子网格列表
		transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine, false);
		transform.gameObject.SetActive(true);

		//为合并后的新Mesh指定材质 ------------------------------
		transform.GetComponent<MeshRenderer>().sharedMaterials = mats; 
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
