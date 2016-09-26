using UnityEngine;
using System.Collections;

namespace LyLib
{

    public static class CoroutineDelay
    {
        public static IEnumerator StartInSeconds(float seconds, Callback callback)
        {
            yield return new WaitForSeconds(seconds);
            callback();
        }
        public static IEnumerator StartAfterFrames(int frames_count, Callback callback)
        {
            for (int i_frame = 0; i_frame < frames_count; ++i_frame)
            {
                yield return new WaitForEndOfFrame();
            }
            callback();
        }
        public static IEnumerator StartAfterFrames<T>(int frames_count, Callback<T> callback, T param)
        {
            for (int i_frame = 0; i_frame < frames_count; ++i_frame)
            {
                yield return new WaitForEndOfFrame();
            }
            callback(param);
        }
    }
}
