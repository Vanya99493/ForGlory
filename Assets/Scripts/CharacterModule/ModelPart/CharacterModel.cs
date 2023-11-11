using System.Collections.Generic;
using CharacterModule.ViewPart;
using UnityEngine;

namespace CharacterModule.ModelPart
{
    public class CharacterModel
    {
        private CharacterView _view;
        private Vector3 _currentPosition;
        private float _speed = 1f;

        public bool CanMove { get; private set; }
        public int HeightCellIndex { get; private set; }
        public int WidthCellIndex { get; private set; }

        public CharacterModel(CharacterView view, Vector3 currentPosition, int heightCellIndex, int widthCellIndex)
        {
            _view = view;
            SetPosition(currentPosition, heightCellIndex, widthCellIndex);
            CanMove = false;
        }

        public void SwitchMoveState()
        {
            CanMove = !CanMove;
            Debug.Log(CanMove);
        }

        public void Move(List<Vector3> route)
        {
            foreach (Vector3 checkPoint in route)
            {
                
            }
        }

        private void SetPosition(Vector3 position, int heightCellIndex, int widthCellIndex)
        {
            _currentPosition = position;
            HeightCellIndex = heightCellIndex;
            WidthCellIndex = widthCellIndex;
            _view.Move(position);
        }
    }
}