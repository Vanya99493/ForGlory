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

        private void Awake()
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

        public void MoveCharacter(Vector3 targetPosition, float movementTime)
        {
            StartCoroutine(MoveCharacterCoroutine(targetPosition, movementTime));
        }
        
        public void DestroyView()
        {
            Die();
            StartCoroutine(DestroyAfterTime(1f));
        }

        private IEnumerator MoveCharacterCoroutine(Vector3 targetPosition, float movementTime)
        {
            Vector3 currentPosition = transform.position;
            Quaternion startRotation = transform.rotation;
            
            Vector3 direction = targetPosition - currentPosition;
            /*float angle = Vector3.Angle(Vector3.forward, direction);
            float sign = Mathf.Sign(Vector3.Cross(Vector3.up, direction).y);
            float eulerAngleY = angle * sign;*/
            transform.rotation = Quaternion.LookRotation(direction);
            transform.Rotate(new Vector3(0, 180, 0));

            float startTime = Time.time;
            Move(true);
            while (true)
            {
                float distCovered = Time.time - startTime;
                float fracJourney = distCovered / movementTime;
                transform.position = Vector3.Lerp(currentPosition, targetPosition, fracJourney);
                
                if (fracJourney >= 1f)
                    break;
                yield return null;
            }
            Move(false);
            transform.rotation = startRotation;
        }

        private IEnumerator DestroyAfterTime(float time)
        {
            yield return new WaitForSeconds(time);
            Destroy?.Invoke(this);
        }
    }
}