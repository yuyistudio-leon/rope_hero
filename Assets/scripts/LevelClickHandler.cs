using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelClickHandler : MonoBehaviour, IPointerClickHandler {
    public int level_index;
    public void OnPointerClick(PointerEventData eventData)
    {
        SoundManager.instance.PlayButtonClickSound();
        Scene_LoadAsync.LoadWithTransition(level_index.ToString(), "Loading level " + level_index + " ...");
    }
}
