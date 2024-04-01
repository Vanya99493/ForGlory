﻿using System;
using Infrastructure.ServiceLocatorModule;
using ScriptableObjects;
using UIModule.Panels.CastleMenuModule;
using UIModule.Panels.LoadLevelMenuModule;
using UnityEngine;

namespace Infrastructure.Providers
{
    [Serializable]
    public class UIPrefabsProvider : IService
    {
        [SerializeField] private UIPrefabsContainer uiPrefabsContainer;

        public HeroCard GetHeroCard() => uiPrefabsContainer.HeroCardPrefab;
        public LevelSaveCard GetLevelSaveCard() => uiPrefabsContainer.LevelSaveCardPrefab;
    }
}