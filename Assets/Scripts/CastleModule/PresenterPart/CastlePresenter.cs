using System;
using CastleModule.ModelPart;
using CastleModule.ViewPart;
using PlaygroundModule.PresenterPart;

namespace CastleModule.PresenterPart
{
    public class CastlePresenter
    {
        public event Action TurnOnEvent;
        public event Action TurnOffEvent;
        
        public readonly CastleModel Model;
        
        private CastleView _view;

        public CastlePresenter(CastleModel castleModel, CastleView castleView)
        {
            Model = castleModel;
            _view = castleView;
        }
        
        public void SetPosition(PlaygroundPresenter playgroundPresenter)
        {
            _view.transform.position = playgroundPresenter.Model.GetCellPresenter(Model.HeightCellIndex, Model.WidthCellIndex).Model.MoveCellPosition;
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
            _view.DestroyView();
        }
    }
}