using UnityEngine;

namespace CameraModule
{
    public class CameraFollower : MonoBehaviour
    {
        [SerializeField] private Vector3 offset;

        private Transform _target;
        private bool _isLocked = false;

        public void SetTarget(Transform target)
        {
            _target = target;
        }

        public void ResetTarget()
        {
            _isLocked = false;
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
            if (_target == null)
            {
                return;
            }

            transform.position = (_isLocked ? new Vector3(0, 0, 0) : _target.position) + offset;
        }
    }
}