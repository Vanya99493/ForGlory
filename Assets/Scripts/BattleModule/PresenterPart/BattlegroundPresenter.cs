using System;
using System.Collections;
using System.Collections.Generic;
using BattleModule.ModelPart;
using BattleModule.ViewPart;
using CharacterModule.PresenterPart;
using Infrastructure.ServiceLocatorModule;
using UIModule;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BattleModule.PresenterPart
{
    public class BattlegroundPresenter
    {
        public event Action<bool, PlayerTeamPresenter, EnemyTeamPresenter> EndBattle;
        
        private BattlegroundModel _model;
        private BattlegroundView _view;

        private CharacterPresenter _clickedCharacter;

        public BattlegroundPresenter(BattlegroundModel battlegroundModel, BattlegroundView battlegroundView)
        {
            _model = battlegroundModel;
            _view = battlegroundView;
            
            _clickedCharacter = null;
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
            SubscribeOnClickActions();
            _view.StartCoroutine(BattleCoroutine());
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

        private IEnumerator BattleCoroutine()
        {
            Queue<CharacterPresenter> attackQueue = MakeAttackQueue();
            
            //while (_model.PlayerTeam.Model.CharactersCount > 0 && _model.EnemyTeam.Model.CharactersCount > 0 && attackQueue.Count > 0)
            while (IsTeamAlive(_model.PlayerTeam) && IsTeamAlive(_model.EnemyTeam) && attackQueue.Count > 0)
            {
                CharacterPresenter attackCharacter = attackQueue.Dequeue();
                if (attackCharacter.Model.Health <= 0)
                    continue;
                
                _view.SetAttackPosition(attackCharacter);

                if (attackCharacter is PlayerCharacterPresenter)
                {
                    while (_clickedCharacter == null)
                    {
                        yield return null;
                    }

                    _clickedCharacter.Model.TakeDamage(attackCharacter.Model.Damage);
                    _clickedCharacter = null;
                    yield return new WaitForSeconds(0.5f);
                }
                else if (attackCharacter is EnemyCharacterPresenter)
                {
                    yield return new WaitForSeconds(1f);
                    int randomIndex;
                    while (true)
                    {
                        randomIndex = Random.Range(0, _model.PlayerTeam.Model.CharactersCount);
                        if (_model.PlayerTeam.Model.GetCharacterPresenter(randomIndex).Model.Health > 0)
                            break;
                    }
                    _model.PlayerTeam.Model.GetCharacterPresenter(randomIndex).Model.TakeDamage(attackCharacter.Model.Damage);
                    yield return new WaitForSeconds(1f);
                }
                
                _view.ResetAttackPosition(attackCharacter);
                attackQueue.Enqueue(attackCharacter);
                
                yield return new WaitForSeconds(0.5f);
            }
            
            
            UnsubscribeUIActions();
            UnsubscribeOnClickActions();

            if (IsTeamAlive(_model.EnemyTeam))
            {
                EndBattle?.Invoke(false, _model.PlayerTeam, _model.EnemyTeam);
            }
            else if (IsTeamAlive(_model.PlayerTeam))
            {
                EndBattle?.Invoke(true, _model.PlayerTeam, _model.EnemyTeam);
            }
        }

        private Queue<CharacterPresenter> MakeAttackQueue()
        {
            List<CharacterPresenter> list = new List<CharacterPresenter>();
            for (int i = 0; i < _model.PlayerTeam.Model.CharactersCount; i++)
                list.Add(_model.PlayerTeam.Model.GetCharacterPresenter(i));

            for (int i = 0; i < _model.EnemyTeam.Model.CharactersCount; i++)
                list.Add(_model.EnemyTeam.Model.GetCharacterPresenter(i));
            
            list.Sort((first, second) => first.Model.Id.CompareTo(second.Model.Id));

            Queue<CharacterPresenter> attackQueue = new Queue<CharacterPresenter>();
            foreach (CharacterPresenter characterPresenter in list)
            {
                attackQueue.Enqueue(characterPresenter);
            }

            return attackQueue;
        }

        private bool IsTeamAlive(TeamPresenter team)
        {
            for (int i = 0; i < team.Model.CharactersCount; i++)
            {
                if (team.Model.GetCharacterPresenter(i).Model.Health > 0)
                    return true;
            }

            return false;
        }

        private void SubscribeUIActions()
        {
            var uiController = ServiceLocator.Instance.GetService<UIController>();
            uiController.battleHudPanel.AvoidAction += OnAvoidBattle;
            uiController.battleHudPanel.WinAction += OnWinBattle;
        }

        private void SubscribeOnClickActions()
        {
            for (int i = 0; i < _model.EnemyTeam.Model.CharactersCount; i++)
            {
                _model.EnemyTeam.Model.GetCharacterPresenter(i).ClickedAction += OnClickEnemy;
            }
        }

        private void UnsubscribeUIActions()
        {
            var uiController = ServiceLocator.Instance.GetService<UIController>();
            uiController.battleHudPanel.AvoidAction -= OnAvoidBattle;
            uiController.battleHudPanel.WinAction -= OnWinBattle;
        }

        private void UnsubscribeOnClickActions()
        {
            for (int i = 0; i < _model.EnemyTeam.Model.CharactersCount; i++)
            {
                _model.EnemyTeam.Model.GetCharacterPresenter(i).ClickedAction -= OnClickEnemy;
            }
        }

        private void OnClickEnemy(CharacterPresenter clickedCharacter)
        {
            _clickedCharacter = clickedCharacter;
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