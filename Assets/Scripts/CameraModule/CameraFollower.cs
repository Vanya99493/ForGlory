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

        public void LockPosition()
        {
            _isLocked = true;
        }

        public void UnlockPosition()
        {
            _isLocked = false;
        }
        
        private void LateUpdate()
        {
            if (_target == null || _isLocked)
            {
                transform.position = Vector3.zero + offset;
                return;
            }

            transform.position = _target.position + offset;
        }
    }
}