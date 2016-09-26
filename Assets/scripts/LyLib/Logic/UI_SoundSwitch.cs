using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_SoundSwitch : MonoBehaviour, IPointerClickHandler {
    public Sprite sprite_sound_off;
    public Sprite sprite_sound_on;

    Image image;
    void Start()
    {
        image = GetComponent<Image>();
        if (SoundManager.instance.IsSoundOn())
        {
            image.sprite = sprite_sound_on;
        }
        else
        {
            image.sprite = sprite_sound_off;
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        SoundManager.instance.PlayButtonClickSound();
        if (image == null)
        {
            return;
        }
        if (SoundManager.instance.FlipSoundState())
        {
            image.sprite = sprite_sound_on;
        }
        else
        {
            image.sprite = sprite_sound_off;
        }
    }
}
