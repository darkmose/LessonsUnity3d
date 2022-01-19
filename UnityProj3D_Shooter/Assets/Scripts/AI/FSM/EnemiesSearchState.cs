namespace FSM
{
    public class EnemiesSearchState : BaseState
    {
        public EnemiesSearchState(IStateSwitcher switcher, AIShared aIShared) : base(switcher, aIShared)
        {
        }

        public override States State => States.EnemiesSearch;

        public override void OnStateStay()
        {
            if (_aIShared.Weapon.HasWeapon)
            {
                if (_aIShared.Navigation.IsEnemiesSpied == false)
                {
                    if (_aIShared.Navigation.IsMovedToPoint == false)
                    {
                        _aIShared.Navigation.SetDestination(MapPointsHelper.GetRandomPointFromList(MapPointsHelper.PointsList.EnemiesPoints));
                    }
                }
                else
                {
                    _stateSwitcher.SwitchStateTo(States.EnemiesAttack);
                }
            }
            else
            {
                _stateSwitcher.SwitchStateTo(States.WeaponSearch);
            }
        }
    }


}