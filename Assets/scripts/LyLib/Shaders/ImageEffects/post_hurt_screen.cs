using UnityEngine;
using System.Collections;

public class post_hurt_screen : MonoBehaviour {
	/// Provides a shader property that is set in the inspector
	/// and a material instantiated from the shader
	public Shader   shader;
	public float strength = 1.0f; // 0.1 ~ 1.0
	private Material m_Material;
	
	void OnEnable() {
		GetComponent<Camera>().depthTextureMode |= DepthTextureMode.Depth;        
	}
	
	protected virtual void Start ()
	{
		// Disable if we don't support image effects
		if (!SystemInfo.supportsImageEffects) {
			enabled = false;
			return;
		}
		
		// Disable the image effect if the shader can't
		// run on the users graphics card
		if (!shader || !shader.isSupported)
			enabled = false;

		if (! enabled) {
			Debug.LogError ("POST_MIST SCRIPT IS NOT WORKING!");
		}
	}
	
	protected Material material {
		get {
			if (m_Material == null) {
				m_Material = new Material (shader);
				//m_Material.hideFlags = HideFlags.HideAndDontSave;
				m_Material.hideFlags = HideFlags.HideAndDontSave;// dont save表示随着游戏结束，mat也随之销毁，但是游戏运行时可以进行编辑。HideAndDontSave则更狠，游戏运行时也看不到，不能编辑。
			}
			return m_Material;
		} 
	}
	
	protected virtual void OnDisable() {
		if( m_Material ) {
			//DestroyImmediate( m_Material );
			Destroy( m_Material );
		}
	}
	void OnRenderImage (RenderTexture source, RenderTexture destination) {
		material.SetFloat ("strength_factor", strength);
		Graphics.Blit (source, destination, material);
	}
}
