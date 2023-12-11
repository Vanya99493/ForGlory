using System;
using Infrastructure.InputHandlerModule;
using Interfaces;
using PlaygroundModule.PresenterPart;
using UnityEngine;

namespace PlaygroundModule.ViewPart
{
    public class CellView : MonoBehaviour, IClickable
    {
        [SerializeField] private Transform movePositionTransform;
        [SerializeField] private Material activeMaterial;
        [SerializeField] private Material activeRedMaterial;
        
        private Material _passiveMaterial;

        public event Action CellClicked;
        public event Action<int, int> MoveToCellAction;
        public event Action<CellView> Destroy;

        private CellPresenter _cellPresenter;

        public Vector3 MoveCellPosition => movePositionTransform.position;

        private void Start()
        {
            _passiveMaterial = GetComponent<MeshRenderer>().material;
        }

        public void Initialize(CellPresenter presenter)
        {
            _cellPresenter = presenter;
        }
        
        public void Click(InputMouseButtonType mouseButtonType)
        {
            if (mouseButtonType == InputMouseButtonType.RightMouseButton)
            {
                MoveToCellAction?.Invoke(_cellPresenter.Model.CellHeightId, _cellPresenter.Model.CellWidthId);
            }
            else if (mouseButtonType == InputMouseButtonType.LeftMouseButton)
            {
                CellClicked?.Invoke();
            }
        }

        public void ActivateCell()
        {
            gameObject.GetComponent<MeshRenderer>().material = activeMaterial;
        }

        public void ActivateRedCell()
        {
            gameObject.GetComponent<MeshRenderer>().material = activeRedMaterial;
        }

        public void DeactivateCell()
        {
            gameObject.GetComponent<MeshRenderer>().material = _passiveMaterial;
        }
    }
}