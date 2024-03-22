using CharacterModule.PresenterPart;
using UIModule.Panels.Base;
using UnityEngine;

namespace UIModule.Panels.BattleHudModule
{
    public class InfoPanel : BasePanel
    {
        [Header("Heroes team HP bars")]
        [SerializeField] private HPBar heroRearguardHPBar;
        [SerializeField] private HPBar heroLeftVanguardHPBar;
        [SerializeField] private HPBar heroRightVanguardHPBar;
        [Header("Enemies team HP bars")]
        [SerializeField] private HPBar enemyRearguardHPBar;
        [SerializeField] private HPBar enemyLeftVanguardHPBar;
        [SerializeField] private HPBar enemyRightVanguardHPBar;

        public void SubscribeBars(PlayerTeamPresenter playerTeam, EnemyTeamPresenter enemyTeam)
        {
            if (playerTeam.Model.GetCharacterPresenter(0) != null)
                heroLeftVanguardHPBar.Subscribe(playerTeam.Model.GetCharacterPresenter(0));
            if (playerTeam.Model.GetCharacterPresenter(1) != null)
                heroRightVanguardHPBar.Subscribe(playerTeam.Model.GetCharacterPresenter(1));
            if (playerTeam.Model.GetCharacterPresenter(2) != null)
                heroRearguardHPBar.Subscribe(playerTeam.Model.GetCharacterPresenter(2));
            if (enemyTeam.Model.GetCharacterPresenter(0) != null)
                enemyLeftVanguardHPBar.Subscribe(enemyTeam.Model.GetCharacterPresenter(0));
            if (enemyTeam.Model.GetCharacterPresenter(1) != null)
                enemyRightVanguardHPBar.Subscribe(enemyTeam.Model.GetCharacterPresenter(1));
            if (enemyTeam.Model.GetCharacterPresenter(2) != null)
                enemyRearguardHPBar.Subscribe(enemyTeam.Model.GetCharacterPresenter(2));
        }
    }
}