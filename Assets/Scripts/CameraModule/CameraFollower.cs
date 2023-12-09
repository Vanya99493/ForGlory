using UnityEngine;

namespace CameraModule
{
    public class CameraFollower : MonoBehaviour
    {
        [SerializeField] private Vector3 offset;

        private Transform _target;
        private bool _haveTarget = false;

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
        
        private void LateUpdate()
        {
            if (!_haveTarget)
            {
                return;
            }

            transform.position = _target.position + offset;
        }
    }
}