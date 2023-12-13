using BattleModule.ViewPart;
using UnityEngine;

namespace BattleModule.PresenterPart
{
    public class BattlegroundFactory
    {
        public BattlegroundView InstantiateBattleground(BattlegroundView battlegroundPrefab)
        {
            BattlegroundView battlegroundView = Object.Instantiate(battlegroundPrefab);
            battlegroundView.Destroy += OnDestroy;
            battlegroundView.gameObject.SetActive(false);
            return battlegroundView;
        }

        private void OnDestroy(BattlegroundView gameObject)
        {
            Object.Destroy(gameObject.gameObject);
        }
    }
}