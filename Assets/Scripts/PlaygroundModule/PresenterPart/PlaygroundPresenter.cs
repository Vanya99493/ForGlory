using System.Collections.Generic;
using Infrastructure.Providers;
using PlaygroundModule.ModelPart;
using PlaygroundModule.ViewPart;
using UnityEngine;

namespace PlaygroundModule.PresenterPart
{
    public class PlaygroundPresenter
    {
        private readonly PlaygroundModel _model;
        private readonly PlaygroundView _view;

        private CellFactory _cellFactory;

        public PlaygroundPresenter(CellPrefabsProvider cellPrefabsProvider)
        {
            _view = new PlaygroundFactory().InstantiatePlayground();
            _model = new PlaygroundModel(_view);

            _cellFactory = new CellFactory(cellPrefabsProvider);
        }

        public void CreateAndSpawnPlayground(int height, int width, float playgroundSizeHeight, float playgroundSizeWidth)
        {
            var playground = CreatePlayground(height, width);
            //playground = SmoothOutPlayground(playground);
            _model.Initialize(playground);
            new PlaygroundSpawner().SpawnPlayground(_cellFactory, _model, _view.transform, playgroundSizeHeight, playgroundSizeWidth);
        }

        private Cell[,] CreatePlayground(int height, int width)
        {
            Cell[,] playground =
            {
                {new Cell(CellType.Hill), new Cell(CellType.Hill),new Cell(CellType.Plain),new Cell(CellType.Hill),new Cell(CellType.Hill),new Cell(CellType.Hill)},
                {new Cell(CellType.Hill), new Cell(CellType.RampBT),new Cell(CellType.Plain),new Cell(CellType.Plain),new Cell(CellType.Plain),new Cell(CellType.Hill)},
                {new Cell(CellType.Water), new Cell(CellType.Plain),new Cell(CellType.Plain),new Cell(CellType.Plain),new Cell(CellType.RampTB),new Cell(CellType.Hill)},
                {new Cell(CellType.Hill), new Cell(CellType.RampRL),new Cell(CellType.Plain),new Cell(CellType.Hill),new Cell(CellType.Hill),new Cell(CellType.Hill)},
                {new Cell(CellType.Hill), new Cell(CellType.Plain),new Cell(CellType.Plain),new Cell(CellType.Plain),new Cell(CellType.Hill),new Cell(CellType.Hill)},
                {new Cell(CellType.Hill), new Cell(CellType.Hill),new Cell(CellType.Plain),new Cell(CellType.Plain),new Cell(CellType.RampLR),new Cell(CellType.Hill)}
            };

            return playground;
        }
        
        private Cell[,] SmoothOutPlayground(Cell[,] playground)
        {
            Dictionary<CellType, double> cellTypes = new Dictionary<CellType, double>();
            
            for (int i = 0; i < playground.GetLength(0); i++)
            {
                for (int j = 0; j < playground.GetLength(1); j++)
                {
                    cellTypes.Clear();
                    if (i != 0)
                    {
                        if (playground[i, j].CellType == playground[i - 1, j].CellType)
                            continue; 
                        cellTypes.TryAdd(playground[i - 1, j].CellType, 1d);
                    }

                    if (i != playground.GetLength(0) - 1)
                    {
                        if (playground[i, j].CellType == playground[i + 1, j].CellType)
                            continue; 
                        cellTypes.TryAdd(playground[i + 1, j].CellType, 1d);
                    }

                    if (j != 0)
                    {
                        if (playground[i, j].CellType == playground[i, j - 1].CellType)
                            continue; 
                        cellTypes.TryAdd(playground[i, j - 1].CellType, 1d);
                    }

                    if (j != playground.GetLength(1) - 1)
                    {
                        if (playground[i, j].CellType == playground[i, j + 1].CellType)
                            continue; 
                        cellTypes.TryAdd(playground[i, j + 1].CellType, 1d);
                    }

                    playground[i, j] = new Cell(GetRandomCellType(cellTypes));
                }
            }

            return playground;
        }

        private CellType GetRandomCellType(Dictionary<CellType, double> cellTypes)
        {
            CellType selectedCellType = CellType.Plain;
            float globalChence = 0.0f;
            foreach (var cellType in cellTypes)
            {
                globalChence += (float)cellType.Value;
            }

            float targetValue = Random.Range(0f, globalChence);

            float currentTargetValue = 0.0f;
            foreach (var cellType in cellTypes)
            {
                currentTargetValue += (float)cellType.Value;
                if (targetValue <= currentTargetValue)
                {
                    selectedCellType = cellType.Key;
                    break;
                }
            }

            return selectedCellType;
        }
    }
}