using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// TODO: 实现INode的OnEnter和OnLeave回调。
namespace LyLib {

	public enum STATUS {
		SUCCESS, FAIL, RUNNING, ERROR
	}
	public delegate STATUS BTCallback();
	public abstract class INode {
        public virtual void OnReset() { }
        public abstract STATUS Run();
        public virtual void Init(FSMCom fsm, BT bt) { }
	}
	#region COMPOSITE NODE
	public abstract class BTComposite : INode {
		public INode[] children;
		public int index_to_run;// index of the last running-node
		public BTComposite(params INode[] node) {
			index_to_run = 0;
			children = new INode[node.Length];
			node.CopyTo (children, 0);
		}
        protected STATUS HandleResult(STATUS child_status, STATUS break_status)
        {
            if (child_status == STATUS.RUNNING)
            {
                return STATUS.RUNNING;
            }
            else if (child_status == STATUS.ERROR)
            {
                index_to_run = 0;
                return STATUS.ERROR;
            }
            else if (child_status == break_status)
            {
                index_to_run = 0;
                return STATUS.SUCCESS;
            }
            else
            {
                if (index_to_run < children.Length - 1)
                {
                    index_to_run++;
                    return STATUS.RUNNING;
                }
                else
                {
                    index_to_run = 0;
                    return STATUS.SUCCESS;
                }
            }
        }
        public override void OnReset()
        {
            foreach (var child in children)
            {
                child.OnReset();
            }
        }
        public override void Init(FSMCom fsm, BT bt)
        {
            base.Init(fsm, bt);
            foreach (var child in children)
            {
                child.Init(fsm, bt);
            }
        }
	}
	public class BTSelector : BTComposite { // run until one success
		public BTSelector(params INode[] node) : base(node) {
		}
		public override STATUS Run(){
            STATUS return_status = children[index_to_run].Run();
            return HandleResult(return_status, STATUS.SUCCESS);
		}

    }
	public class BTSequence : BTComposite {
		public BTSequence(params INode[] node) : base(node) {
		}
		public override STATUS Run(){
            STATUS return_status = children[index_to_run].Run();
            return HandleResult(return_status, STATUS.FAIL);
		}
	}
    public class BTRandom : BTComposite
    {
        public BTRandom(params INode[] nodes)
            : base(nodes)
        {
            index_to_run = LM.RandomIndex(children.Length);
        }
        public override STATUS Run()
        {
            int child_index = index_to_run;
            STATUS return_status = children[child_index].Run();
            switch (return_status)
            {
                case STATUS.RUNNING:
                    index_to_run = child_index;
                    return STATUS.RUNNING;
                case STATUS.SUCCESS:
                    index_to_run = LM.RandomIndex(children.Length);
                    return STATUS.SUCCESS;
            }
            return return_status;
        }
    }
    // return RUNNING when everything ok
    // return ERROR when something wrong
	public class BTParallel : BTComposite {
		public BTParallel(params INode[] node) : base(node) {
		}
		public override STATUS Run(){
			for (int i = 0; i < children.Length; ++i) {
				if (children [i].Run () == STATUS.ERROR)
                {
					return STATUS.ERROR;
				}
			}
			// all success
            return STATUS.RUNNING ;
		}
	}
	#endregion
	#region BEHAVIOUR NODE
    // base class for all custome node
    public abstract class BTAction : INode
    {
        // 子类可以使用的变量
        protected FSMCom fsm;
        protected BT bt;
        protected Animation anim;

        // 子类重载这些函数来实现功能
        protected virtual STATUS OnAction() {
            return STATUS.RUNNING;
        }
        protected virtual void OnEnter() { }
        protected virtual void OnExit() { }
        protected virtual void OnInit() { }

        // --
        public void _OnEnter() {
            OnEnter();
        }
        public void _OnExit() {
            running = false;
            OnExit();
        }
        bool running = false;
		public override STATUS Run(){
            if (!running)
            {
                running = true;
                OnEnter();
            }
            STATUS return_status = OnAction();
            if (return_status == STATUS.RUNNING)
            {
                bt.last_running_action = this;
            }
            else
            {
                bt.last_running_action = null;
                running = false;
                OnExit();
            }
			return return_status;
		}
        public override void Init(FSMCom fsm, BT bt)
        {
            this.fsm = fsm;
            this.anim = fsm.transform.GetChildByName("View").GetComponent<Animation>();
            if (this.anim == null)
            {
                Debug.LogError("no Animation component found for " + fsm.name + ".View");
            }
            this.bt = bt;
            OnInit();
        }
	}
    public class BTFn : BTAction
    {
        BTCallback fn;
        public BTFn(BTCallback fn)
        {
            this.fn = fn;
        }
        protected override STATUS OnAction()
        {
            return fn();
        }
    }
	#endregion
	#region DECORATOR NODE
	public abstract class BTDecorator : INode {
		public INode child;
		public BTDecorator(INode node){
			child = node;// shadow copy
		}
        public override void OnReset()
        {
            child.OnReset();
        }
        public override void Init(FSMCom fsm, BT bt)
        {
            base.Init(fsm, bt);
            child.Init(fsm, bt);
        }
	}
	public class BTFailure : BTDecorator {
		public BTFailure(INode children) : base(children) {
			// do nothing
		}
		public override STATUS Run(){
			STATUS run_status = child.Run ();
			if (run_status == STATUS.SUCCESS || run_status == STATUS.FAIL) {
				return STATUS.FAIL;
			}
			return run_status;
		}
	}
	public class BTNegate : BTDecorator {
		public BTNegate(INode children) : base(children) {
			// do nothing
		}
		public override STATUS Run(){
			STATUS return_status = child.Run ();
			if (return_status == STATUS.SUCCESS) {
				return STATUS.FAIL;
			} else if(return_status == STATUS.FAIL){
				return STATUS.SUCCESS;
			}else{
				return return_status;
			}
		}
	}
	#endregion
	#region BEHAVIOUR TREE
	public class BT {
        public FSMCom fsm;
        public BTAction last_running_action;
        public delegate void OnError();
        public void OnExit()
        {
            if (last_running_action != null)
                last_running_action._OnExit();
        }
		public OnError on_error;
		private bool paused = false;
		private STATUS last_status = STATUS.SUCCESS;
		private INode root;
		public BT(FSMCom fsm, INode root_node){
			root = root_node;
            this.fsm = fsm;
            root_node.Init(fsm, this);
		}
		public void Run(){
			if (paused)
				return;
			last_status = root.Run ();
			switch (last_status) {
			case STATUS.ERROR:
				Pause ();
				on_error();
				break;
			}
		}
		public void Reset(){
			last_status = STATUS.SUCCESS;
			paused = false;
            root.OnReset();
		}
		public void Pause(){
			paused = true;
		}
	}
	#endregion
}

