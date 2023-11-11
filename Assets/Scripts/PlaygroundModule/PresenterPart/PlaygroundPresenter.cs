using System;
using CharacterModule.PresenterPart;
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

        public void CreateAndSpawnPlayground(Transform parent, float playgroundSizeHeight, float playgroundSizeWidth, Action<int, int> OnCellClicked)
        {
            var playground = new PlaygroundCreator().CreatePlayground(_model);
            _model.InitializePlayground(playground);
            new PlaygroundSpawner().SpawnPlayground(_cellFactory, _model, parent, playgroundSizeHeight, playgroundSizeWidth, OnCellClicked);
        }

        public bool SetCharacterOnCell(CharacterPresenter character, int heightCellIndex, int widthCellIndex)
        {
            return _model.SetCharacterOnCell(character, heightCellIndex, widthCellIndex);
        }

        public bool CheckCellOnCharacter(int heightCellIndex, int widthCellIndex)
        {
            return _model.CheckCellOnCharacter(heightCellIndex, widthCellIndex);
        }

        public void RemoveCharacterFromCell(int heightCellIndex, int widthCellIndex)
        {
            _model.RemoveCharacterFromCell(heightCellIndex, widthCellIndex);
        }
    }
}