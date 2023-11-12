using System.Collections.Generic;
using System.Threading;
using CharacterModule.ViewPart;
using CustomClasses;
using UnityEngine;

namespace CharacterModule.ModelPart
{
    public class CharacterModel
    {
        private CharacterView _view;
        private float _speed = 1f;
        private List<Pair<int, int>> _route;

        public bool CanMove { get; private set; }
        public int HeightCellIndex { get; private set; }
        public int WidthCellIndex { get; private set; }

        public CharacterModel(CharacterView view, int heightCellIndex, int widthCellIndex)
        {
            _view = view;
            SetPosition(heightCellIndex, widthCellIndex);
            CanMove = false;
        }

        public void SwitchMoveState()
        {
            CanMove = !CanMove;
            Debug.Log(CanMove);
        }

        public void AddRoute(List<Pair<int, int>> route)
        {
            foreach (var checkPoint in route)
            {
                _route.Add(new Pair<int, int>(checkPoint.FirstValue, checkPoint.SecondValue));
            }
        }

        public void Move()
        {
            foreach (Pair<int, int> checkPoint in _route)
            {
                SetPosition(checkPoint.FirstValue, checkPoint.SecondValue);
                Thread.Sleep(1000);
            }
        }

        private void SetPosition(int heightCellIndex, int widthCellIndex)
        {
            HeightCellIndex = heightCellIndex;
            WidthCellIndex = widthCellIndex;
            _view.Move(HeightCellIndex, WidthCellIndex);
        }
    }
}