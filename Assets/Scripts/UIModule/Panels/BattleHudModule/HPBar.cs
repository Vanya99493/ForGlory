using CharacterModule.PresenterPart;
using UnityEngine;
using UnityEngine.UI;

namespace UIModule.Panels.BattleHudModule
{
    public class HPBar : MonoBehaviour
    {
        [SerializeField] private GridLayoutGroup gridLayoutGroup;
        [SerializeField] private float standardSize;

        private CharacterPresenter _subscribedCharacter;
        
        public void Initialize()
        {
            HideBar();
        }

        public void Subscribe(CharacterPresenter character)
        {
            gameObject.SetActive(true);
            _subscribedCharacter = character;
            _subscribedCharacter.Model.Damaged += UpdateHPBar;
            _subscribedCharacter.Model.Death += Unsubscribe;
            UpdateHPBar(_subscribedCharacter.Model.MaxHealth, _subscribedCharacter.Model.Health);
        }

        public void Update()
        {
            if(_subscribedCharacter != null)
                gameObject.SetActive(true);
        }
        
        public void Unsubscribe()
        {
            gridLayoutGroup.cellSize = new Vector2(standardSize, gridLayoutGroup.cellSize.y);

            if (_subscribedCharacter != null)
            {
                _subscribedCharacter.Model.Damaged -= UpdateHPBar;
                _subscribedCharacter.Model.Death -= Unsubscribe;
            }
            HideBar();
        }

        private void HideBar()
        {
            gameObject.SetActive(false);
        } 

        private void UpdateHPBar(int maxHealth, int newHealth)
        {
            gridLayoutGroup.cellSize = new Vector2(standardSize * newHealth / maxHealth, gridLayoutGroup.cellSize.y);
        }
    }
}