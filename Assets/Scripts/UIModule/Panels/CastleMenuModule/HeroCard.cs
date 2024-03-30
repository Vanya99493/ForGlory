using System;
using CharacterModule.PresenterPart;
using UIModule.Panels.BattleHudModule;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace UIModule.Panels.CastleMenuModule
{
    public class HeroCard : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public event Action<CharacterPresenter, Sprite> ClickAction;
        
        [SerializeField] private Image heroIcon;
        [SerializeField] private HPBar hpBar;

        private CanvasGroup _canvasGroup;
        private CharacterPresenter _character;
        private GameObject _tempParent;

        public int CharacterId => _character.Model.Id;
        public Transform LastParent { get; private set; }

        public void Initialize(CharacterPresenter character, Sprite heroSprite, GameObject tempParent)
        {
            _character = character;
            hpBar.Initialize();
            hpBar.Subscribe(_character);
            heroIcon.sprite = heroSprite;
            _tempParent = tempParent;
        }
        
        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _canvasGroup.alpha = 0.8f;
            _canvasGroup.blocksRaycasts = false;
            LastParent = transform.parent;
            eventData.pointerDrag.transform.SetParent(_tempParent.transform);
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = new Vector3(
                transform.position.x + eventData.delta.x, 
                transform.position.y + eventData.delta.y, 
                transform.position.z 
                );
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _canvasGroup.alpha = 1f;
            _canvasGroup.blocksRaycasts = true;

            if (transform.parent.Equals(_tempParent.transform))
            {
                eventData.pointerDrag.transform.SetParent(LastParent);
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            ClickAction?.Invoke(_character, heroIcon.sprite);
        }

        public void Destroy()
        {
            hpBar.Unsubscribe();
            Object.Destroy(gameObject);
        }
    }
}