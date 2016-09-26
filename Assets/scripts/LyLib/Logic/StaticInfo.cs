using UnityEngine;
using System.Collections;

/*
 * 保存游戏的一些全局信息，类似于PlayerPrefs
 * */
public static class StaticInfo {
    public static void SetInt(string key, int value) {
        PlayerPrefs.SetInt(key, value);
    }
    public static int GetInt(string key)
    {
        return PlayerPrefs.GetInt(key);
    }
    public static void SetFloat(string key, float value)
    {
        PlayerPrefs.SetFloat(key, value);
    }
    public static float GetFloat(string key)
    {
        return PlayerPrefs.GetFloat(key);
    }
    public static void SetString(string key, string value)
    {
        PlayerPrefs.SetString(key, value);
    }
    public static string GetString(string key)
    {
        return PlayerPrefs.GetString(key);
    }
    public static bool HasKey(string key)
    {
        return PlayerPrefs.HasKey(key);
    }
}
