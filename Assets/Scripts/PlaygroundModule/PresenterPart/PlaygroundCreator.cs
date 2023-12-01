using System.Collections.Generic;
using PlaygroundModule.ModelPart;
using UnityEngine;

namespace PlaygroundModule.PresenterPart
{
    public class PlaygroundCreator
    {
        public CellPresenter[,] CreatePlayground(int height, int width)
        {
            CellPresenter[,] playground = new CellPresenter[height, width];
            
            float standartPlaneChanece = 0.5f;
            float standartHillChance = 0.5f;
            float planeChance;
            float hillChance;

            for (int i = 0; i < playground.GetLength(0); i++)
            {
                for (int j = 0; j < playground.GetLength(1); j++)
                {
                    planeChance = standartPlaneChanece;
                    hillChance = standartHillChance;
                    
                    if (i != 0)
                    {
                        if (playground[i - 1, j].Model.CellType == CellType.Hill)
                            hillChance *= 3f;
                        else
                            planeChance *= 3f;
                    }
                    if (j != 0)
                    {
                        if (playground[i, j - 1].Model.CellType == CellType.Hill)
                            hillChance *= 3f;
                        else
                            planeChance *= 3f;
                    }

                    float chance = Random.Range(0, hillChance + planeChance);
                    if (chance < hillChance)
                    {
                        playground[i, j] = new CellPresenter(new CellModel(CellType.Hill, i, j));
                    }
                    else
                    {
                        playground[i, j] = new CellPresenter(new CellModel(CellType.Plain, i, j));
                    }
                }
            }

            SmoothOutPlayground(playground);

            return playground;
        }

        public CellPresenter[,] CreateHardPlayground()
        {
            CellPresenter[,] playground =
            {
                {
                    new CellPresenter(new CellModel(CellType.Hill, 0, 0)), new CellPresenter(new CellModel(CellType.Hill, 0, 1)),
                    new CellPresenter(new CellModel(CellType.Plain, 0, 2)), new CellPresenter(new CellModel(CellType.Hill, 0, 3)),
                    new CellPresenter(new CellModel(CellType.Hill, 0, 4)), new CellPresenter(new CellModel(CellType.Hill, 0, 5))
                },
                {
                    new CellPresenter(new CellModel(CellType.Hill, 1, 0)), new CellPresenter(new CellModel(CellType.RampBT, 1, 1)),
                    new CellPresenter(new CellModel(CellType.Plain, 1, 2)), new CellPresenter(new CellModel(CellType.Plain, 1, 3)),
                    new CellPresenter(new CellModel(CellType.Plain, 1, 4)), new CellPresenter(new CellModel(CellType.Hill, 1, 5))
                },
                {
                    new CellPresenter(new CellModel(CellType.Water, 2, 0)), new CellPresenter(new CellModel(CellType.Plain, 2, 1)),
                    new CellPresenter(new CellModel(CellType.Plain, 2, 2)), new CellPresenter(new CellModel(CellType.Plain, 2, 3)),
                    new CellPresenter(new CellModel(CellType.RampTB, 2, 4)), new CellPresenter(new CellModel(CellType.Hill, 2, 5))
                },
                {
                    new CellPresenter(new CellModel(CellType.Hill, 3, 0)), new CellPresenter(new CellModel(CellType.RampRL, 3, 1)),
                    new CellPresenter(new CellModel(CellType.Plain, 3, 2)), new CellPresenter(new CellModel(CellType.Hill, 3, 3)),
                    new CellPresenter(new CellModel(CellType.Hill, 3, 4)), new CellPresenter(new CellModel(CellType.Hill, 3, 5))
                },
                {
                    new CellPresenter(new CellModel(CellType.Hill, 4, 0)), new CellPresenter(new CellModel(CellType.Plain, 4, 1)),
                    new CellPresenter(new CellModel(CellType.Plain, 4, 2)), new CellPresenter(new CellModel(CellType.Plain, 4, 3)),
                    new CellPresenter(new CellModel(CellType.Hill, 4, 4)), new CellPresenter(new CellModel(CellType.Hill, 4, 5))
                },
                {
                    new CellPresenter(new CellModel(CellType.Hill, 5, 0)), new CellPresenter(new CellModel(CellType.Hill, 5, 1)),
                    new CellPresenter(new CellModel(CellType.Plain, 5, 2)), new CellPresenter(new CellModel(CellType.Plain, 5, 3)),
                    new CellPresenter(new CellModel(CellType.RampLR, 5, 4)), new CellPresenter(new CellModel(CellType.Hill, 5, 5))
                }
            };

            return playground;
        }

        private void SmoothOutPlayground(CellPresenter[,] playground)
        {
            Dictionary<CellType, double> cellTypes = new Dictionary<CellType, double>();
            
            for (int i = 0; i < playground.GetLength(0); i++)
            {
                for (int j = 0; j < playground.GetLength(1); j++)
                {
                    cellTypes.Clear();
                    if (i != 0)
                    {
                        if (playground[i, j].Model.CellType == playground[i - 1, j].Model.CellType)
                            continue; 
                        cellTypes.TryAdd(playground[i - 1, j].Model.CellType, 1d);
                    }

                    if (i != playground.GetLength(0) - 1)
                    {
                        if (playground[i, j].Model.CellType == playground[i + 1, j].Model.CellType)
                            continue; 
                        cellTypes.TryAdd(playground[i + 1, j].Model.CellType, 1d);
                    }

                    if (j != 0)
                    {
                        if (playground[i, j].Model.CellType == playground[i, j - 1].Model.CellType)
                            continue; 
                        cellTypes.TryAdd(playground[i, j - 1].Model.CellType, 1d);
                    }

                    if (j != playground.GetLength(1) - 1)
                    {
                        if (playground[i, j].Model.CellType == playground[i, j + 1].Model.CellType)
                            continue; 
                        cellTypes.TryAdd(playground[i, j + 1].Model.CellType, 1d);
                    }

                    playground[i, j] = new CellPresenter(new CellModel(GetRandomCellType(cellTypes), i, j));
                }
            }
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