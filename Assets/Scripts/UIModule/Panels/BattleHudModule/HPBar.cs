using CharacterModule.PresenterPart;
using UnityEngine;
using UnityEngine.UI;

namespace UIModule.Panels.BattleHudModule
{
    public class HPBar : MonoBehaviour
    {
        [SerializeField] private GridLayoutGroup gridLayoutGroup;

        private float _standartSize;
        private CharacterPresenter _subscribedCharacter;
        
        public void Initialize()
        {
            _standartSize = gridLayoutGroup.cellSize.x;
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
            gridLayoutGroup.cellSize = new Vector2(_standartSize, gridLayoutGroup.cellSize.y);

            if (_subscribedCharacter != null)
            {
                _subscribedCharacter.Model.Damaged -= UpdateHPBar;
                _subscribedCharacter.Model.Death -= Unsubscribe;
            }
            HideBar();
        }

        public void HideBar()
        {
            gameObject.SetActive(false);
        } 

        private void UpdateHPBar(int maxHealth, int newHealth)
        {
            gridLayoutGroup.cellSize = new Vector2(_standartSize * newHealth / maxHealth, gridLayoutGroup.cellSize.y);
        }

        
    }
}