﻿using System;
using System.Collections.Generic;
using CharacterModule.PresenterPart;
using Infrastructure.Providers;
using PlaygroundModule.ModelPart;
using PlaygroundModule.PresenterPart.WideSearchModule;
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

        public void CreateAndSpawnPlayground(Transform parent, int height, int width, int lengthOfWater, int lengthOfCoast, 
            float playgroundSizeHeight, float playgroundSizeWidth, CellDataProvider cellDataProvider, 
            Action<int, int> OnMoveCellClicked, Action OnCellClicked, Action<List<TeamPresenter>> OnTeamsCollision)
        {
            var playground = new PlaygroundCreator().CreatePlaygroundByNewRootSpawnSystem(cellDataProvider, height, width, lengthOfWater, lengthOfCoast);
            Model.InitializePlayground(playground);
            new PlaygroundSpawner().SpawnPlayground(_cellFactory, Model, parent, playgroundSizeHeight, playgroundSizeWidth, OnMoveCellClicked, OnCellClicked);
            SubscribeOnTeamsCollision(OnTeamsCollision);
        }

        public bool SetCharacterOnCell(TeamPresenter team, int heightCellIndex, int widthCellIndex, bool isFirstInitialization = false)
        {
            return Model.SetCharacterOnCell(team, heightCellIndex, widthCellIndex, isFirstInitialization);
        }

        public bool CheckCellOnCharacter(int heightCellIndex, int widthCellIndex)
        {
            return Model.CheckCellOnCharacters(heightCellIndex, widthCellIndex);
        }

        public void RemoveCharacterFromCell(TeamPresenter team, int heightCellIndex, int widthCellIndex)
        {
            // need to add remove character from cell after his destroying in battle
            Model.RemoveCharacterFromCell(team, heightCellIndex, widthCellIndex);
        }

        public void SetAciveCells(List<Node> cells)
        {
            Model.ActiveCells = cells;
        }
        
        public void ActivateCells()
        {
            foreach (Node cell in Model.ActiveCells)
            {
                Model.GetCellPresenter(cell.HeightIndex, cell.WidthIndex).ActivateCell();
            }
        }

        public void ActivateRedCells()
        {
            foreach (Node cell in Model.ActiveCells)
            {
                Model.GetCellPresenter(cell.HeightIndex, cell.WidthIndex).ActivateRedCell();
            }
        }

        public void DeactivateCells()
        {
            foreach (Node cell in Model.ActiveCells)
            {
                Model.GetCellPresenter(cell.HeightIndex, cell.WidthIndex).DeactivateCell();
            }
        }

        public void ResetActiveCells()
        {
            Model.ActiveCells.Clear();
        }

        private void SubscribeOnTeamsCollision(Action<List<TeamPresenter>> OnTeamsCollision)
        {
            for (int i = 0; i < Model.PlaygroundHeight; i++)
            {
                for (int j = 0; j < Model.PlaygroundWidth; j++)
                {
                    Model.GetCellPresenter(i, j).Model.TeamsCollisionAction += OnTeamsCollision;
                }
            }
        }
    }
}