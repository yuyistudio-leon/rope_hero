using UnityEngine;
using System.Collections;

public static class MaxScore {
    const string max_score_key = "_____max_score_key_name_unique_____";
    public static int GetMaxScore(int current_score = 0, string key = "default") 
    {
        key = max_score_key + key;
        int max_score = 0;
        if (PlayerPrefs.HasKey(key)) {
            max_score = PlayerPrefs.GetInt(key);
        }
        if (current_score > max_score)
        {
            max_score = current_score;
        }
        PlayerPrefs.SetInt(key, max_score);
        return max_score;
    }
    public static void SetMaxScore(int max_score, string key = "default")
    {
        PlayerPrefs.SetInt(max_score_key + key, max_score);
    }
}
