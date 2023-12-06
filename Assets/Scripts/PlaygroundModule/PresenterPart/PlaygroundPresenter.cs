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

        public readonly PlaygroundModel Model;

        private CellFactory _cellFactory;

        public PlaygroundPresenter(PlaygroundModel model, CellDataProvider cellDataProvider)
        {
            Model = model;
            _cellFactory = new CellFactory(cellDataProvider);
        }

        public void Destroy()
        {
            DestroyPlayground?.Invoke();
        }

        public void CreateAndSpawnPlayground(Transform parent, int height, int width, int lengthOfWater, int lengthOfCoast, float playgroundSizeHeight, float playgroundSizeWidth, CellDataProvider cellDataProvider, Action<int, int> OnCellClicked)
        {
            //var playground = new PlaygroundCreator().CreatePlayground(height, width);
            //var playground = new PlaygroundCreator().CreatePlaygroundByBundles(height, width);
            var playground = new PlaygroundCreator().CreatePlaygroundByNewRootSpawnSystem(cellDataProvider, height, width, lengthOfWater, lengthOfCoast);
            Model.InitializePlayground(playground);
            new PlaygroundSpawner().SpawnPlayground(_cellFactory, Model, parent, playgroundSizeHeight, playgroundSizeWidth, OnCellClicked);
        }

        public bool SetCharacterOnCell(CharacterPresenter character, int heightCellIndex, int widthCellIndex)
        {
            return Model.SetCharacterOnCell(character, heightCellIndex, widthCellIndex);
        }

        public bool CheckCellOnCharacter(int heightCellIndex, int widthCellIndex)
        {
            return Model.CheckCellOnCharacter(heightCellIndex, widthCellIndex);
        }

        public void RemoveCharacterFromCell(int heightCellIndex, int widthCellIndex)
        {
            Model.RemoveCharacterFromCell(heightCellIndex, widthCellIndex);
        }
    }
}