using System;
using Infrastructure.Providers;
using Infrastructure.ServiceLocatorModule;
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

        public void FillSaves(int[] levelSavesId, Action<int> onSelectSave, Action<int> onDeleteSave)
        {
            RemoveSaveCards();

            var levelSaveCardPrefab = ServiceLocator.Instance.GetService<UIPrefabsProvider>().GetLevelSaveCard();
            _saveCards = new LevelSaveCard[levelSavesId.Length];
            for (int i = 0; i < _saveCards.Length; i++)
            {
                _saveCards[i] = Instantiate(levelSaveCardPrefab, contentParent.transform);
                _saveCards[i].Initialize(levelSavesId[i]);
                _saveCards[i].LoadSaveAction += onSelectSave;
                _saveCards[i].DeleteSaveAction += onDeleteSave;
            }
        }
        
        protected override void SubscribeActions()
        {
            goBackButton.onClick.AddListener(OnGoBack);
            EnterLoadLevelAction?.Invoke();
        }

        protected override void UnsubscribeActions()
        {
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