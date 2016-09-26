using UnityEngine;
using System.Collections;

/*
 * 当触发时调用某个函数
 */
public class FunctionTrigger : MonoBehaviour {
    public bool call_self = true;
    public string function_trigger_enter = "onenter";
    public string function_trigger_leave = "onleave";
	
	private void OnTriggerEnter(Collider other)
    {
        CallFunction(function_trigger_enter, other);
	}
    private void OnTriggerExit(Collider other)
    {
        CallFunction(function_trigger_leave, other);
    }
    private void CallFunction(string function_name, Collider other)
    {
        if (function_name != null && function_name != "")
        {
            if (call_self)
            {
                gameObject.SendMessage(function_name, other, SendMessageOptions.DontRequireReceiver);
            }
            else
            {
                other.gameObject.SendMessage(function_name, gameObject, SendMessageOptions.DontRequireReceiver);
            }
        }
    }
    public void onenter()
    {
        Debug.Log("entering");
    }
    public void onleave()
    {
        Debug.Log("leaving");
    }
}
