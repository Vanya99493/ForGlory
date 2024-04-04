using System;
using CharacterModule.PresenterPart;
using Infrastructure.CoroutineRunnerModule;
using UnityEngine;

namespace BattleModule.ViewPart
{
    public class BattlegroundView : MonoBehaviour, ICoroutineRunner
    {
        public event Action<BattlegroundView> Destroy;
        
        [SerializeField] private Transform[] playerSidePositions;
        [SerializeField] private Transform[] enemiesSidePositions;
        [SerializeField] private Transform attackStepPosition;
        [SerializeField] private Transform[] attackPlayersPositions;
        [SerializeField] private Transform[] attackEnemiesPositions;

        private Vector3 _lastPosition;

        public Vector3 GetAttackStepPosition() => attackStepPosition.position;
        public Vector3 GetAttackPlayerPosition(int index) => attackPlayersPositions[index].position;
        public Vector3 GetAttackEnemyPosition(int index) => attackEnemiesPositions[index].position;

        public void SetCharactersOnBattleground(PlayerTeamPresenter players, EnemyTeamPresenter enemies)
        {
            for (int i = 0; i < players.Model.CharactersCount; i++)
            {
                var player = players.Model.GetCharacterPresenter(i);
                if (player == null)
                    continue;
                player.View.transform.position = playerSidePositions[i].position;
            }

            for (int i = 0; i < enemies.Model.CharactersCount; i++)
            {
                var enemy = enemies.Model.GetCharacterPresenter(i);
                if (enemy == null)
                    continue;
                enemy.View.transform.position = enemiesSidePositions[i].position;
            }
        }

        public void SetAttackPosition(CharacterPresenter attackingCharacter, float movementTime)
        {
            _lastPosition = attackingCharacter.View.transform.position;
            attackingCharacter.MoveCharacter(attackStepPosition.position, movementTime);
            //attackingCharacter.View.transform.position = attackStepPosition.position;
        }

        public void SetAttackPlayerPosition(CharacterPresenter attackingCharacter, int attackedPlayerNumber, float movementTime)
        {
            attackingCharacter.MoveCharacter(attackPlayersPositions[attackedPlayerNumber].position, movementTime);
            //attackingCharacter.View.transform.position = attackPlayersPositions[attackedPlayerNumber].position;
        }

        public void SetAttackEnemyPosition(CharacterPresenter attackingCharacter, int attackedEnemyNumber, float movementTime)
        {
            attackingCharacter.MoveCharacter(attackEnemiesPositions[attackedEnemyNumber].position, movementTime);
            //attackingCharacter.View.transform.position = attackEnemiesPositions[attackedEnemyNumber].position;
        }

        public void ResetAttackPosition(CharacterPresenter attackingCharacter, float movementTime)
        {
            //attackingCharacter.View.transform.position = _lastPosition;
            attackingCharacter.MoveCharacter(_lastPosition, movementTime);
            _lastPosition = new Vector3(0, 0, 0);
        }

        public void ActivateBattleground()
        {
            gameObject.SetActive(true);
        }

        public void DeactivateBattleground()
        {
            gameObject.SetActive(false);
        }

        public void DestroyView()
        {
            Destroy?.Invoke(this);
        }
    }
}