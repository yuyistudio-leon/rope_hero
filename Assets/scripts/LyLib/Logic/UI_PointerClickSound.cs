using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class UI_PointerClickSound : MonoBehaviour, IPointerClickHandler {

    public void OnPointerClick(PointerEventData eventData)
    {
        SoundManager.instance.PlayButtonClickSound();
    }
}
