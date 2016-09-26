using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/* 
 * 简单的动画直接使用这个组件即可。
 * 复杂动画需要手动创建_SpriteAnimation。
 */
public class SpriteAnimation : MonoBehaviour {
	public int count_row = 1, count_column = 1;
	public float frame_rate = 0.1f;
	public int[] frames;
	private _SpriteAnimation anim;
	// Use this for initialization
	void Start () {
		anim = new _SpriteAnimation (gameObject.GetComponent<MeshRenderer>().sharedMaterial, 
		                            count_row, count_column, frame_rate,
		                            frames);
	}
	void Update () {
		anim.Update ();
	}
}

// 使用standard specular & transparent/fade设置的materials来渲染plane上的texture！
public class _SpriteAnimation
{
    private Material mat;// 定义材质，必须具备offset属性。
    private float cw, ch;// 定义每一帧的长宽。
    private float frameRate = 0.2f;
    private List<Rect> rects = new List<Rect>(); // 定义从起始和终止行列，例如(1,1,3,4)代表从第1行第1列到第3行第4列是动画帧。
    public _SpriteAnimation(Material mat, int rows, int cols, float rate, params int[] frames)
    {
        Debug.Log(mat.mainTexture.width + "&" + mat.mainTexture.height);
        this.mat = mat;
        this.cw = mat.mainTexture.width / cols;
        this.ch = mat.mainTexture.height / rows;
        this.frameRate = rate;
        for (int i = 0; i < frames.Length; i += 4)
        {
            AddFrames(frames[i], frames[i + 1], frames[i + 2], frames[i + 3]);
        }
        mat.SetTextureScale("_MainTex", new Vector2(1.0f / cols, 1.0f / rows));// tiling
    }
    public void AddFrames(int r1, int c1, int r2, int c2)
    {
        for (int r = r1 - 1; r < r2; ++r)
        {
            for (int c = c1 - 1; c < c2; ++c)
            {
                float x = c * cw;
                float y = mat.mainTexture.height - (r + 1) * ch;
                rects.Add(new Rect(
                    x, y, 0, 0
                ));
            }
        }
    }
    private float timer = 0;
    private int cur_index = 0;
    public void Update()
    {
        timer += Time.deltaTime;
        if (timer > frameRate)
        {
            timer = 0;
            cur_index++;
            if (cur_index >= rects.Count) { cur_index = 0; }
            Rect rect = rects[cur_index];

            float offset_x = rect.x / mat.mainTexture.width;
            float offset_y = rect.y / mat.mainTexture.height;
            mat.SetTextureOffset("_MainTex", new Vector2(offset_x, offset_y));// offset
        }
    }
    public void Reset()
    {
        timer = 0;
    }
}
