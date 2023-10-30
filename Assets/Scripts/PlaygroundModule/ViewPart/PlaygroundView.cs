using System;
using PlaygroundModule.PresenterPart;
using UnityEngine;

namespace PlaygroundModule.ViewPart
{
    [Serializable]
    public class PlaygroundView : MonoBehaviour
    {
        public event Action<PlaygroundView> Destroy;
        
        private PlaygroundPresenter _presenter;

        public void Initialize(PlaygroundPresenter playgroundPresenter)
        {
            _presenter = playgroundPresenter;
            _presenter.DestroyPlayground += OnDestroyPlayground;
        }

        private void OnDestroyPlayground()
        {
            Destroy?.Invoke(this);
        }
    }
}