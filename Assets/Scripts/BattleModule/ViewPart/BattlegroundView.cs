using CharacterModule.ViewPart;
using UnityEngine;

namespace BattleModule.ViewPart
{
    public class BattlegroundView : MonoBehaviour
    {
        [SerializeField] private Transform[] playerSidePositions;
        [SerializeField] private Transform[] enemiesSidePositions;

        public void SetCharactersOnBattleground(PlayerCharacterView[] playersView, EnemyCharacterView[] enemiesView)
        {
            for (int i = 0; i < playersView.Length; i++)
            {
                playersView[i].transform.position = playerSidePositions[i].position;
            }

            for (int i = 0; i < enemiesView.Length; i++)
            {
                enemiesView[i].transform.position = enemiesSidePositions[i].position;
            }
        }
    }
}