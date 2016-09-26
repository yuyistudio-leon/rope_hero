using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour {
    public AudioClip ui_click_sound;
    public static SoundManager instance;

    float click_sound_volume_scale = 2;
    const float andio_source_volume = 1;
    AudioSource audio_source;
    const string SOUND_CONFIG_KEY = "__@_@_is_sound_on_key_O_o__";

    SoundManager()
    {
        instance = this;
    }

    void Awake()
    {
        audio_source = GetComponent<AudioSource>();
        audio_source.volume = andio_source_volume;
        InitSound();
    }
    void Start()
    {
    }
    void InitSound()
    {
        if (PlayerPrefs.HasKey(SOUND_CONFIG_KEY))
        {
            bool sound_on = PlayerPrefs.GetInt(SOUND_CONFIG_KEY) == 1;
            if (!sound_on)
            {
                Debug.Log("SOUND MUTE!!!!");
                audio_source.mute = true;
            }
        } else {
            PlayerPrefs.SetInt(SOUND_CONFIG_KEY, 1);
        }
    }
    public bool IsSoundOn()
    {
        return PlayerPrefs.GetInt(SOUND_CONFIG_KEY) == 1;
    }
    public bool FlipSoundState()
    {
        bool sound_on = PlayerPrefs.GetInt(SOUND_CONFIG_KEY) == 1;
        sound_on = !sound_on;
        audio_source.mute = !sound_on;
        PlayerPrefs.SetInt(SOUND_CONFIG_KEY, sound_on ? 1 : 0);
        Debug.Log("SOUND: " + sound_on + "!!!");
        return sound_on;
    }
    
    public void PlayButtonClickSound()
    {
        if (ui_click_sound != null)
        {
            audio_source.PlayOneShot(ui_click_sound, click_sound_volume_scale);
        }
    }
    public void StopAudioSource()
    {
        audio_source.Stop();
    }
    public void PlayOneShot_PichAdd(AudioClip clip, float random_amount = 0.3f)
    {
        PlayOneShot(clip, 1 + (1 - 2 * Random.value) * random_amount, 1 + Random.value * random_amount);
    }
    public void PlayOneShot(AudioClip clip, float random_amount = 0.3f)
    {
        PlayOneShot(clip, 1 + (1 - 2 * Random.value) * random_amount, 1 + (1 - 2 * Random.value) * random_amount);
    }
    public void PlayOneShot(AudioClip clip, float volume, float pitch)
    {
        if (clip == null || audio_source == null)
        {
            return;
        }
        audio_source.pitch = pitch;
        audio_source.PlayOneShot(clip, volume);
    }
}
