using System;
using LevelModule.Data;
using UnityEngine;
using UnityEngine.UI;

namespace UIModule.Panels.MainMenuModule
{
    public class SelectDifficultyPanel : MonoBehaviour
    {
        public event Action<LevelDifficulty> SelectDifficultyAction;

        [SerializeField] private Button backToMainMenuButton;
        [SerializeField] private Button selectEasyDifficultyButton;
        [SerializeField] private Button selectMediumDifficultyButton;
        [SerializeField] private Button selectHardDifficultyButton;
        
        public void Initialize()
        {
            backToMainMenuButton.onClick.AddListener(() => gameObject.SetActive(false));
            selectEasyDifficultyButton.onClick.AddListener(() => SelectDifficultyAction?.Invoke(LevelDifficulty.Easy));
            selectMediumDifficultyButton.onClick.AddListener(() => SelectDifficultyAction?.Invoke(LevelDifficulty.Medium));
            selectHardDifficultyButton.onClick.AddListener(() => SelectDifficultyAction?.Invoke(LevelDifficulty.Hard));
        }
    }
}