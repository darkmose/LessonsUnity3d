using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FSM
{
    public enum States 
    { 
        EntryPoint, 
        HealthPointsSearch, 
        EnemiesSearch, 
        EnemiesAttack, 
        WeaponSearch, 
        AmmunitionSearch 
    }


    public interface IState
    {
        States State { get; }

        void OnEnterState();

        void OnExitState();

        void OnStateStay();
    }

    public interface IStateSwitcher 
    {
        void SwitchStateTo(States state);
    }

    public class FinitStateMachineSwitcher : IStateSwitcher
    {
        private Dictionary<States, IState> _states;
        private IState _currentState;

        public void SwitchStateTo(States state)
        {
            if (_states.ContainsKey(state))
            {
                _currentState?.OnExitState();
                _currentState = _states[state];
                _currentState?.OnEnterState();
            }
            else
            {
                Debug.Log($"State {state}, doesn't exist!");
            }
        }

        public void InitializeDictionary() 
        {
            _states = new Dictionary<States, IState>();
        }

        public void RegisterState(IState state) 
        {
            if (_states.ContainsKey(state.State))
            {
                Debug.Log($"State {state.State} already exist!");
            }
            else
            {
                _states.Add(state.State, state);
            }
        } 
        public void StateUpdate() 
        {
            _currentState?.OnStateStay();
            Debug.Log($"State Update: State {_currentState.State}");
        }
    }
}