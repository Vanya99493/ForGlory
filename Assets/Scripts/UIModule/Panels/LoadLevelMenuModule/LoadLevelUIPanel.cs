using System;
using System.Collections.Generic;
using Infrastructure.Providers;
using Infrastructure.ServiceLocatorModule;
using Infrastructure.Services;
using UIModule.Panels.Base;
using UnityEngine;
using UnityEngine.UI;

namespace UIModule.Panels.LoadLevelMenuModule
{
    public class LoadLevelUIPanel : BaseUIPanel
    {
        public event Action EnterLoadLevelAction;
        public event Action GoBackAction;

        [SerializeField] private Button goBackButton;
        [SerializeField] private GameObject contentParent;

        private LevelSaveCard[] _saveCards;

        private void Awake()
        {
            _saveCards = Array.Empty<LevelSaveCard>();
        }

        public void FillSaves(Dictionary<int, string> saves, Action<int> onSelectSave, Action<int> onDeleteSave)
        {
            RemoveSaveCards();

            var levelSaveCardPrefab = ServiceLocator.Instance.GetService<UIPrefabsProvider>().GetLevelSaveCard();
            _saveCards = new LevelSaveCard[saves.Count];
            int i = 0;
            foreach (var save in saves)
            {
                _saveCards[i] = Instantiate(levelSaveCardPrefab, contentParent.transform);
                _saveCards[i].Initialize(save.Key, save.Value);
                _saveCards[i].LoadSaveAction += onSelectSave;
                _saveCards[i].DeleteSaveAction += onDeleteSave;
                i++;
            }
        }
        
        protected override void SubscribeActions()
        {
            ServiceLocator.Instance.GetService<PauseController>().TurnOnInputPause();
            goBackButton.onClick.AddListener(OnGoBack);
            EnterLoadLevelAction?.Invoke();
        }

        protected override void UnsubscribeActions()
        {
            ServiceLocator.Instance.GetService<PauseController>().TurnOffInputPause();
            RemoveSaveCards();
            goBackButton.onClick.RemoveListener(OnGoBack);
        }

        private void RemoveSaveCards()
        {
            for (int i = 0; i < _saveCards.Length; i++)
            {
                _saveCards[i].Destroy();
            }
            _saveCards = Array.Empty<LevelSaveCard>();
        }

        private void OnGoBack()
        {
            GoBackAction?.Invoke();
        }
    }
}