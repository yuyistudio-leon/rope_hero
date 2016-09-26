using UnityEngine;
using System.Collections;

namespace LyLib
{
    /*
     * 左右摇晃的动画，独占式地控制旋转。
     */
    public class Swing_Exclusive : MonoBehaviour
    {
        public bool start_on_load = true;
        public float angle = 30;
        public float duration = 1;
        public Tween.EaseType ease_type = Tween.EaseType.easeInOutSine;
        public Vector3 swing_axis = Vector3.forward;
        public Tween.TweenBase tween;

        Quaternion origin_rot;
        bool started = false;
        void Start()
        {
            origin_rot = transform.rotation;
            if (start_on_load)
            {
                StartAnimation();
            }
        }

        public void StartAnimation()
        {
            float current_angle = transform.rotation.eulerAngles.MaxElement();
            if (started) return;
            started = true;
            tween = new Tween.Float().From(current_angle).To(current_angle + angle).Time(duration).Ease(ease_type)
                .OnUpdate(SwingCallback).Loop();
        }
        void Update()
        {
            if (tween != null)
            {
                tween.OnUpdate();
            }
        }
        public void SwingCallback(float value)
        {
            transform.rotation = Quaternion.AngleAxis(value, swing_axis) * origin_rot;
        }
    }
}