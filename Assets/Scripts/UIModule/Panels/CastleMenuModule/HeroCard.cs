using System;
using CharacterModule.PresenterPart;
using UIModule.Panels.BattleHudModule;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UIModule.Panels.CastleMenuModule
{
    public class HeroCard : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public event Action<CharacterPresenter, Image> ClickAction;
        
        [SerializeField] private Image heroIcon;
        [SerializeField] private HPBar hpBar;
        [SerializeField] private GameObject tempParent;

        private CanvasGroup _canvasGroup;
        private CharacterPresenter _character;
        
        public Transform LastParent { get; private set; }

        public void Initialize(CharacterPresenter character, Image heroIcon)
        {
            _character = character;
            hpBar.Initialize();
            hpBar.Subscribe(_character);
            this.heroIcon.color = heroIcon.color;
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
            eventData.pointerDrag.transform.SetParent(tempParent.transform);
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
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            ClickAction?.Invoke(_character, heroIcon);
        }
    }
}