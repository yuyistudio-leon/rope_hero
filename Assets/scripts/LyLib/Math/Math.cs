using UnityEngine;
using System.Collections;
using Math = System.Math;

namespace LyLib
{
    public static class Vector3Extension
    {
        //这里MyNewFunc就是你要的扩展函数，返回值可以自定义
        //注意参数列表，第一个参数前有this关键字，后面的参数对应你扩展方法的参数
        public static Vector3 Multi(this Vector3 v, float factor)
        {
            return new Vector3(v.x * factor, v.y * factor, v.z * factor);
        }
        public static Vector2 XZ(this Vector3 v)
        {
            return new Vector2(v.x, v.z);
        }
        public static Vector3 ToVector3(this Vector2 v)
        {
            return new Vector3(v.x, v.y, 0);
        }
        public static Vector3 SetY(this Vector3 v, float y)
        {
            return new Vector3(v.x, y, v.z);
        }
        public static Vector3 AddY(this Vector3 v, float y_delta)
        {
            return new Vector3(v.x, v.y + y_delta, v.z);
        }
        public static Vector3 Add(this Vector3 v, float value)
        {
            v.x += value;
            v.y += value;
            v.z += value;
            return v;
        }
        public static Vector3 Abs(this Vector3 v)
        {
            v.x = Mathf.Abs(v.x);
            v.y = Mathf.Abs(v.y);
            v.z = Mathf.Abs(v.z);
            return v;
        }
        public static Vector3 Randomize(this Vector3 v, float amount)
        {
            v.x = v.x + (1 - Random.value * 2) * amount;
            v.y = v.y + (1 - Random.value * 2) * amount;
            v.z = v.z + (1 - Random.value * 2) * amount;
            return v;
        }
        public static Vector3 RandomizeCircle(this Vector3 v, float amount)
        {
            float angle = Random.value * 2 * 3.1415927f;
            v.x = v.x + Mathf.Sin(angle) * amount;
            v.z = v.z + Mathf.Cos(angle) * amount;
            return v;
        }
        public static Vector3 Randomize(this Vector3 v)
        {
            v.x = Random.value;
            v.y = Random.value;
            v.z = Random.value;
            return v;
        }
        public static Vector3 Multi(this Vector3 v, Vector3 f)
        {
            v.x *= f.x;
            v.y *= f.y;
            v.z *= f.z;
            return v;
        }
        public static Color AsColor(this Vector3 v)
        {
            return new Color(v.x, v.y, v.z);
        }
        public static float MaxElement(this Vector3 v)
        {
            float max = v.x;
            if (v.y > max) max = v.y;
            if (v.z > max) max = v.z;
            return max;
        }
        public static float MinElement(this Vector3 v)
        {
            float min = v.x;
            if (v.y < min) min = v.y;
            if (v.z < min) min = v.z;
            return min;
        }
    }
    public static class ColorExtension
    {
        public static Color Randomize(this Color c, float min, float max)
        {
            c.r = LM.Map(Random.value, min, max);
            c.g = LM.Map(Random.value, min, max);
            c.b = LM.Map(Random.value, min, max);
            return c;
        }
        public static Color Multi(this Color v, float f)
        {
            return new Color(v.r * f, v.g * f, v.b * f);
        }
    }
    class LM
    {
        public static int RandomInt(int min, int max)
        {
            return (int)Mathf.Floor((max - min + 1) * Random.value * 0.999f + min);
        }
        public static int SelectIndexWithWeight(int[] weight)
        {
            int total = 0;
            foreach (var w in weight)
            {
                total += w;
            }
            float p = 0;
            float v = Random.value;
            for (int i = 0; i < weight.Length; ++i)
            {
                p += (float)weight[i] / total;
                if (v < p)
                {
                    return i;
                }
            }
            return weight.Length - 1;
        }
        public static int RandomIndex(int length)
        {
            return RandomInt(0, length - 1);
        }
        public static float RandomFloat(float min, float max)
        {
            return (max - min) * Random.value + min;
        }
        public static float Map(float value_0_1, float min, float max)
        {
            return min + (max - min) * value_0_1;
        }
        public static Vector3 Map(Vector3 value_0_1, float min, float max)
        {
            value_0_1 = value_0_1.Multi(max - min);
            return value_0_1.Add(min);
        }
        public static float clamp(float v, float min, float max)
        {
            if (v < min) return min;
            if (v > max) return max;
            return v;
        }
        public static int clamp(int v, int min, int max)
        {
            if (v < min) return min;
            if (v > max) return max;
            return v;
        }

        public static float Round(float x, int digits = 0)
        {
            return (float)Math.Round(x, digits);
        }
        public static float Sign(float x)
        {
            return x > 0 ? -1 : 1;
        }
        public static IEnumerable Range(int count) {
            for (int i = 0; i < count; ++i)
            {
                yield return i;
            }
        }
        /**
         * 返回数字值域 [start,end)
         * */
        public static IEnumerable Range(int start, int end)
        {
            for (int i = start; i < end; ++i)
            {
                yield return i;
            }
        }
    }
}