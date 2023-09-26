using System;
using BattleModule.ViewPart;
using PlaygroundModule.ViewPart;
using ScriptableObjects;
using UIModule;
using UnityEngine;

namespace Infrastructure.Providers
{
    [Serializable]
    public class GameScenePrefabsProvider
    {
        [SerializeField] private GameScenePrefabsContainer gameScenePrefabsContainer;

        public UIController GetUIController() => gameScenePrefabsContainer.UIControllerPrefab;
        public PlaygroundView GetPlaygroundView() => gameScenePrefabsContainer.PlaygroundViewPrefab;
        public BattlegroundView GetBattlegroundView() => gameScenePrefabsContainer.BattlegroundViewPrefab;
    }
}