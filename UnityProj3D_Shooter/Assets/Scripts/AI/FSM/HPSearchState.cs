namespace FSM
{
    class HPSearchState : BaseState
    {
        public HPSearchState(IStateSwitcher switcher, AIShared aIShared) : base(switcher, aIShared)
        {
        }

        public override States State => States.HealthPointsSearch;

        public override void OnStateStay()
        {
            if (_aIShared.Vitality.IsCriticalHP)
            {
                if (_aIShared.Navigation.IsMovedToPoint == false)
                {
                    _aIShared.Navigation.SetDestination(MapPointsHelper.GetRandomPointFromList(MapPointsHelper.PointsList.HealthPackPoints));
                }
            }
            else
            {
                _stateSwitcher.SwitchStateTo(States.EnemiesSearch);
            }
        }
    }


}