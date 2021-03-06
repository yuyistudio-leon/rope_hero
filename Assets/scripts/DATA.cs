﻿using UnityEngine;
using System.Collections;

public class DATA : MonoBehaviour 
{
    public static int LEVEL_COUNT = 14;
    public static int PAGE_LEVEL_COUNT = 6;
    public static int GetFirstUnfinishedLevel()
    {
        for (int i = 0; i < LEVEL_COUNT; ++i)
        {
            var key = "level" + (i + 1).ToString() + ".time";
            if (PlayerPrefs.HasKey(key) == false)
            {
                return i + 1;
            }
        }
        return -1;
    }
    /*
     * return -1 if not finished
     */
    public static float IsLevelFinished(string name)
    {
        var key = "level" + name + ".time";
        if (PlayerPrefs.HasKey(key))
        {
            return PlayerPrefs.GetFloat(key);
        }
        else
        {
            return -1;
        }
    }
    public static void RecordLevelFinished(float seconds)
    {
        int level = LoadLevel.level;
        print("record level : " + level);
        var key = "level" + level + ".time";
        if (PlayerPrefs.HasKey(key))
        {
            var last_time = PlayerPrefs.GetFloat(key);
            if (last_time > seconds)
            {
                PlayerPrefs.SetFloat(key, seconds);
            }
        }
        else
        {
            PlayerPrefs.SetFloat(key, seconds);
        }
    }
}
