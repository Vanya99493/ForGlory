using System;
using System.Collections.Generic;
using CharacterModule.ModelPart;
using CharacterModule.PresenterPart.CharacterStates;
using CharacterModule.PresenterPart.CharacterStates.Base;
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
        public abstract event Action<TeamPresenter> ClickOnCharacterAction;
        
        public readonly TeamModel Model;
        public readonly TeamView View;

        private Dictionary<Type, ICharacterState> _characterStates;

        protected TeamPresenter(TeamModel model, TeamView view)
        {
            Model = model;
            View = view;
            Model.MoveAction += View.Move;
            Model.EndMoveAction += OnEndMoveAction;
            DestroyCharacter += View.OnDestroyCharacter;
            View.ClickOnCharacter += ClickOnTeam;

            _characterStates = new Dictionary<Type, ICharacterState>()
            {
                { typeof(IdleCharacterState), new IdleCharacterState() },
                { typeof(MoveCharacterState), new MoveCharacterState() },
                { typeof(FollowCharacterState), new FollowCharacterState() }
            };
        }

        public void EnterIdleState()
        {
            
        }

        public void EnterMoveState(PlaygroundPresenter playgroundPresenter, int heightCoordinate, int widthCoordinate)
        {
            
        }

        public void EnterFollowState(PlaygroundPresenter playgroundPresenter, CharacterPresenter target)
        {
            
        }

        public abstract void ClickOnTeam(InputMouseButtonType mouseButtonType);
        
        public void Destroy()
        {
            DestroyCharacter?.Invoke();
        }

        private void SubscribeStateMachineActions()
        {
            
        }

        public void AddRoute(List<Pair<int, int>> route)
        {
            Model.AddRoute(route);
        }

        public void Move(ICoroutineRunner coroutineRunner, PlaygroundPresenter playgroundPresenter)
        {
            Model.Move(coroutineRunner, playgroundPresenter, this as PlayerTeamPresenter);
        }

        private void OnEndMoveAction(PlaygroundPresenter playgroundPresenter)
        {
            playgroundPresenter.SetCharacterOnCell(this, Model.HeightCellIndex, Model.WidthCellIndex);
        }
    }
}