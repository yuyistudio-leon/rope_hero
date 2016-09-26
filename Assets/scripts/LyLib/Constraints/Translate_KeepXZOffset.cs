using UnityEngine;
using System.Collections;


namespace LyLib
{
    public class Translate_KeepXZOffset : MonoBehaviour
    {
        public Transform target;

        Vector3 offset;
        void Start()
        {
            offset = transform.position - target.position;
        }

        private void LateUpdate()
        {
            Vector3 pos = target.position + offset;
            pos.y = transform.position.y;
            transform.position = pos;
        }
    }
}