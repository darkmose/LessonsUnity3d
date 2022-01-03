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
        EnemiesFire, 
        WeaponSearch, 
        AmmunitionSearch 
    }


    public interface IState
    {
        public void OnEnterState();

        public void OnExitState();

        public void OnStateStay();
    }

    public interface IStateSwitcher 
    {
        public States state;
    
    }

    public class FinitStateMachine
    {
        




    }



}