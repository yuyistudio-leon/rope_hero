using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tween : MonoBehaviour
{
    static bool in_update_loop = false;
    static List<TweenBase> tweens = new List<TweenBase>();
    static List<TweenBase> remove_list = new List<TweenBase>();
    static List<TweenBase> add_list = new List<TweenBase>();
    private delegate float EasingFunction(float start, float end, float Value);

    public void OnUpdate(bool physics)
    {
        Update(physics);
    }
    void FixedUpdate()
    {
        Update(true);
    }
    void Update()
    {
        Update(false);
    }
    void Update(bool physics)
    {
        in_update_loop = true;
        foreach (TweenBase tween in tweens)
        {
            if (tween.physics == physics)
            {
                tween.OnUpdate();
            }
        }
        in_update_loop = false;
        foreach (TweenBase tween in remove_list)
        {
            tweens.Remove(tween);
        }
        remove_list.Clear();
        foreach (TweenBase tween in add_list)
        {
            tween.Start();
        }
        add_list.Clear();
    }

    public abstract class TweenBase {
        internal bool physics = false;
        private EasingFunction ease_fn;
        protected float duration;
        protected LoopType loop_type;
        protected event CallbackFloat on_update;
        protected event Callback on_complete;
        protected event Callback on_start;
        private float from, to;
        
        private Callback update_fn;
        private bool complete = false;
        protected float timer = 0;

        // sequence support
        protected TweenBase tween_after;
        protected float tween_after_time_gap;

        public TweenBase()
        {
            ease_fn = linear;
            update_fn = UpdatePercentage;
            duration = 1;
            loop_type = LoopType.none;
            from = 0;
            to = 1;
        }
        public void Cancel()
        {
            remove_list.Add(this);
        }
        public TweenBase Start()
        {
            if (in_update_loop)
            {
                add_list.Add(this);
                return this;
            }
            if (tween_after != null && loop_type != LoopType.none)
            {
                Debug.LogError("tween-after won't take effect when loop type is not none");
            }
            tweens.Add(this);
            if (on_start != null)
            {
                on_start();
            }
            return this;
        }


        protected void Physical()
        {
            physics = true;
        }
        protected void After(TweenBase after)
        {
            tween_after = after;
        }
        protected void Time(float v)
        {
            duration = v;
        }
        protected void Ease(EaseType ease_type)
        {
            ease_fn = GetEasingFunction(ease_type);
        }
        protected void OnComplete(Callback fn)
        {
            on_complete += fn;
        }
        protected void Loop(LoopType param_loop_type)
        {
            loop_type = param_loop_type;
        }
        protected void Loop()
        {
            loop_type = LoopType.loop;
        }
        public void PingPong()
        {
            loop_type = LoopType.pingPong;
        }

        protected void SetUpdateFn(CallbackFloat fn)
        {
            if (fn != null)
            {
                on_update = fn;
                update_fn = UpdatePercentageWithCallback;
            }
        }
        protected void UpdatePercentage()
        {
            timer += UnityEngine.Time.deltaTime;
            if (timer > duration)
            {
                complete = true; ;
                OnComplete();
                remove_list.Add(this);
            }
        }
        protected void UpdatePercentageWithCallback() 
        {
            timer += UnityEngine.Time.deltaTime;
            if (timer > duration)
            {
                timer = duration;
                if (!complete)
                {
                    complete = true;
                    float ease_value = ease_fn(from, to, timer / duration);
                    on_update(ease_value);
                }
                else
                {
                    OnComplete();
                }
            }
            else
            {
                float ease_value = ease_fn(from, to, timer / duration);
                on_update(ease_value);
            }
        }
        internal void OnUpdate()
        {
            update_fn();
        }
        private void OnComplete()
        {
            if (on_complete != null)
            {
                on_complete();
            }
            remove_list.Add(this);
            switch (loop_type)
            {
                case LoopType.pingPong:
                    float tmp = to;
                    to = from;
                    from = tmp;
                    break;
                case LoopType.none:
                    if (tween_after != null)
                    {
                        add_list.Add(tween_after);
                    }
                    break;
            }
            if (loop_type != LoopType.none)
            {
                complete = false;
                timer = 0;
            }
        }
    }
    public class Float : TweenBase
    {
        float from_value = 0, to_value = 1;
        public Float From(float f)
        {
            from_value = f;
            return this;
        }
        public Float To(float v)
        {
            to_value = v;
            return this;
        }
        public Float OnUpdate(CallbackFloat fn)
        {
            SetUpdateFn((float percentage)=> fn(clerp(from_value, to_value, percentage)));
            return this;
        }

        public new Float Physical()
        {
            base.Physical();
            return this;
        }
        public new Float After(TweenBase after)
        {
            base.After(after);
            return this;
        }
        public new Float Time(float v)
        {
            base.Time(v);
            return this;
        }
        public new Float Ease(EaseType ease_type)
        {
            base.Ease(ease_type);
            return this;
        }
        public new Float OnComplete(Callback fn)
        {
            base.OnComplete(fn);
            return this;
        }
        public new Float Loop()
        {
            base.Loop();
            return this;
        }
        public new Float Loop(LoopType param_loop_type)
        {
            loop_type = param_loop_type;
            return this;
        }
        public new Float PingPong()
        {
            base.PingPong();
            return this;
        }
    }
    public class Timer : Float
    {
        public Float DoAfter(float time, Callback on_complete)
        {
            return Time(time).OnComplete(on_complete);
        }
        public Float Schedule(float duration, Callback on_loop)
        {
            return Time(duration).OnComplete(on_loop).Loop();
        }
    }
    // a demo demonstrating the way to implement a tween-class for the type you want
    public class V3 : TweenBase
    {
        // type-specifined vars
        Vector3 from_v3, to_v3;

        // take control of the OnUpdate callback to lerp your type-specified value with user-provided callback function
        public V3 OnUpdate(CallbackVector3 fn)
        {
            SetUpdateFn((float percentage) =>
            {
                fn(new Vector3(
                    clerp(from_v3.x, to_v3.x, percentage),
                    clerp(from_v3.y, to_v3.y, percentage),
                    clerp(from_v3.z, to_v3.z, percentage)
                    ));
            });
            return this;
        }
        // fns to provide some convinience
        public V3 From(CallbackGetVector3 fn)
        {
            on_start += () => from_v3 = fn();
            return this;
        }
        public V3 From(Vector3 v)
        {
            from_v3 = v;
            return this;
        }
        public V3 To(CallbackGetVector3 fn)
        {

            on_start += () => to_v3 = fn();
            return this;
        }
        public V3 To(Vector3 v)
        {
            to_v3 = v;
            return this;
        }
        public V3 ToOffset(Vector3 v)
        {
            to_v3 = from_v3 + v;
            Debug.Log(to_v3 + "," + from_v3);
            return this;
        }

        public new V3 Physical()
        {
            base.Physical();
            return this;
        }
        public new V3 After(TweenBase after)
        {
            base.After(after);
            return this;
        }
        public new V3 Time(float v)
        {
            base.Time(v);
            return this;
        }
        public new V3 Ease(EaseType ease_type)
        {
            base.Ease(ease_type);
            return this;
        }
        public new V3 OnComplete(Callback fn)
        {
            base.OnComplete(fn);
            return this;
        }
        public new V3 Loop()
        {
            base.Loop();
            return this;
        }
        public new V3 PingPong()
        {
            base.PingPong();
            return this;
        }

    }
    public class Padding : TweenBase
    {
        public new Padding Time(float time)
        {
            base.Time(time);
            return this;
        }
        public new Padding OnComplete(Callback fn)
        {
            base.OnComplete(fn);
            return this;
        }
        public new Padding After(TweenBase next)
        {
            base.After(next);
            return this;
        }
    }
    // FROM iTween !!!
    public enum LoopType
    {
        /// <summary>
        /// Do not loop.
        /// </summary>
        none,
        /// <summary>
        /// Rewind and replay.
        /// </summary>
        loop,
        /// <summary>
        /// Ping pong the animation back and forth.
        /// </summary>
        pingPong
    }
    public enum EaseType
    {
        easeInQuad,
        easeOutQuad,
        easeInOutQuad,
        easeInCubic,
        easeOutCubic,
        easeInOutCubic,
        easeInQuart,
        easeOutQuart,
        easeInOutQuart,
        easeInQuint,
        easeOutQuint,
        easeInOutQuint,
        easeInSine,
        easeOutSine,
        easeInOutSine,
        easeInExpo,
        easeOutExpo,
        easeInOutExpo,
        easeInCirc,
        easeOutCirc,
        easeInOutCirc,
        linear,
        spring,
        /* GFX47 MOD START */
        //bounce,
        easeInBounce,
        easeOutBounce,
        easeInOutBounce,
        /* GFX47 MOD END */
        easeInBack,
        easeOutBack,
        easeInOutBack,
        /* GFX47 MOD START */
        //elastic,
        easeInElastic,
        easeOutElastic,
        easeInOutElastic,
        /* GFX47 MOD END */
        punch
    }
    //instantiates a cached ease equation refrence:
    static EasingFunction GetEasingFunction(EaseType easeType)
    {
        EasingFunction ease = new EasingFunction(linear);
        switch (easeType)
        {
            case EaseType.easeInQuad:
                ease = new EasingFunction(easeInQuad);
                break;
            case EaseType.easeOutQuad:
                ease = new EasingFunction(easeOutQuad);
                break;
            case EaseType.easeInOutQuad:
                ease = new EasingFunction(easeInOutQuad);
                break;
            case EaseType.easeInCubic:
                ease = new EasingFunction(easeInCubic);
                break;
            case EaseType.easeOutCubic:
                ease = new EasingFunction(easeOutCubic);
                break;
            case EaseType.easeInOutCubic:
                ease = new EasingFunction(easeInOutCubic);
                break;
            case EaseType.easeInQuart:
                ease = new EasingFunction(easeInQuart);
                break;
            case EaseType.easeOutQuart:
                ease = new EasingFunction(easeOutQuart);
                break;
            case EaseType.easeInOutQuart:
                ease = new EasingFunction(easeInOutQuart);
                break;
            case EaseType.easeInQuint:
                ease = new EasingFunction(easeInQuint);
                break;
            case EaseType.easeOutQuint:
                ease = new EasingFunction(easeOutQuint);
                break;
            case EaseType.easeInOutQuint:
                ease = new EasingFunction(easeInOutQuint);
                break;
            case EaseType.easeInSine:
                ease = new EasingFunction(easeInSine);
                break;
            case EaseType.easeOutSine:
                ease = new EasingFunction(easeOutSine);
                break;
            case EaseType.easeInOutSine:
                ease = new EasingFunction(easeInOutSine);
                break;
            case EaseType.easeInExpo:
                ease = new EasingFunction(easeInExpo);
                break;
            case EaseType.easeOutExpo:
                ease = new EasingFunction(easeOutExpo);
                break;
            case EaseType.easeInOutExpo:
                ease = new EasingFunction(easeInOutExpo);
                break;
            case EaseType.easeInCirc:
                ease = new EasingFunction(easeInCirc);
                break;
            case EaseType.easeOutCirc:
                ease = new EasingFunction(easeOutCirc);
                break;
            case EaseType.easeInOutCirc:
                ease = new EasingFunction(easeInOutCirc);
                break;
            case EaseType.linear:
                ease = new EasingFunction(linear);
                break;
            case EaseType.spring:
                ease = new EasingFunction(spring);
                break;
            /* GFX47 MOD START */
            /*case EaseType.bounce:
                ease = new EasingFunction(bounce);
                break;*/
            case EaseType.easeInBounce:
                ease = new EasingFunction(easeInBounce);
                break;
            case EaseType.easeOutBounce:
                ease = new EasingFunction(easeOutBounce);
                break;
            case EaseType.easeInOutBounce:
                ease = new EasingFunction(easeInOutBounce);
                break;
            /* GFX47 MOD END */
            case EaseType.easeInBack:
                ease = new EasingFunction(easeInBack);
                break;
            case EaseType.easeOutBack:
                ease = new EasingFunction(easeOutBack);
                break;
            case EaseType.easeInOutBack:
                ease = new EasingFunction(easeInOutBack);
                break;
            /* GFX47 MOD START */
            /*case EaseType.elastic:
                ease = new EasingFunction(elastic);
                break;*/
            case EaseType.easeInElastic:
                ease = new EasingFunction(easeInElastic);
                break;
            case EaseType.easeOutElastic:
                ease = new EasingFunction(easeOutElastic);
                break;
            case EaseType.easeInOutElastic:
                ease = new EasingFunction(easeInOutElastic);
                break;
            /* GFX47 MOD END */
        }
        return ease;
    }
    #region Easing Curves

    private static float linear(float start, float end, float value)
    {
        return Mathf.Lerp(start, end, value);
    }

    private static float clerp(float start, float end, float value)
    {
        float min = 0.0f;
        float max = 360.0f;
        float half = Mathf.Abs((max - min) * 0.5f);
        float retval = 0.0f;
        float diff = 0.0f;
        if ((end - start) < -half)
        {
            diff = ((max - start) + end) * value;
            retval = start + diff;
        }
        else if ((end - start) > half)
        {
            diff = -((max - end) + start) * value;
            retval = start + diff;
        }
        else retval = start + (end - start) * value;
        return retval;
    }

    private static float spring(float start, float end, float value)
    {
        value = Mathf.Clamp01(value);
        value = (Mathf.Sin(value * Mathf.PI * (0.2f + 2.5f * value * value * value)) * Mathf.Pow(1f - value, 2.2f) + value) * (1f + (1.2f * (1f - value)));
        return start + (end - start) * value;
    }

    private static float easeInQuad(float start, float end, float value)
    {
        end -= start;
        return end * value * value + start;
    }

    private static float easeOutQuad(float start, float end, float value)
    {
        end -= start;
        return -end * value * (value - 2) + start;
    }

    private static float easeInOutQuad(float start, float end, float value)
    {
        value /= .5f;
        end -= start;
        if (value < 1) return end * 0.5f * value * value + start;
        value--;
        return -end * 0.5f * (value * (value - 2) - 1) + start;
    }

    private static float easeInCubic(float start, float end, float value)
    {
        end -= start;
        return end * value * value * value + start;
    }

    private static float easeOutCubic(float start, float end, float value)
    {
        value--;
        end -= start;
        return end * (value * value * value + 1) + start;
    }

    private static float easeInOutCubic(float start, float end, float value)
    {
        value /= .5f;
        end -= start;
        if (value < 1) return end * 0.5f * value * value * value + start;
        value -= 2;
        return end * 0.5f * (value * value * value + 2) + start;
    }

    private static float easeInQuart(float start, float end, float value)
    {
        end -= start;
        return end * value * value * value * value + start;
    }

    private static float easeOutQuart(float start, float end, float value)
    {
        value--;
        end -= start;
        return -end * (value * value * value * value - 1) + start;
    }

    private static float easeInOutQuart(float start, float end, float value)
    {
        value /= .5f;
        end -= start;
        if (value < 1) return end * 0.5f * value * value * value * value + start;
        value -= 2;
        return -end * 0.5f * (value * value * value * value - 2) + start;
    }

    private static float easeInQuint(float start, float end, float value)
    {
        end -= start;
        return end * value * value * value * value * value + start;
    }

    private static float easeOutQuint(float start, float end, float value)
    {
        value--;
        end -= start;
        return end * (value * value * value * value * value + 1) + start;
    }

    private static float easeInOutQuint(float start, float end, float value)
    {
        value /= .5f;
        end -= start;
        if (value < 1) return end * 0.5f * value * value * value * value * value + start;
        value -= 2;
        return end * 0.5f * (value * value * value * value * value + 2) + start;
    }

    private static float easeInSine(float start, float end, float value)
    {
        end -= start;
        return -end * Mathf.Cos(value * (Mathf.PI * 0.5f)) + end + start;
    }

    private static float easeOutSine(float start, float end, float value)
    {
        end -= start;
        return end * Mathf.Sin(value * (Mathf.PI * 0.5f)) + start;
    }

    private static float easeInOutSine(float start, float end, float value)
    {
        end -= start;
        return -end * 0.5f * (Mathf.Cos(Mathf.PI * value) - 1) + start;
    }

    private static float easeInExpo(float start, float end, float value)
    {
        end -= start;
        return end * Mathf.Pow(2, 10 * (value - 1)) + start;
    }

    private static float easeOutExpo(float start, float end, float value)
    {
        end -= start;
        return end * (-Mathf.Pow(2, -10 * value) + 1) + start;
    }

    private static float easeInOutExpo(float start, float end, float value)
    {
        value /= .5f;
        end -= start;
        if (value < 1) return end * 0.5f * Mathf.Pow(2, 10 * (value - 1)) + start;
        value--;
        return end * 0.5f * (-Mathf.Pow(2, -10 * value) + 2) + start;
    }

    private static float easeInCirc(float start, float end, float value)
    {
        end -= start;
        return -end * (Mathf.Sqrt(1 - value * value) - 1) + start;
    }

    private static float easeOutCirc(float start, float end, float value)
    {
        value--;
        end -= start;
        return end * Mathf.Sqrt(1 - value * value) + start;
    }

    private static float easeInOutCirc(float start, float end, float value)
    {
        value /= .5f;
        end -= start;
        if (value < 1) return -end * 0.5f * (Mathf.Sqrt(1 - value * value) - 1) + start;
        value -= 2;
        return end * 0.5f * (Mathf.Sqrt(1 - value * value) + 1) + start;
    }

    /* GFX47 MOD START */
    private static float easeInBounce(float start, float end, float value)
    {
        end -= start;
        float d = 1f;
        return end - easeOutBounce(0, end, d - value) + start;
    }
    /* GFX47 MOD END */

    /* GFX47 MOD START */
    //private static float bounce(float start, float end, float value){
    private static float easeOutBounce(float start, float end, float value)
    {
        value /= 1f;
        end -= start;
        if (value < (1 / 2.75f))
        {
            return end * (7.5625f * value * value) + start;
        }
        else if (value < (2 / 2.75f))
        {
            value -= (1.5f / 2.75f);
            return end * (7.5625f * (value) * value + .75f) + start;
        }
        else if (value < (2.5 / 2.75))
        {
            value -= (2.25f / 2.75f);
            return end * (7.5625f * (value) * value + .9375f) + start;
        }
        else
        {
            value -= (2.625f / 2.75f);
            return end * (7.5625f * (value) * value + .984375f) + start;
        }
    }
    /* GFX47 MOD END */

    /* GFX47 MOD START */
    private static float easeInOutBounce(float start, float end, float value)
    {
        end -= start;
        float d = 1f;
        if (value < d * 0.5f) return easeInBounce(0, end, value * 2) * 0.5f + start;
        else return easeOutBounce(0, end, value * 2 - d) * 0.5f + end * 0.5f + start;
    }
    /* GFX47 MOD END */

    private static float easeInBack(float start, float end, float value)
    {
        end -= start;
        value /= 1;
        float s = 1.70158f;
        return end * (value) * value * ((s + 1) * value - s) + start;
    }

    private static float easeOutBack(float start, float end, float value)
    {
        float s = 1.70158f;
        end -= start;
        value = (value) - 1;
        return end * ((value) * value * ((s + 1) * value + s) + 1) + start;
    }

    private static float easeInOutBack(float start, float end, float value)
    {
        float s = 1.70158f;
        end -= start;
        value /= .5f;
        if ((value) < 1)
        {
            s *= (1.525f);
            return end * 0.5f * (value * value * (((s) + 1) * value - s)) + start;
        }
        value -= 2;
        s *= (1.525f);
        return end * 0.5f * ((value) * value * (((s) + 1) * value + s) + 2) + start;
    }

    private static float punch(float amplitude, float value)
    {
        float s = 9;
        if (value == 0)
        {
            return 0;
        }
        else if (value == 1)
        {
            return 0;
        }
        float period = 1 * 0.3f;
        s = period / (2 * Mathf.PI) * Mathf.Asin(0);
        return (amplitude * Mathf.Pow(2, -10 * value) * Mathf.Sin((value * 1 - s) * (2 * Mathf.PI) / period));
    }

    /* GFX47 MOD START */
    private static float easeInElastic(float start, float end, float value)
    {
        end -= start;

        float d = 1f;
        float p = d * .3f;
        float s = 0;
        float a = 0;

        if (value == 0) return start;

        if ((value /= d) == 1) return start + end;

        if (a == 0f || a < Mathf.Abs(end))
        {
            a = end;
            s = p / 4;
        }
        else
        {
            s = p / (2 * Mathf.PI) * Mathf.Asin(end / a);
        }

        return -(a * Mathf.Pow(2, 10 * (value -= 1)) * Mathf.Sin((value * d - s) * (2 * Mathf.PI) / p)) + start;
    }
    /* GFX47 MOD END */

    /* GFX47 MOD START */
    //private static float elastic(float start, float end, float value){
    private static float easeOutElastic(float start, float end, float value)
    {
        /* GFX47 MOD END */
        //Thank you to rafael.marteleto for fixing this as a port over from Pedro's UnityTween
        end -= start;

        float d = 1f;
        float p = d * .3f;
        float s = 0;
        float a = 0;

        if (value == 0) return start;

        if ((value /= d) == 1) return start + end;

        if (a == 0f || a < Mathf.Abs(end))
        {
            a = end;
            s = p * 0.25f;
        }
        else
        {
            s = p / (2 * Mathf.PI) * Mathf.Asin(end / a);
        }

        return (a * Mathf.Pow(2, -10 * value) * Mathf.Sin((value * d - s) * (2 * Mathf.PI) / p) + end + start);
    }

    /* GFX47 MOD START */
    private static float easeInOutElastic(float start, float end, float value)
    {
        end -= start;

        float d = 1f;
        float p = d * .3f;
        float s = 0;
        float a = 0;

        if (value == 0) return start;

        if ((value /= d * 0.5f) == 2) return start + end;

        if (a == 0f || a < Mathf.Abs(end))
        {
            a = end;
            s = p / 4;
        }
        else
        {
            s = p / (2 * Mathf.PI) * Mathf.Asin(end / a);
        }

        if (value < 1) return -0.5f * (a * Mathf.Pow(2, 10 * (value -= 1)) * Mathf.Sin((value * d - s) * (2 * Mathf.PI) / p)) + start;
        return a * Mathf.Pow(2, -10 * (value -= 1)) * Mathf.Sin((value * d - s) * (2 * Mathf.PI) / p) * 0.5f + end + start;
    }
    /* GFX47 MOD END */

    #endregion	
	
}
