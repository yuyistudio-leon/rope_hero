using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/*
 * 几张图片一次淡入、淡出
 */
public class Logo : MonoBehaviour {
    public Sprite[] sprites;
    public Image image;
    public float duration_before_start = 0.5f;
    public float alpha_1_duration = 0.8f;
    public float sprite_time_gap = 0.2f;
    public float fade_in_speed = 1 / 0.1f;
    public float fade_out_speed = 1 / 0.1f;
    public string next_level;
    public float duration_before_exit = 0f;

	// Use this for initialization
    void Start()
    {
        if (next_level == "")
        {
            Debug.LogError("next level for logo-scene is not set!");
            return;
        }
        image = GetComponent<Image>();
        image.color = new Color(1, 1, 1, 0);
        StartCoroutine(SpriteAnimation());
	}
    IEnumerator SpriteAnimation()
    {
        yield return new WaitForSeconds(duration_before_start);
        for (int i_sprite = 0; i_sprite < sprites.Length; ++i_sprite)
        {
            image.sprite = sprites[i_sprite];
            float alpha = 0;
            while (alpha < 1)
            {
                image.color = new Color(1, 1, 1, alpha);
                alpha += fade_in_speed * Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForSeconds(alpha_1_duration);
            while (alpha > 0)
            {
                image.color = new Color(1, 1, 1, alpha);
                alpha -= fade_out_speed * Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            image.color = new Color(1, 1, 1, 0);
            yield return new WaitForSeconds(sprite_time_gap);   
        }
        yield return new WaitForSeconds(duration_before_exit);

        Scene_LoadAsync.LoadWithTransition(next_level);
    }
}
