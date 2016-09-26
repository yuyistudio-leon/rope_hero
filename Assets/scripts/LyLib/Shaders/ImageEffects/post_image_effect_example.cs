using UnityEngine;
using System.Collections;

// 支持渲染多个shader特效！
public class post_image_effect_example : MonoBehaviour {
	/// Provides a shader property that is set in the inspector
	/// and a material instantiated from the shader
	public Shader[] shaders;
	private Material[] m_Materials;
	private int mat_count = 0;
	
	RenderTexture rt;
	void OnEnable() {
		// 开启相机的depth buffer。
		GetComponent<Camera>().depthTextureMode |= DepthTextureMode.Depth;     
	}
	// 检查是否支持Image effect
	private bool CheckValidity(){
		// Disable if we don't support image effects
		if (!SystemInfo.supportsImageEffects) {
			enabled = false;
			return false;
		}
		return true;
	}
	// 将每个有效的shander都放到一个material里面
	private void PrepareMaterials(){
		m_Materials = new Material[shaders.Length];
		for (int i = 0; i < shaders.Length; ++i) {
			if (shaders[i] && shaders[i].isSupported){
				m_Materials[mat_count] = new Material (shaders[i]);
				m_Materials[mat_count].hideFlags = HideFlags.HideAndDontSave;// dont save表示随着游戏结束，mat也随之销毁，但是游戏运行时可以进行编辑。HideAndDontSave则更狠，游戏运行时也看不到，不能编辑。
				mat_count++;
			}
		}
		if (mat_count == 0) {
			enabled = false;
		}
	}
	protected virtual void Start ()
	{
		if (CheckValidity ()) {
			PrepareMaterials ();
		}
		if (! enabled) {
			Debug.LogError ("The post image effect is not working!");
		}
		// 因为destination RenderTexture貌似不可读，所以弄了一个中间变量。
		rt = new RenderTexture (1024,768,21);
	}
	
	protected virtual void OnDisable() {
		for (int i = 0; i < mat_count; ++i){
			Destroy( m_Materials[i] );
		}
	}
	void OnRenderImage (RenderTexture source, RenderTexture destination) {
		// apply every material
		for (int i = 0; i < mat_count - 1; ++i) {
			if (i % 2 == 0){
				Graphics.Blit (source, rt, m_Materials[i]);
			}else{
				Graphics.Blit (rt, source, m_Materials[i]);
			}
		}
		if ((mat_count - 1) % 2 == 1) {
			Graphics.Blit (rt, destination, m_Materials[mat_count - 1]);
		} else {
			Graphics.Blit (source, destination, m_Materials[mat_count - 1]);
		}
	}
}
