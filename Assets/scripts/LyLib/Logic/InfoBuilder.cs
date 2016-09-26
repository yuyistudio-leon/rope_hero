using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class InfoBuilder {
    StringBuilder sb = new StringBuilder();
    Dictionary<KeyCode, string> key_info = new Dictionary<KeyCode,string>();
    Dictionary<KeyCode, string> key_interrupt = new Dictionary<KeyCode, string>()
    {
        {KeyCode.Mouse0, "Left Click"},
        {KeyCode.Mouse1, "Right Click"},
        {KeyCode.Mouse2, "Middle Click"},
        {KeyCode.Space, "Space"},
    };
    bool first = true;

    public InfoBuilder() { }
    public InfoBuilder(string line)
    {
        Append(line);
    }
    public void Append(char c)
    {
        if (char.IsWhiteSpace(c))
        {
            return;
        }
        if (first)
        {
            first = false;
        }
        else
        {
            sb.Append('\n');
        }
        sb.Append(c);
    }
    public void AddKey(KeyCode key, string info)
    {
        if (key_info.ContainsKey(key))
        {
            return;
        }
        key_info[key] = info;
    }
    public void Append(string line)
    {
        if (line.Trim() == "")
        {
            return;
        }
        if (first)
        {
            first = false;
        }
        else
        {
            sb.Append('\n');
        }
        sb.Append(line);
    }
    public override string ToString()
    {
        // 输出按键信息
        foreach (var pair in key_info)
        {
            string key_name = "";
            if (key_interrupt.ContainsKey(pair.Key))
            {
                key_name = key_interrupt[pair.Key];
            }
            else
            {
                key_name = pair.Key.ToString();
            }
            if (first)
            {
                first = false;
            }
            else
            {
                sb.Append('\n');
            }
            sb.Append("[");
            sb.Append(key_name);
            sb.Append("] ");
            sb.Append(pair.Value);
        }
        return sb.ToString();
    }
}
