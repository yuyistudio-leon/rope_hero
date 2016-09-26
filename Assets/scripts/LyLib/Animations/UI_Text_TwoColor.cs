using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_Text_TwoColor : MonoBehaviour {
    public Color color1 = Color.red;
    public Color color2 = Color.white;
    public float switch_period = 0.2f;

    Text text;
	// Use this for initialization
	void Start () {
        if (text == null)
        {
            text = GetComponent<Text>();
        }
        StartCoroutine(SwitchTextColor());
	}
    IEnumerator SwitchTextColor()
    {
        while (true)
        {
            text.color = color1;
            yield return new WaitForSeconds(switch_period);
            text.color = color2;
            yield return new WaitForSeconds(switch_period);
        }
    }
}
