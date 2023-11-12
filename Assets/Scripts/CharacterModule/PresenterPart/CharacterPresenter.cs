using System;
using System.Collections.Generic;
using CharacterModule.ModelPart;
using CustomClasses;
using Infrastructure.InputHandlerModule;
using UnityEngine;

namespace CharacterModule.PresenterPart
{
    public class CharacterPresenter
    {
        public event Action DestroyCharacter;
        public event Action ClickOnCharacterAction;
        
        public readonly CharacterModel Model;

        public CharacterPresenter(CharacterModel model)
        {
            Model = model;
        }

        public void Enter<TState>()
        {
            
        }
        
        public void ClickOnCharacter(InputMouseButtonType mouseButtonType)
        {
            if (mouseButtonType == InputMouseButtonType.LeftMouseButton)
            {
                Model.SwitchMoveState();
                ClickOnCharacterAction?.Invoke();
            }
        }

        public void AddRoute(List<Pair<int, int>> route)
        {
            Model.AddRoute(route);
        }

        public void Move()
        {
            Model.Move();
        }
        
        public void Destroy()
        {
            DestroyCharacter?.Invoke();
        }
    }
}