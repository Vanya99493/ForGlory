using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace UIModule.Panels.LoadLevelMenuModule
{
    public class LevelSaveCard : MonoBehaviour
    {
        public event Action<int> LoadSaveAction;
        public event Action<int> DeleteSaveAction;

        [SerializeField] private TextMeshProUGUI saveNameText;
        [SerializeField] private Button loadSaveButton;
        [SerializeField] private Button deleteSaveAction;
        
        public int LevelIndex { get; private set; }
        
        public void Initialize(int levelIndex, string save)
        {
            LevelIndex = levelIndex;
            saveNameText.text = $"{save} {LevelIndex}";
            SubscribeActions();
        }

        public void Destroy()
        {
            UnsubscribeActions();
            Object.Destroy(gameObject);
        }

        private void SubscribeActions()
        {
            loadSaveButton.onClick.AddListener(LoadLevel);
            deleteSaveAction.onClick.AddListener(DeleteLevel);
        }

        private void UnsubscribeActions()
        {
            loadSaveButton.onClick.RemoveListener(LoadLevel);
            deleteSaveAction.onClick.RemoveListener(DeleteLevel);
        }

        private void LoadLevel()
        {
            LoadSaveAction?.Invoke(LevelIndex);
        }

        private void DeleteLevel()
        {
            DeleteSaveAction?.Invoke(LevelIndex);
        }
    }
}