using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace LyLib
{
    public class AutoResetBoolean
    {
        bool _is_true = false;
        public bool is_true
        {
            get
            {
                if (_is_true)
                {
                    _is_true = false;
                    return true;
                }
                return false;
            }
            set
            {
                _is_true = value;
            }
        }
    }
    public class Blackboard
    {
        Dictionary<string, object> transforms = new Dictionary<string, object>();
        public object Get(string name)
        {
            if (transforms.ContainsKey(name))
            {
                return transforms[name];
            }
            else
            {
                return null;
            }
        }
        public void Set(string name, object value)
        {
            transforms[name] = value;
        }
        public bool IsTrue(string name)
        {
            var v = Get(name);
            return v != null && (bool)v;
        }
    }
}

