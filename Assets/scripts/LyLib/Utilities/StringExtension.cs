using UnityEngine;
using System.Collections;

namespace LyLib
{
    public static class StringExtension
    {
        public static string Join(this string[] strs, char seperator = ' ')
        {
            if (strs.Length == 0)
            {
                return "";
            }
            string result = "";
            for (int i = 0; i < strs.Length - 1; ++i)
            {
                result += strs[i] + seperator;
            }
            result += strs[strs.Length - 1];
            return result;
        }
    }
}
