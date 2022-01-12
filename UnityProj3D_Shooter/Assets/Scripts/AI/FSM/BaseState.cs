using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
    public abstract class BaseState : IState
    {
        protected IStateSwitcher _stateSwitcher;
        protected AIShared _aIShared;
        
        public BaseState(IStateSwitcher switcher, AIShared aIShared) 
        {
            _stateSwitcher = switcher;
            _aIShared = aIShared;
        }

        public abstract States State { get; }

        public virtual void OnEnterState()
        {
            Debug.Log($"[{this}] On Enter State");
        }

        public virtual void OnExitState()
        {
            Debug.Log($"[{this}] On Exit State");
        }

        public abstract void OnStateStay();
    }
}