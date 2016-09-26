using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;

namespace LyLib
{
    public abstract class State
    {
        public string name = "default";
        public FSM fsm;// a reference to its parent FSM
        public virtual void OnUpdate() { }
        public virtual void OnEnter(string last_state) { }
        public virtual void OnExit(string next_state) { }
    }

    public class StateBT : State
    {
        BT bt;
        public StateBT(BT bt)
        {
            this.bt = bt;
        }
        public override void OnUpdate()
        {
            bt.Run();
        }
        public override void OnExit(string last_state)
        {
            bt.OnExit();
        }
    }
    public class StateEmpty : State
    {
    }

    public class FSM
    {
        Dictionary<string, State> states = new Dictionary<string, State>();
        string last_state = "default";
        string current_state = "default";

        public State GetState(string state_name)
        {
            State state = null;
            states.TryGetValue(state_name, out state);
            return state;
        }
        public FSM()
        {
            states["default"] = new StateEmpty();
        }
        public void AddState(string name, State state)
        {
            if (states.ContainsKey(name))
            {
                Debug.LogError("duplicated state: " + name);
                return;
            }
            if (current_state == "default")
            {
                current_state = name;
            }
            states[name] = state;
            state.fsm = this;
            state.name = name;
        }
        public void ChangeState(string target)
        {
            if (states.ContainsKey(target) == false)
            {
                Debug.LogError("try to change to unknown state: " + target);
                return;
            }
            if (current_state == target)
            {
                return;
            }

            states[current_state].OnExit(target);

            last_state = current_state;
            current_state = target;

            states[current_state].OnEnter(last_state);

            // rdDebug.Log("state: " + last_state + " => " + current_state);
        }
        public void Update()
        {
            if (states.ContainsKey(current_state))
            {
                states[current_state].OnUpdate();
            }
            else
            {
                Debug.LogError("unknown state name: " + current_state);
            }
        }
    }
}