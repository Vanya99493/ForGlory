using System;
using System.Collections.Generic;
using CharacterModule.ModelPart;
using CharacterModule.ViewPart;
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
        public readonly CharacterView View;

        public CharacterPresenter(CharacterModel model, CharacterView view)
        {
            Model = model;
            View = view;
            Model.MoveAction += View.Move;
            DestroyCharacter += View.OnDestroyCharacter;
            View.ClickOnCharacter += ClickOnCharacter;
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