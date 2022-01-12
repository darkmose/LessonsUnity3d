namespace FSM
{
    public class WeaponSearchState : BaseState 
    {
        public WeaponSearchState(IStateSwitcher switcher, AIShared aIShared) : base(switcher, aIShared)
        {
        }

        public override States State => States.WeaponSearch;

        public override void OnStateStay()
        {
            if (_aIShared.Weapon.HasWeapon == false)
            {
                if (_aIShared.Navigation.IsMovedToPoint == false)
                {
                    _aIShared.Navigation.SetDestination(MapPointsHelper.GetRandomPointFromList(MapPointsHelper.PointsList.WeaponPoints));
                }
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