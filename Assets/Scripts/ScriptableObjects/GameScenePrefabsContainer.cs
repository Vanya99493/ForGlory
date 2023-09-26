using BattleModule.ViewPart;
using PlaygroundModule.ViewPart;
using UIModule;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "GameScenePrefabsContainer", menuName = "ScriptableObjects/Prefabs/GameScenePrefabsContainer", order = 1)]
    public class GameScenePrefabsContainer : ScriptableObject
    {
        [SerializeField] private UIController uiController;
        [SerializeField] private PlaygroundView playgroundView;
        [SerializeField] private BattlegroundView battlegroundView;

        public UIController UIControllerPrefab => uiController;
        public PlaygroundView PlaygroundViewPrefab => playgroundView;
        public BattlegroundView BattlegroundViewPrefab => battlegroundView;
    }
}