﻿using CharacterModule.PresenterPart;
using UnityEngine;
using UnityEngine.UI;

namespace UIModule.Panels.BattleHudModule
{
    public class HPBar : MonoBehaviour
    {
        private GridLayoutGroup _gridLayoutGroup;
        private float _standartSize;

        private CharacterPresenter _subscribedCharacter;
        
        public void Initialize()
        {
            _gridLayoutGroup = gameObject.GetComponent<GridLayoutGroup>();
            _standartSize = _gridLayoutGroup.cellSize.x;
            HideBar();
        }

        public void Subscribe(CharacterPresenter character)
        {
            gameObject.SetActive(true);
            _subscribedCharacter = character;
            _subscribedCharacter.Model.Damaged += UpdateHPBar;
            _subscribedCharacter.Model.Death += Unsubscribe;
            UpdateHPBar(character.Model.MaxHealth, character.Model.Health);
        }
        
        public void Unsubscribe()
        {
            _gridLayoutGroup.cellSize = new Vector2(_standartSize, _gridLayoutGroup.cellSize.y);
            _subscribedCharacter.Model.Damaged -= UpdateHPBar;
            _subscribedCharacter.Model.Death -= Unsubscribe;
            HideBar();
        }

        public void HideBar()
        {
            gameObject.SetActive(false);
        } 

        private void UpdateHPBar(int maxHealth, int newHealth)
        {
            _gridLayoutGroup.cellSize = new Vector2(_standartSize * newHealth / maxHealth, _gridLayoutGroup.cellSize.y);
        }

        
    }
}