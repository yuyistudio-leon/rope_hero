using UnityEngine;
using System.Collections;
using LyLib;

public class Timer {
    public float max_time = 3;
    float current_time;
    public bool Timeout()
    {
        current_time += Time.deltaTime;
        return current_time > max_time;
    }
    public void Reset()
    {
        current_time = 0;
    }
}
public class RandomTimer
{
    public float min_time = .1f, max_time = 3;
    float total_time;
    float current_time;
    public RandomTimer()
    {
        Reset();
    }
    public bool Timeout()
    {
        current_time += Time.deltaTime;
        return current_time > total_time;
    }
    public void Reset()
    {
        total_time = LM.RandomFloat(min_time, max_time);
        current_time = 0;
    }
}