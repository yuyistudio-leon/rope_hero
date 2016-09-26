using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace LyLib
{
    public class LAnim : MonoBehaviour
    {
        public static void AddEvent(Animation anim, string clip_name, string callback_name, float time_percent)
        {
            var clip = anim.GetClip(clip_name);
            if (clip == null)
            {
                Debug.LogError("cannot found clip named " + clip_name + " on " + anim.transform.parent.name);
                return;
            }
            var e = new AnimationEvent();
            e.functionName = callback_name;
            e.time = clip.length * time_percent;
            clip.AddEvent(e);
        }
    }
}