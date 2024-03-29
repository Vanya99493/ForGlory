using UnityEngine;

namespace CharacterModule.ViewPart.AnimationControllerModule
{
    public class CharacterAnimationController : MonoBehaviour
    {
        [SerializeField] private string isRunningBoolName = "IsRunning";
        [SerializeField] private string attackingTriggerName = "Attacking";
        [SerializeField] private string defendingTriggerName = "Defending";
        [SerializeField] private string dyingTriggerName = "Dying";

        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public void SubscribeAnimations(CharacterView characterView, 
            bool hasRunningState, bool hasAttackingState,
            bool hasDefendingState, bool hasDyingState)
        {
            if (hasRunningState)
                characterView.MovingAction += Running;
            if (hasAttackingState)
                characterView.AttackingAction += Attacking;
            if (hasDefendingState)
                characterView.DefendingAction += Defending;
            if (hasDyingState)
                characterView.DyingAction += Dying;
        }

        public void UnsubscribeAnimations(CharacterView characterView)
        {
            characterView.MovingAction -= Running;
            characterView.AttackingAction -= Attacking;
            characterView.DefendingAction -= Defending;
            characterView.DyingAction -= Dying;
        }

        private void Running(bool isRunning) => _animator.SetBool(isRunningBoolName, isRunning);
        private void Attacking() => _animator.SetTrigger(attackingTriggerName);
        private void Defending() => _animator.SetTrigger(defendingTriggerName);
        private void Dying() => _animator.SetTrigger(dyingTriggerName);
    }
}