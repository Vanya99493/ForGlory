using System;
using System.Collections.Generic;
using CharacterModule.ModelPart;
using Infrastructure.InputHandlerModule;
using PlaygroundModule.PresenterPart;
using UnityEngine;

namespace CharacterModule.PresenterPart
{
    public class CharacterPresenter
    {
        public event Action DestroyCharacter;
        public event Action ClickOnCharacterAction;
        
        private CharacterModel _model;

        public CharacterPresenter(CharacterModel model)
        {
            _model = model;
        }

        public void Enter<TState>()
        {
            
        }
        
        public void ClickOnCharacter(InputMouseButtonType mouseButtonType)
        {
            if (mouseButtonType == InputMouseButtonType.LeftMouseButton)
            {
                _model.SwitchMoveState();
                ClickOnCharacterAction?.Invoke();
            }
        }

        public (int, int) GetCharacterCellIndexes()
        {
            return (_model.HeightCellIndex, _model.WidthCellIndex);
        }

        public bool TryAddMoveRoute(PlaygroundPresenter playgroundPresenter, int heightIndex, int widthIndex)
        {
            bool canMove = _model.CanMove && !playgroundPresenter.CheckCellOnCharacter(heightIndex, widthIndex);

            // realize BFS 
            
            return canMove;
        }

        public void Move()
        {
            _model.Move(new List<Vector3>());
        }
        
        public void Destroy()
        {
            DestroyCharacter?.Invoke();
        }
    }
}