using UnityEngine;
using System.Collections;
using LyLib;

public class TriggerCallback : MonoBehaviour 
{
    public Callback<Collider> on_trigger_stay, on_trigger_exit, on_trigger_enter;
    void OnTriggerStay(Collider co)
    {
        if (on_trigger_stay != null)
        {
            on_trigger_stay(co);
        }
    }
    void OnTriggerExit(Collider co)
    {
        if (on_trigger_exit != null)
        {
            on_trigger_exit(co);
        }
    }
    void OnTriggerEnter(Collider co)
    {
        if (on_trigger_enter != null)
        {
            on_trigger_enter(co);
        }
    }
}
