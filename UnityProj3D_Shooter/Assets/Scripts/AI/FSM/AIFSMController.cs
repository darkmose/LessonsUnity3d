using GameEvents;
using System;
using UnityEngine;

namespace FSM
{
    public class AIFSMController : MonoBehaviour
    {
        [SerializeField] private NavigationSystem _navigationSystem;  
        [SerializeField] private VitalitySystem _vitalitySystem;  
        [SerializeField] private NPCWeaponSystem _weaponSystem;
        [SerializeField] private States _currentState;
        [SerializeField] private bool _isMovedToPoint;
        private FinitStateMachineSwitcher _stateMachineSwitcher;
        private AIShared _aISharedSystem;

        private void Awake()
        {
            PrepareAIShared();
            PrepareFSM();
            PrepareEntryPointState();
            PrepareWeaponSearchState();
            PrepareAmmoSearchState();
            PrepareEnemiesSearchState();
            PrepareEnemiesAttackState();
            PrepareHPSearchState();
            _stateMachineSwitcher.SwitchStateTo(States.EntryPoint);
        }

        public void PrepareFSM() 
        {
            _stateMachineSwitcher = new FinitStateMachineSwitcher();
            _stateMachineSwitcher.InitializeDictionary();
        }

        public void PrepareAIShared() 
        {
            _aISharedSystem = new AIShared();
            _aISharedSystem.Navigation = _navigationSystem;
            _aISharedSystem.Vitality = _vitalitySystem;
            _aISharedSystem.Weapon = _weaponSystem;
        }

        public void PrepareEntryPointState() 
        {
            var entryPoint = new EntryPointState(_stateMachineSwitcher, _aISharedSystem);
            _stateMachineSwitcher.RegisterState(entryPoint);       
        }


        public void PrepareWeaponSearchState()
        {
            var weaponSearch = new WeaponSearchState(_stateMachineSwitcher, _aISharedSystem);
            _stateMachineSwitcher.RegisterState(weaponSearch);
        }

        public void PrepareAmmoSearchState()
        {
            var ammoSearch = new AmmoSearchState(_stateMachineSwitcher, _aISharedSystem);
            _stateMachineSwitcher.RegisterState(ammoSearch);
        }
        public void PrepareEnemiesSearchState()
        {
            var enemiesSearch = new EnemiesSearchState(_stateMachineSwitcher, _aISharedSystem);
            _stateMachineSwitcher.RegisterState(enemiesSearch);
        }

        public void PrepareEnemiesAttackState()
        {
            var enemiesAttack = new EnemiesAttackState(_stateMachineSwitcher, _aISharedSystem);
            _stateMachineSwitcher.RegisterState(enemiesAttack);
        }

        public void PrepareHPSearchState()
        {
            var healthSearch = new HPSearchState(_stateMachineSwitcher, _aISharedSystem);
            _stateMachineSwitcher.RegisterState(healthSearch);
        }

        private void FixedUpdate()
        {
            _currentState = _stateMachineSwitcher.CurrentState;
            _isMovedToPoint = _navigationSystem.IsMovedToPoint;
            _stateMachineSwitcher?.StateUpdate();
        }


    }


}