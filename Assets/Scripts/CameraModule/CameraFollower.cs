using UnityEngine;

namespace CameraModule
{
    public class CameraFollower : MonoBehaviour
    {
        [SerializeField] private Vector3 offset;

        private Transform _target;
        private bool _haveTarget = false;
        private bool _isLocked = false;

        public void SetTarget(Transform target)
        {
            _target = target;
            _haveTarget = true;
        }

        public void ResetTarget()
        {
            _haveTarget = false;
            _target = null;
        }

        public void StayAtZero()
        {
            _isLocked = true;
        }

        public void FollowTarget()
        {
            _isLocked = false;
        }
        
        private void LateUpdate()
        {
            if (!_haveTarget)
            {
                return;
            }

            transform.position = (_isLocked ? new Vector3(0, 0, 0) : _target.position) + offset;
        }
    }
}