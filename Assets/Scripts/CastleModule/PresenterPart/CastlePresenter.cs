using System;
using CastleModule.ModelPart;
using CastleModule.ViewPart;
using CharacterModule.PresenterPart;
using PlaygroundModule.PresenterPart;

namespace CastleModule.PresenterPart
{
    public class CastlePresenter
    {
        public event Action TurnOnEvent;
        public event Action TurnOffEvent;
        
        public readonly CastleView View;
        
        private readonly CastleModel _model;

        public int CastleHeightIndex => _model.HeightCellIndex;
        public int CastleWidthIndex => _model.WidthCellIndex;

        public CastlePresenter(CastleModel castleModel, CastleView castleView)
        {
            _model = castleModel;
            View = castleView;
        }
        
        public void SetPosition(PlaygroundPresenter playgroundPresenter)
        {
            View.transform.position = playgroundPresenter.Model.GetCellPresenter(_model.HeightCellIndex, _model.WidthCellIndex).Model.MoveCellPosition;
        }
        
        public void SetCharactersInCastle(PlayerCharacterPresenter[] characters)
        {
            _model.SetCharactersInCastle(characters, View.transform);
        }

        public CharacterPresenter[] GetCharactersInCastle() => _model.CharactersInCastle;

        public void ResetHeroesEnergy()
        {
            _model.ResetHeroesEnergy();
        }

        public void ActivateEvent()
        {
            TurnOnEvent?.Invoke();
        }

        public void DeactivateEvent()
        {
            TurnOffEvent?.Invoke();
        }

        public void Destroy()
        {
            View.DestroyView();
        }
    }
}