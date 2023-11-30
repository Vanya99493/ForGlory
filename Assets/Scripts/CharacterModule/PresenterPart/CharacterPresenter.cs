using System;
using System.Collections.Generic;
using CharacterModule.ModelPart;
using CustomClasses;
using Infrastructure.CoroutineRunnerModule;
using Infrastructure.InputHandlerModule;
using PlaygroundModule.PresenterPart;

namespace CharacterModule.PresenterPart
{
    public class CharacterPresenter
    {
        public event Action DestroyCharacter;
        public event Action<bool, int> ClickOnCharacterAction;
        
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
            if (mouseButtonType == InputMouseButtonType.LeftMouseButton && Model.CanMove)
            {
                Model.SwitchMoveState();
                ClickOnCharacterAction?.Invoke(Model.MoveState, Model.Energy);
            }
        }

        public void AddRoute(List<Pair<int, int>> route)
        {
            Model.AddRoute(route);
        }

        public void Move(ICoroutineRunner coroutineRunner, PlaygroundPresenter playgroundPresenter)
        {
            Model.Move(coroutineRunner, playgroundPresenter);
            playgroundPresenter.SetCharacterOnCell(this, Model.HeightCellIndex, Model.WidthCellIndex);
        }
        
        public void Destroy()
        {
            DestroyCharacter?.Invoke();
        }
    }
}