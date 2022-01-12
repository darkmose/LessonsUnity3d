namespace FSM
{
    public class EnemiesAttackState : BaseState
    {
        public EnemiesAttackState(IStateSwitcher switcher, AIShared aIShared) : base(switcher, aIShared)
        {
        }

        public override States State => States.EnemiesAttack;

        public override void OnEnterState()
        {
            _aIShared.Navigation.GetRandomEnemyAsTarget();
        }

        public override void OnStateStay()
        {
            if (_aIShared.Navigation.IsEnemySpied)
            {
                _aIShared.Weapon.FireTarget(_aIShared.Navigation.EnemyTarget);
            }
            else if (_aIShared.Vitality.IsCriticalHP)
            {
                _stateSwitcher.SwitchStateTo(States.HealthPointsSearch);
            }
            else if (_aIShared.Weapon.HasAmmo == false)
            {
                _stateSwitcher.SwitchStateTo(States.AmmunitionSearch);
            }
            else
            {
                _stateSwitcher.SwitchStateTo(States.EnemiesSearch);
            }
        }
    }


}