using UnityEngine;

namespace CameraModule
{
    public class CameraFolower : MonoBehaviour
    {
        [SerializeField] private Vector3 offset;

        private Transform _target;
        private bool _haveTarget = false;

        public void SetTarget(Transform target)
        {
            _target = target;
            _haveTarget = true;
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