using System;
using System.Collections.Generic;
using BattleModule.ModelPart;
using BattleModule.ViewPart;
using CharacterModule.PresenterPart;
using Infrastructure.ServiceLocatorModule;
using UIModule;
using UnityEngine;

namespace BattleModule.PresenterPart
{
    public class BattlegroundPresenter
    {
        public event Action<bool, PlayerTeamPresenter, EnemyTeamPresenter> EndBattle;
        
        private BattlegroundModel _model;
        private BattlegroundView _view;

        public BattlegroundPresenter(BattlegroundModel battlegroundModel, BattlegroundView battlegroundView)
        {
            _model = battlegroundModel;
            _view = battlegroundView;
        }
        
        public void StartBattle(List<TeamPresenter> teams)
        {
            bool isWin = true;
            
            if (teams[0] is PlayerTeamPresenter)
            {
                _model.SetTeams(teams[0] as PlayerTeamPresenter, teams[1] as EnemyTeamPresenter);
            }
            else
            {
                _model.SetTeams(teams[1] as PlayerTeamPresenter, teams[0] as EnemyTeamPresenter);
            }
            
            _view.SetCharactersOnBattleground(_model.PlayerTeam, _model.EnemyTeam);

            SubscribeUIActions();

            // EndBattle?.Invoke(isWin, _model.PlayerTeam, _model.EnemyTeam);
        }

        public void ShowBattleground()
        {
            _view.ActivateBattleground();
        }

        public void HideBattleground()
        {
            _view.DeactivateBattleground();
        }

        public void Destroy()
        {
            _view.DestroyView();
        }

        private void SubscribeUIActions()
        {
            var uiController = ServiceLocator.Instance.GetService<UIController>();
            uiController.battleHudPanel.AvoidAction += OnAvoidBattle;
            uiController.battleHudPanel.WinAction += OnWinBattle;
        }

        private void UnsubscribeUIActions()
        {
            var uiController = ServiceLocator.Instance.GetService<UIController>();
            uiController.battleHudPanel.AvoidAction -= OnAvoidBattle;
            uiController.battleHudPanel.WinAction -= OnWinBattle;
        }

        private void OnAvoidBattle()
        {
            UnsubscribeUIActions();
            EndBattle?.Invoke(false, _model.PlayerTeam, _model.EnemyTeam);
        }

        private void OnWinBattle()
        {
            UnsubscribeUIActions();
            EndBattle?.Invoke(true, _model.PlayerTeam, _model.EnemyTeam);
        }
    }
}