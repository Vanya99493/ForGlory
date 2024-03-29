using System;
using System.Collections;
using CharacterModule.ViewPart.AnimationControllerModule;
using Infrastructure.InputHandlerModule;
using Interfaces;
using UnityEngine;

namespace CharacterModule.ViewPart
{
    public class CharacterView : MonoBehaviour, IClickable
    {
        [SerializeField] private CharacterAnimationController animationController;
        [SerializeField] private bool hasRunningState;
        [SerializeField] private bool hasAttackingState;
        [SerializeField] private bool hasDefendingState;
        [SerializeField] private bool hasDyingState;
        
        public event Action ClickedAction;
        public event Action<CharacterView> Destroy;

        public event Action<bool> MovingAction;
        public event Action AttackingAction;
        public event Action DefendingAction;
        public event Action DyingAction;

        public void Awake()
        {
            if(animationController != null)
                animationController.SubscribeAnimations(this, hasRunningState, hasAttackingState, hasDefendingState, hasDyingState);
        }
        
        public void HideView()
        {
            gameObject.SetActive(false);
        }
        
        public void Move(bool isRunning) => MovingAction?.Invoke(isRunning);
        public void Attack() => AttackingAction?.Invoke();
        public void Defend() => DefendingAction?.Invoke();
        public void Die() => DyingAction?.Invoke();

        public void Click(InputMouseButtonType mouseButtonType)
        {
            ClickedAction?.Invoke();
        }
        
        public void DestroyView()
        {
            Die();
            StartCoroutine(DestroyAfterTime(1f));
        }

        private IEnumerator DestroyAfterTime(float time)
        {
            yield return new WaitForSeconds(time);
            Destroy?.Invoke(this);
        }
    }
}