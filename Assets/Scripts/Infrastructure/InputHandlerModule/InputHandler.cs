using System.Linq;
using Infrastructure.ServiceLocatorModule;
using Infrastructure.Services;
using Interfaces;
using UnityEngine;

namespace Infrastructure.InputHandlerModule
{
    public class InputHandler : MonoBehaviour
    {
        private Camera _mainCamera;
        private InputMouseButtonType _pressedMouseButton = InputMouseButtonType.None;

        private void Start()
        {
            _mainCamera = Camera.main;
        }

        private void Update()
        {
            if (ServiceLocator.Instance.GetService<PauseController>().IsPause)
                return;
            
            if (Input.GetMouseButtonDown(0) && _pressedMouseButton == InputMouseButtonType.None)
            {
                _pressedMouseButton = InputMouseButtonType.LeftMouseButton;
            }
            if (Input.GetMouseButtonDown(1) && _pressedMouseButton == InputMouseButtonType.None)
            {
                _pressedMouseButton = InputMouseButtonType.RightMouseButton;
            }

            if (Input.GetMouseButtonUp(0) && _pressedMouseButton == InputMouseButtonType.LeftMouseButton ||
                Input.GetMouseButtonUp(1) && _pressedMouseButton == InputMouseButtonType.RightMouseButton)
            {
                ThrowRay();
                _pressedMouseButton = InputMouseButtonType.None;
            }
        }

        private void ThrowRay()
        {
            Vector3 mousePosition = Input.mousePosition;
            Ray ray = _mainCamera.ScreenPointToRay(mousePosition);
            RaycastHit[] hits = Physics.RaycastAll(ray, 100f);
            hits = hits.OrderBy(h => h.distance).ToArray();
            
            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.TryGetComponent(typeof(IClickable), out var clickableObject))
                {
                    ((IClickable)clickableObject).Click(_pressedMouseButton);
                    break;
                }
            }
        }
    }
}