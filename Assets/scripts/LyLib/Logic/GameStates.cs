using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/*
 * 控制游戏状态
 * 1、继承GameState类，这个类应该命名为 StateXXX。XXX作为State的名称。
 */

public interface IGameState
{
    void _StateEnter(string last_state);
    void _StateLeave(string next);
    void _StateUpdate();
    void _StateFixedUpdate();
    void _StateOnDestroy();
    void _StateLateUpdate();
}
public class EmptyGameState : IGameState
{
    public void _StateEnter(string last_state) { }
    public void _StateLeave(string next){}
    public void _StateUpdate(){}
    public void _StateFixedUpdate(){}
    public void _StateOnDestroy(){}
    public void _StateLateUpdate(){}
}
// 基类，用来被继承并实现需要的回调函数。
public class GameState : MonoBehaviour, IGameState
{
    public event CallbackString event_state_enter;
    public event CallbackString event_state_leave;
    public event Callback event_state_update;
    public event Callback event_state_fixedupdate;
    public event Callback event_state_destroy;
    public event Callback event_state_lateupdate;
    public void _StateEnter(string last_state) {
        if (event_state_enter != null)
        {
            event_state_enter(last_state);
        }
        StateEnter(last_state);
    }
    public void _StateLeave(string next)
    {
        if (event_state_leave != null)
        {
            event_state_leave(next);
        }
        StateLeave(next);
    }
    public void _StateUpdate()
    {
        if (event_state_update != null)
        {
            event_state_update();
        }
        StateUpdate();
    }
    public void _StateFixedUpdate()
    {
        if (event_state_fixedupdate != null)
        {
            event_state_fixedupdate();
        }
        StateFixedUpdate();
    }
    public void _StateOnDestroy()
    {
        if (event_state_destroy != null)
        {
            event_state_destroy();
        }
        StateOnDestroy();
    }
    public void _StateLateUpdate()
    {
        if (event_state_lateupdate != null)
        {
            event_state_lateupdate();
        }
        StateLateUpdate();
    }
    public virtual void StateEnter(string last_state) { }
    public virtual void StateLeave(string next_state) { }
    public virtual void StateUpdate() { }
    public virtual void StateFixedUpdate() { }
    public virtual void StateOnDestroy() { }
    public virtual void StateLateUpdate() { }
}

public class GameStates : MonoBehaviour
{
    public const string DEFAULT_STATE = "Default";
    public string first_state = "Default";
    private Dictionary<string, IGameState> states;
    private string current_state = DEFAULT_STATE;
    private string last_state = DEFAULT_STATE;
    public static GameStates instance;

    static Stack<string> state_stack = new Stack<string>();

    public GameStates()
	{
        instance = this;
        states = new Dictionary<string, IGameState>();
        states[DEFAULT_STATE] = new EmptyGameState();
	}
	public void AddState(string name, GameState state)
    {
        if (states.ContainsKey(name))
        {
            Debug.LogError("duplicated state name: " + name);
            return;
        }
        states[name] = state;
	}
    public void Push(string name)
    {
        state_stack.Push(current_state);
        CurrentState = name;
    }
    public bool Pop()
    {
        if (state_stack.Count == 0)
        {
            Debug.LogError("no state to pop!");
            return false;
        }
        string state_name = state_stack.Pop();
        CurrentState = state_name;
        return true;
    }
    public IGameState GetState(string name)
    {
        return states[name];
    }
    void Start()
    {
        if (first_state == "")
        {
            Debug.LogError("you forgot to set first state name");
            return;
        }
        StartCoroutine(OnStart());
    }
    IEnumerator OnStart() 
    {
        yield return new WaitForEndOfFrame();
        // 找到所有State
        var found_states = GetComponents<GameState>();
        foreach (var state in found_states)
        {
            var state_name = state.GetType().Name.Substring("State".Length);
            AddState(state_name, state);
            //Debug.Log("add state: " + state_name);
        }
        CurrentState = first_state;
    }
    // 例如Update的时候调用此函数，确定当前状态是否可以执行Update。
    public bool CheckState(List<string> states)
    {
        return states.Contains(current_state);
    }
	public string LastState {
		get{ return last_state;}
	}
	public string CurrentState {
		get{ return current_state;}
		set {
			if (value == current_state){
				return;
			}
            if (!states.ContainsKey(value))
            {
                Debug.LogError("state not found: " + value.ToString());
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
                return;
            }
            //Debug.Log("entering " + value + " from " + current_state);

            // on leave current state
            states[current_state]._StateLeave(value);
			// enter new state
			last_state = current_state;
			current_state = value;
			// on enter new state
            states[current_state]._StateEnter(last_state);
		}
	}


	public void Update()
    {
        states[current_state]._StateUpdate();
	}
    void FixedUpdate()
    {
        states[current_state]._StateFixedUpdate();
	}
    void LateUpdate()
    {
        states[current_state]._StateLateUpdate();
    }
    void OnDisable()
    {
        states[current_state]._StateLeave(DEFAULT_STATE);
        states[current_state]._StateOnDestroy();
    }
}
