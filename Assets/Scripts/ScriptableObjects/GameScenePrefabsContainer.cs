using MVP.Battleground;
using MVP.Playground;
using UIModule;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "GameScenePrefabsContainer", menuName = "ScriptableObjects/GameScenePrefabsContainer", order = 1)]
    public class GameScenePrefabsContainer : ScriptableObject
    {
        [SerializeField] private UIController uiController;
        [SerializeField] private PlaygroundView playgroundView;
        [SerializeField] private BattlegroundView battlegroundView;

        public UIController GetUIController() => uiController;
        public PlaygroundView GetPlaygroundView() => playgroundView;
        public BattlegroundView GetBattlegroundView() => battlegroundView;
    }
}