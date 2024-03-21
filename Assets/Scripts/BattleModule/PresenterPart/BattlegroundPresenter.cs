using System;
using System.Collections;
using System.Collections.Generic;
using BattleModule.ModelPart;
using BattleModule.ViewPart;
using CharacterModule.PresenterPart;
using Infrastructure.ServiceLocatorModule;
using PlaygroundModule.ModelPart;
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
            
            _model.PlayerTeam.View.Rotate(Direction.Right);
            _model.EnemyTeam.View.Rotate(Direction.Left);
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
                    do
                    {
                        _clickedCharacter = null;
                        while (_clickedCharacter == null)
                        {
                            yield return null;
                        }
                    } while (_model.EnemyTeam.Model.GetCharacterPresenter(2) != null &&
                             _clickedCharacter.Model.Id == _model.EnemyTeam.Model.GetCharacterPresenter(2).Model.Id && 
                             _model.EnemyTeam.Model.GetAliveCharactersCount() != 1);
                    
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
                        do
                        {
                            randomIndex = Random.Range(0, _model.PlayerTeam.Model.CharactersCount);
                            
                        } while (_model.PlayerTeam.Model.GetCharacterPresenter(randomIndex) == null || 
                                 (_model.PlayerTeam.Model.GetCharacterPresenter(2) != null &&
                                  _model.PlayerTeam.Model.GetCharacterPresenter(randomIndex).Model.Id == _model.PlayerTeam.Model.GetCharacterPresenter(2).Model.Id && 
                                  _model.PlayerTeam.Model.GetAliveCharactersCount() != 1));
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
            {
                if (_model.PlayerTeam.Model.GetCharacterPresenter(i) == null)
                    continue;
                list.Add(_model.PlayerTeam.Model.GetCharacterPresenter(i));
            }

            for (int i = 0; i < _model.EnemyTeam.Model.CharactersCount; i++)
            {
                if (_model.EnemyTeam.Model.GetCharacterPresenter(i) == null)
                    continue;
                list.Add(_model.EnemyTeam.Model.GetCharacterPresenter(i));
            }
            
            list.Sort((first, second) => second.Model.Initiative.CompareTo(first.Model.Initiative));

            Queue<CharacterPresenter> attackQueue = new Queue<CharacterPresenter>();
            foreach (CharacterPresenter characterPresenter in list)
            {
                attackQueue.Enqueue(characterPresenter);
            }

            return attackQueue;
        }

        private bool IsTeamAlive(TeamPresenter team)
        {
            var characters = team.Model.GetCharacters();

            foreach (var character in characters)
                if (character.Model.Health > 0)
                    return true;

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
            var characters = _model.EnemyTeam.Model.GetCharacters();

            foreach (var character in characters)
            {
                character.ClickedAction += OnClickEnemy;
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
            var characters = _model.EnemyTeam.Model.GetCharacters();

            foreach (var character in characters)
            {
                character.ClickedAction -= OnClickEnemy;
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