using UnityEngine;
using System.Collections;
namespace LyLib
{
    public class Rotate_Simple : MonoBehaviour
    {
        public float angle_per_second = 180;
        public Vector3 axis = Vector3.up;
        public Space space = Space.World;

        void FixedUpdate()
        {
            transform.Rotate(axis, angle_per_second * Time.deltaTime, space);
        }
    }
}