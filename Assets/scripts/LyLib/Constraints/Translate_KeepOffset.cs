using UnityEngine;
using System.Collections;


namespace LyLib
{
    public class Translate_KeepOffset : MonoBehaviour
    {
        public Transform target;
        public Vector3 offset = new Vector3(0f, 7.5f, 0f);


        private void LateUpdate()
        {
            if (target != null)
            {
                transform.position = target.position + offset;
            }
        }
    }
}