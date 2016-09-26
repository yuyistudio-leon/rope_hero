using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace LyLib
{
    public static class UnityExtension
    {
        public static Transform GetChildByName(this Transform obj, string name)
        {
            for (int i = 0; i < obj.childCount; ++i)
            {
                if (obj.GetChild(i).name == name)
                {
                    return obj.GetChild(i);
                }
            }
            return null;
        }
    }

#if UNITY_EDITOR
    public static class E
    {
        public static void IntField(string label, ref int value)
        {
            value = EditorGUILayout.IntField(label, value);
        }
        public static void StringField(ref string value)
        {
            value = EditorGUILayout.TextField(value);
        }
        public static void IntField(string label, ref int value, int min, int max)
        {
            value = LM.clamp(EditorGUILayout.IntField(label, value), min, max);
        }
        public static void IntField(ref int value)
        {
            value = EditorGUILayout.IntField(value);
        }
        public static void FloatField(string label, ref float value)
        {
            value = EditorGUILayout.FloatField(label, value);
        }
        public static void ColorField(string label, ref Color value)
        {
            value = EditorGUILayout.ColorField(label, value);
        }
        /*
        * 尽量保存原有内容
        */
        public static void ResizeArray<T>(ref T[] src, int new_size)
            //where T : new
        {
            if (new_size <= 0)
            {
                Debug.LogError("array size must be greater than zero");
                src = new T[0];
                return;
            }
            if (src.Length == new_size)
            {
                return;
            }

            T[] tmp = new T[new_size];
            for (int i = 0; i < src.Length && i < new_size; ++i)
            {
                tmp[i] = src[i];
            }
            src = tmp;
        }
    }
#endif
}