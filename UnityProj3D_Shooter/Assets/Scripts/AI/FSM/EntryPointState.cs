using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
    public class EntryPointState : BaseState
    {   
        public EntryPointState(IStateSwitcher switcher, AIShared aIShared) : base(switcher, aIShared)
        {
        }

        public override States State => States.EntryPoint;

        public override void OnStateStay()
        {
            if (_aIShared.Weapon.HasWeapon == false || _aIShared.Weapon.HasAmmo == false)
            {
                _stateSwitcher.SwitchStateTo(States.WeaponSearch);
            }
            else if (_aIShared.Navigation.IsEnemiesSpied == false)
            {
                _stateSwitcher.SwitchStateTo(States.EnemiesSearch);
            }
            else if (_aIShared.Navigation.IsEnemiesSpied)
            {
                _stateSwitcher.SwitchStateTo(States.EnemiesAttack);
            }
        }
    }


}