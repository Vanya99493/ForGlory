using UnityEngine;

namespace CameraModule
{
    public class CameraFollower : MonoBehaviour
    {
        [SerializeField] private Vector3 offset;
        [Header("Rotation parameters")]
        [SerializeField] private float rotationSpeed;

        private Quaternion _lastRotation;
        private GameObject _rotationParent;
        private Transform _lastParent;
        private bool _isRotating = false;
        
        private Transform _target;
        private bool _isLocked = false;

        private void Awake()
        {
            _rotationParent = new GameObject("CameraRotationParent");
        }

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

        public void ActivateRotation()
        {
            _lastParent = transform.parent;
            _lastRotation = _rotationParent.transform.rotation;
            transform.SetParent(_rotationParent.transform);
            _isRotating = true;
        }

        public void DeactivateRotation()
        {
            _isRotating = false;
            _rotationParent.transform.rotation = _lastRotation;
            transform.SetParent(_lastParent);
        }
        
        private void LateUpdate()
        {
            if (_isRotating)
            {
                _rotationParent.transform.Rotate(new Vector3(0, rotationSpeed * Time.fixedDeltaTime, 0));
            }
            
            /*if (_target == null)
            {
                return;
            }*/
            
            //transform.position = (_isLocked ? new Vector3(0, 0, 0) : _target.position) + offset;
            //transform.position = (!_isLocked && _target != null ? _target.position : new Vector3(0, 0, 0)) + offset;
            transform.position = _target != null ? (!_isLocked ? _target.position :  Vector3.zero) + offset : new Vector3(0f, offset.y, 0f);
        }
    }
}