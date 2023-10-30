using System;
using Infrastructure.Providers;
using PlaygroundModule.ModelPart;
using UnityEngine;

namespace PlaygroundModule.PresenterPart
{
    public class PlaygroundPresenter
    {
        public event Action DestroyPlayground; 

        private readonly PlaygroundModel _model;

        private CellFactory _cellFactory;

        public PlaygroundPresenter(PlaygroundModel model, CellPrefabsProvider cellPrefabsProvider)
        {
            _model = model;
            _cellFactory = new CellFactory(cellPrefabsProvider);
        }

        public void Destroy()
        {
            DestroyPlayground?.Invoke();
        }

        public void CreateAndSpawnPlayground(Transform parent, float playgroundSizeHeight, float playgroundSizeWidth)
        {
            var playground = new PlaygroundCreator().CreatePlayground(_model);
            _model.InitializePlayground(playground);
            new PlaygroundSpawner().SpawnPlayground(_cellFactory, _model, parent, playgroundSizeHeight, playgroundSizeWidth);
        }
    }
}