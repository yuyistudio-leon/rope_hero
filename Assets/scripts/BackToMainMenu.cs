using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class BackToMainMenu : MonoBehaviour, IPointerClickHandler {
    public void OnPointerClick(PointerEventData eventData)
    {
        Scene_LoadAsync.LoadWithTransition("menu", "Loading menu ...");
    }
}
