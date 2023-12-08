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
    public abstract class TeamPresenter
    {
        public event Action DestroyCharacter;
        public abstract event Action<bool, int> ClickOnCharacterAction;
        
        public readonly TeamModel Model;
        public readonly TeamView View;

        protected TeamPresenter(TeamModel model, TeamView view)
        {
            Model = model;
            View = view;
            Model.MoveAction += View.Move;
            DestroyCharacter += View.OnDestroyCharacter;
            View.ClickOnCharacter += ClickOnTeam;
        }

        public void Enter<TState>()
        {
            
        }

        public abstract void ClickOnTeam(InputMouseButtonType mouseButtonType);

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