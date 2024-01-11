using System;
using System.Collections.Generic;
using CharacterModule.ModelPart;
using CharacterModule.PresenterPart.BehaviourModule.Base;
using CharacterModule.PresenterPart.CharacterStates;
using CharacterModule.PresenterPart.CharacterStates.Base;
using CharacterModule.ViewPart;
using CustomClasses;
using Infrastructure.InputHandlerModule;
using PlaygroundModule.PresenterPart;

namespace CharacterModule.PresenterPart
{
    public abstract class TeamPresenter
    {
        public event Action StateEnded;
        public event Action StepEnded;
        public event Action DestroyCharacter;
        public abstract event Action<TeamPresenter> ClickOnCharacterAction;
        
        public readonly TeamModel Model;
        public readonly TeamView View;

        private IBehaviour _characterBehaviour;
        private Dictionary<Type, ICharacterState> _characterStates;
        private ICharacterState _currentState;

        protected TeamPresenter(TeamModel model, TeamView view, IBehaviour characterBehaviour)
        {
            Model = model;
            View = view;
            _characterBehaviour = characterBehaviour;
            
            Model.MoveAction += View.Move;
            //Model.EndMoveAction += OnEndMoveAction;
            DestroyCharacter += View.OnDestroyCharacter;
            View.ClickOnCharacter += ClickOnTeam;

            _characterStates = new Dictionary<Type, ICharacterState>()
            {
                { typeof(IdleCharacterState), new IdleCharacterState() },
                { typeof(MoveCharacterState), new MoveCharacterState() },
                { typeof(FollowCharacterState), new FollowCharacterState() }
            };
            
            SubscribeStateMachineActions();
        }

        public void StartBehave(PlaygroundPresenter playgroundPresenter)
        {
            _characterBehaviour.Start(this, playgroundPresenter);
        }

        public bool UpdateCurrentState(PlaygroundPresenter playgroundPresenter)
        {
            if (_currentState != null)
            {
                if (_currentState.Update(this, playgroundPresenter))
                {
                    StateEnded?.Invoke();
                    return true;
                }
            }
            return false;
        }

        public void EnterIdleState(PlaygroundPresenter playgroundPresenter)
        {
            _currentState?.Exit();
            _currentState = _characterStates[typeof(IdleCharacterState)];
            ((IdleCharacterState)_currentState).Enter(playgroundPresenter);
        }

        public void EnterMoveState(PlaygroundPresenter playgroundPresenter, int heightCoordinate, int widthCoordinate)
        {
            _currentState?.Exit();
            _currentState = _characterStates[typeof(MoveCharacterState)];
            ((MoveCharacterState)_currentState).Enter(this, playgroundPresenter, heightCoordinate, widthCoordinate);
        }

        public void EnterFollowState(PlaygroundPresenter playgroundPresenter, TeamPresenter target)
        {
            _currentState?.Exit();
            _currentState = _characterStates[typeof(FollowCharacterState)];
            ((FollowCharacterState)_currentState).Enter(this, playgroundPresenter, target);
        }

        public abstract void ClickOnTeam(InputMouseButtonType mouseButtonType);
        
        public void Destroy()
        {
            DestroyCharacter?.Invoke();
        }

        private void SubscribeStateMachineActions()
        {
            foreach (var characterState in _characterStates)
            {
                characterState.Value.StepEndedAction += OnStepEnded;
            }
            
            ((IdleCharacterState)_characterStates[typeof(IdleCharacterState)]).MoveAction += OnMove;
            ((MoveCharacterState)_characterStates[typeof(MoveCharacterState)]).AddRouteAction += OnAddRoute;
            ((MoveCharacterState)_characterStates[typeof(MoveCharacterState)]).MoveAction += OnMove;
            ((FollowCharacterState)_characterStates[typeof(FollowCharacterState)]).AddRouteAction += OnAddRoute;
            ((FollowCharacterState)_characterStates[typeof(FollowCharacterState)]).MoveAction += OnMove;
        }

        private void OnStepEnded()
        {
            StepEnded?.Invoke();
        }

        private void OnAddRoute(List<Pair<int, int>> route)
        {
            Model.AddRoute(route);
        }

        private void OnMove(PlaygroundPresenter playgroundPresenter)
        {
            Model.Move(View, playgroundPresenter, this);
        }

        private void OnEndMoveAction(PlaygroundPresenter playgroundPresenter)
        {
            playgroundPresenter.SetCharacterOnCell(this, Model.HeightCellIndex, Model.WidthCellIndex);
        }
    }
}