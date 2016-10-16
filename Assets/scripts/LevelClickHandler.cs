using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelClickHandler : MonoBehaviour, IPointerClickHandler {
    public int level_index;
    public void OnPointerClick(PointerEventData eventData)
    {
        SoundManager.instance.PlayButtonClickSound();
        LoadLevel.level = level_index;
        Scene_LoadAsync.LoadWithTransition("Level", "Loading level " + level_index + " ...");
    }
}
