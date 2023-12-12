using System;
using System.Collections.Generic;
using CharacterModule.PresenterPart.CharacterStates.Base;
using CustomClasses;
using PlaygroundModule.PresenterPart;

namespace CharacterModule.PresenterPart.CharacterStates
{
    public class MoveCharacterState : ICharacterMoveState
    {
        public event Action StateEndedAction;
        public event Action<List<Pair<int, int>>> AddRouteAction; 

        public void Enter(PlaygroundPresenter playgroundPresenter, int heightCoordinate, int widthCoordinate)
        {
            // AddRouteAction?.Invoke();
            
            StateEndedAction?.Invoke();
        }
        
        public void Exit()
        {
            
        }
    }
}