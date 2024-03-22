using CharacterModule.PresenterPart;
using UnityEngine;
using UnityEngine.UI;

namespace UIModule.Panels.BattleHudModule
{
    public class HPBar : MonoBehaviour
    {
        private GridLayoutGroup _gridLayoutGroup;
        private float _standartSize;

        private void Awake()
        {
            _gridLayoutGroup = gameObject.GetComponent<GridLayoutGroup>();
            _standartSize = _gridLayoutGroup.cellSize.x;
            gameObject.SetActive(false);
        }

        public void Subscribe(CharacterPresenter character)
        {
            gameObject.SetActive(true);
            character.Model.Damaged += UpdateHPBar;
            character.Model.Death += Unsubscribe;
            UpdateHPBar(character.Model.MaxHealth, character.Model.Health);
        }

        private void UpdateHPBar(int maxHealth, int newHealth)
        {
            _gridLayoutGroup.cellSize = new Vector2(_standartSize * newHealth / maxHealth, _gridLayoutGroup.cellSize.y);
        }

        private void Unsubscribe()
        {
            _gridLayoutGroup.cellSize = new Vector2(_standartSize, _gridLayoutGroup.cellSize.y);
            gameObject.SetActive(false);
        }
    }
}