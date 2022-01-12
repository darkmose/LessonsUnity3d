namespace FSM
{
    public class AmmoSearchState : BaseState
    {
        public AmmoSearchState(IStateSwitcher switcher, AIShared aIShared) : base(switcher, aIShared)
        {
        }

        public override States State => States.AmmunitionSearch;

        public override void OnStateStay()
        {
            if (_aIShared.Weapon.HasAmmo == false)
            {
                if (_aIShared.Navigation.IsMovedToPoint == false)
                {
                    _aIShared.Navigation.SetDestination(MapPointsHelper.GetRandomPointFromList(MapPointsHelper.PointsList.AmmoPoints));
                }
            }
            else
            {
                _stateSwitcher.SwitchStateTo(States.EnemiesSearch);
            }
        }

    }


}