using UnityEngine;
using System.Collections;
using LyLib;

[ExecuteInEditMode]
public class Light_DayNightSeason : MonoBehaviour {
    public int season_count = 4;
    const int MAX_SEASON_COUNT = 10;
    public string[] season_names = new string[MAX_SEASON_COUNT];
    public Color[] season_colors = new Color[MAX_SEASON_COUNT];
    public int[] season_days = new int[MAX_SEASON_COUNT];
    public float day_minutes = 1;
    public float night_minutes = 1;
    public float day_strength = 1;
    public float night_strength = 0;
    public Light directional_light;
    public float change_light_strength_factor = 5;

    float day_timer = 0;
    float light_strength = 1;
    float target_light_strength = 1;
    int season_index = 0;


    void UpdateArraySize<T>(ref T[] origin)
    {
        var tmp = new T[season_count];
        for (int i = 0; i < tmp.Length && i < origin.Length; ++i)
        {
            tmp[i] = origin[i];
        }
        origin = tmp;
    }
    public void OnUpdateSeasonCount(int new_season_count)
    {
        season_count = (int)Mathf.Clamp(new_season_count, 1, MAX_SEASON_COUNT);
    }
    void Start()
    {
        if (directional_light == null)
        {
            directional_light = GetComponent<Light>();
        }
    }

	// Update is called once per frame
	void Update () {
        light_strength = Mathf.Lerp(light_strength, target_light_strength, Time.deltaTime * change_light_strength_factor);
        Color season_color = season_colors[season_index];
        directional_light.color = season_color.Multi(light_strength);

        if (day_timer > day_minutes * 60)
        {
            target_light_strength = 0;
        }
        if (day_timer > (day_minutes + night_minutes) * 60)
        {
            day_timer = 0;
            target_light_strength = 1;
        }

        day_timer += Time.deltaTime;
	}
}
