using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UI_Color_Image2Color : MonoBehaviour {
    public Color color1 = Color.yellow;
    public Color color2 = Color.red;
    public float duration = 0.4f;
    
    float timer = 0;
    bool color1_to_2 = true;

    Image image;
	// Use this for initialization
	void Start () {
        image = GetComponent<Image>();    
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        if (timer > duration)
        {
            timer = 0;
            color1_to_2 = !color1_to_2;
        }
        if (color1_to_2)
        {
            image.color = Color.Lerp(color1, color2, timer / duration);
        }
        else
        {
            image.color = Color.Lerp(color2, color1, timer / duration);
        }
	}
}
