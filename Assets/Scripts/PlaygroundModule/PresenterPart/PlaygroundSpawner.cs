using System;
using PlaygroundModule.ModelPart;
using PlaygroundModule.ViewPart;
using UnityEngine;

namespace PlaygroundModule.PresenterPart
{
    public class PlaygroundSpawner
    {
        public void SpawnPlayground(CellFactory cellFactory, PlaygroundModel model, Transform parent, 
            float playgroundSizeHeight, float playgroundSizeWidth,
            Action<int, int> OnCellClicked)
        {
            float cellSizeHeight = playgroundSizeHeight / model.Height;
            float cellSizeWidth = playgroundSizeWidth / model.Width;
            
            float xStartSpawnPosition = -1 * (model.Width / 2.0f - cellSizeWidth / 2.0f);
            float zStartSpawnPosition = model.Height / 2.0f - cellSizeHeight / 2.0f;

            for (int i = 0; i < model.Height; i++)
            {
                for (int j = 0; j < model.Width; j++)
                {
                    CellPresenter cellPresenter = model.GetCellPresenter(i, j);
                    Vector3 cellSpawnPosition = new Vector3(
                        xStartSpawnPosition + cellSizeWidth * j,
                        0f,
                        zStartSpawnPosition - cellSizeHeight * i
                    );
                    CellView cellView = cellFactory.InstantiateCell(cellPresenter.Model.CellType, parent, cellSpawnPosition);
                    cellPresenter.InitializeView(cellView);
                    cellView.CellClicked += OnCellClicked;
                    cellView.Initialize(cellPresenter);
                    cellPresenter.Model.IntitializeView(cellView);
                }
            }
        }
    }
}