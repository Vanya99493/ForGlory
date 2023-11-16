using System;
using System.Collections.Generic;
using CharacterModule.PresenterPart;
using CustomClasses;
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

        public PlaygroundPresenter(PlaygroundModel model, CellPrefabsProvider cellPrefabsProvider)
        {
            Model = model;
            _cellFactory = new CellFactory(cellPrefabsProvider);
        }

        public void Destroy()
        {
            DestroyPlayground?.Invoke();
        }

        public void CreateAndSpawnPlayground(Transform parent, float playgroundSizeHeight, float playgroundSizeWidth, Action<int, int> OnCellClicked)
        {
            var playground = new PlaygroundCreator().CreatePlayground(Model);
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