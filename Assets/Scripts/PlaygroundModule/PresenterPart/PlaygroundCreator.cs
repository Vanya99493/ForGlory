using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.Providers;
using PlaygroundModule.ModelPart;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PlaygroundModule.PresenterPart
{
    public class PlaygroundCreator
    {
        class Node
        {
            public int HeightIndex;
            public int WidthIndex;
            public CellType CellType;
            public bool HasType;
            public bool Visited;

            public Node(int heightIndex, int widthIndex)
            {
                HeightIndex = heightIndex;
                WidthIndex = widthIndex;
                HasType = false;
                Visited = false;
            }
        }
        
        public CellPresenter[,] CreatePlaygroundByNewRootSpawnSystem(CellDataProvider cellDataProvider, int height, int width, int lengthOfWater, int lengthOfCoast)
        {
            Node[,] nodes = new Node[height, width];

            for (int i = 0; i < nodes.GetLength(0); i++)
            {
                for (int j = 0; j < nodes.GetLength(1); j++)
                {
                    nodes[i, j] = new Node(i, j);
                }
            }

            if (height - (lengthOfWater + lengthOfCoast) <= 0 || width - (lengthOfWater + lengthOfCoast) <= 0)
                throw new Exception("Height or width too small");

            FillWater(nodes, lengthOfWater);
            FillCoast(nodes, lengthOfCoast, lengthOfWater);
            
            int heightStartIndex = Random.Range(lengthOfWater + lengthOfCoast, height - (lengthOfWater + lengthOfCoast));
            int widthStartIndex = Random.Range(lengthOfWater + lengthOfCoast, width - (lengthOfWater + lengthOfCoast));

            Queue<Node> nodesQueue = new Queue<Node>();
            nodesQueue.Enqueue(nodes[heightStartIndex, widthStartIndex]);
            SetCell(nodes, heightStartIndex, widthStartIndex, CellType.Plain);

            while (nodesQueue.Count > 0)
            {
                Node pickedNode = nodesQueue.Dequeue();

                if (pickedNode.HeightIndex > 0 && 
                    !nodes[pickedNode.HeightIndex - 1, pickedNode.WidthIndex].Visited)
                {
                    SetNewCell(nodes, pickedNode.HeightIndex - 1, pickedNode.WidthIndex, cellDataProvider);
                    nodesQueue.Enqueue(nodes[pickedNode.HeightIndex - 1, pickedNode.WidthIndex]);
                }
                if (pickedNode.HeightIndex < nodes.GetLength(0) - 1 && 
                    !nodes[pickedNode.HeightIndex + 1, pickedNode.WidthIndex].Visited)
                {
                    SetNewCell(nodes, pickedNode.HeightIndex + 1, pickedNode.WidthIndex, cellDataProvider);
                    nodesQueue.Enqueue(nodes[pickedNode.HeightIndex + 1, pickedNode.WidthIndex]);
                }
                if (pickedNode.WidthIndex > 0 && 
                    !nodes[pickedNode.HeightIndex, pickedNode.WidthIndex - 1].Visited)
                {
                    SetNewCell(nodes, pickedNode.HeightIndex, pickedNode.WidthIndex - 1, cellDataProvider);
                    nodesQueue.Enqueue(nodes[pickedNode.HeightIndex, pickedNode.WidthIndex - 1]);
                }
                if (pickedNode.WidthIndex < nodes.GetLength(1) - 1 && 
                    !nodes[pickedNode.HeightIndex, pickedNode.WidthIndex + 1].Visited)
                {
                    SetNewCell(nodes, pickedNode.HeightIndex, pickedNode.WidthIndex + 1, cellDataProvider);
                    nodesQueue.Enqueue(nodes[pickedNode.HeightIndex, pickedNode.WidthIndex + 1]);
                }
            }

            DeleteRamps(nodes, cellDataProvider);
            
            CellPresenter[,] playground = new CellPresenter[height, width];
            
            for (int i = 0; i < playground.GetLength(0); i++)
            {
                for (int j = 0; j < playground.GetLength(1); j++)
                {
                    playground[i, j] = new CellPresenter(new CellModel(nodes[i, j].CellType, nodes[i, j].HeightIndex, nodes[i, j].WidthIndex));
                }
            }

            int smoothedCells;
            do
            {
                smoothedCells = SmoothOutPlayground(playground);
            } while (smoothedCells > 0);

            return playground;
        }

        private void FillWater(Node[,] nodes, int lengthOfWater)
        {
            for (int masterIterator = 0; masterIterator < lengthOfWater; masterIterator++)
            {
                for (int topRow = 0, bottomRow = nodes.GetLength(0) - 1, stepIterator = 0;
                     stepIterator < nodes.GetLength(1);
                     stepIterator++)
                {
                    SetWater(nodes, topRow + masterIterator, stepIterator);
                    SetWater(nodes, bottomRow - masterIterator, stepIterator);
                }

                for (int leftColumn = 0, rightColumn = nodes.GetLength(1) - 1, stepIterator = lengthOfWater;
                     stepIterator < nodes.GetLength(0) - lengthOfWater;
                     stepIterator++)
                {
                    SetWater(nodes, stepIterator, leftColumn + masterIterator);
                    SetWater(nodes, stepIterator, rightColumn - masterIterator);
                }
            }
        }

        private void FillCoast(Node[,] nodes, int lengthOfCoast, int lengthOfWater)
        {
            int coastStartPoint = (int)Math.Round(lengthOfCoast/2f);
            int coastCurrentPoint = coastStartPoint;

            // top
            for (int j = lengthOfWater; j < lengthOfCoast + lengthOfWater; j++)
            {
                SetWater(nodes, j, lengthOfWater);
            }
            for (int i = lengthOfWater + 1; i < nodes.GetLength(1) - lengthOfWater - 1; i++)
            {
                for (int j = lengthOfWater; j < lengthOfCoast + lengthOfWater - coastCurrentPoint; j++)
                {
                    SetWater(nodes, j, i);
                }
                
                coastCurrentPoint = CalculateCoastCurrentPoint(lengthOfCoast, coastCurrentPoint);
            }
            for (int j = lengthOfWater; j < lengthOfCoast + lengthOfWater; j++)
            {
                SetWater(nodes, j, nodes.GetLength(1) - lengthOfWater - 1);
            }

            // left s right
            coastCurrentPoint = coastStartPoint;
            int coastCurrentPoint2 = coastStartPoint;
            
            for (int i = lengthOfWater + lengthOfCoast; i < nodes.GetLength(0) - (lengthOfWater + lengthOfCoast); i++)
            {
                for (int j = lengthOfWater; j < lengthOfCoast + lengthOfWater - coastCurrentPoint; j++)
                {
                    SetWater(nodes, i, j);
                }
                
                coastCurrentPoint = CalculateCoastCurrentPoint(lengthOfCoast, coastCurrentPoint);

                for (int j = nodes.GetLength(1) - lengthOfWater; j > nodes.GetLength(1) - (lengthOfWater + lengthOfCoast) + coastCurrentPoint2; j--)
                {
                    SetWater(nodes, i, j);
                }

                coastCurrentPoint2 = CalculateCoastCurrentPoint(lengthOfCoast, coastCurrentPoint2);
            }
            
            // bottom
            coastCurrentPoint = coastStartPoint;
            
            for (int j = nodes.GetLength(0) - lengthOfWater - 1; j > nodes.GetLength(0) - (lengthOfCoast + lengthOfWater) - 1; j--)
            {
                SetWater(nodes, j, lengthOfWater);
            }
            for (int i = lengthOfWater + 1; i < nodes.GetLength(1) - lengthOfWater - 1; i++)
            {
                for (int j = nodes.GetLength(1) - lengthOfWater - 1; j > nodes.GetLength(1) - lengthOfWater - lengthOfCoast - 1 + coastCurrentPoint; j--)
                {
                    SetWater(nodes, j, i);
                }

                coastCurrentPoint = CalculateCoastCurrentPoint(lengthOfCoast, coastCurrentPoint);
            }
            for (int j = nodes.GetLength(0) - lengthOfWater - 1; j > nodes.GetLength(0) - (lengthOfCoast + lengthOfWater) - 1; j--)
            {
                SetWater(nodes, j, nodes.GetLength(1) - lengthOfWater - 1);
            }
        }

        private void SetWater(Node[,] nodes, int heightIndex, int widthIndex)
        {
            SetCell(nodes, heightIndex, widthIndex, CellType.Water);
        }

        private void SetNewCell(Node[,] nodes, int heightIndex, int widthIndex, CellDataProvider cellDataProvider)
        {
            SetCell(nodes, heightIndex, widthIndex, 
                ChooseCellType(cellDataProvider, heightIndex, widthIndex, nodes)
                );
        }

        private void SetCell(Node[,] nodes, int heightIndex, int widthIndex, CellType cellType)
        {
            nodes[heightIndex, widthIndex].Visited = true;
            nodes[heightIndex, widthIndex].CellType = cellType;
            nodes[heightIndex, widthIndex].HasType = true;
        }

        private int CalculateCoastCurrentPoint(int lengthOfCoast, int coastCurrentPoint)
        {
            return Random.Range(0, 2) == 0
                ? coastCurrentPoint + 1 > lengthOfCoast ? coastCurrentPoint : ++coastCurrentPoint
                : coastCurrentPoint - 1 < 0 ? coastCurrentPoint : --coastCurrentPoint;
        }

        private CellType ChooseCellType(CellDataProvider cellDataProvider, int heightIndex, int widthIndex, Node[,] nodes)
        {
            Dictionary<CellType, double> possibleCellTypes = new Dictionary<CellType, double>();
            
            if (heightIndex > 0 && nodes[heightIndex - 1, widthIndex].HasType)
            {
                if(nodes[heightIndex - 1, widthIndex].CellType == CellType.RampBT || 
                   nodes[heightIndex - 1, widthIndex].CellType == CellType.RampTB)
                {
                    return cellDataProvider.GetCellPixelsSpawnRoot(nodes[heightIndex - 1, widthIndex].CellType)[Direction.Down].First().Key;
                }
                
                AddPossibleCases(cellDataProvider, nodes, heightIndex - 1, widthIndex, Direction.Down, possibleCellTypes);
            }

            if (heightIndex < nodes.GetLength(0) - 1 && nodes[heightIndex + 1, widthIndex].HasType)
            {
                if(nodes[heightIndex + 1, widthIndex].CellType == CellType.RampBT || 
                   nodes[heightIndex + 1, widthIndex].CellType == CellType.RampTB)
                {
                    return cellDataProvider.GetCellPixelsSpawnRoot(nodes[heightIndex + 1, widthIndex].CellType)[Direction.Up].First().Key;
                }
                
                AddPossibleCases(cellDataProvider, nodes, heightIndex + 1, widthIndex, Direction.Up, possibleCellTypes);
            }

            if (widthIndex > 0 && nodes[heightIndex, widthIndex - 1].HasType)
            {
                if (nodes[heightIndex, widthIndex - 1].CellType == CellType.RampLR || 
                    nodes[heightIndex, widthIndex - 1].CellType == CellType.RampRL)
                {
                    return cellDataProvider.GetCellPixelsSpawnRoot(nodes[heightIndex, widthIndex - 1].CellType)[Direction.Right].First().Key;
                }
                
                AddPossibleCases(cellDataProvider, nodes, heightIndex, widthIndex - 1, Direction.Right, possibleCellTypes);
            }

            if (widthIndex < nodes.GetLength(1) - 1 && nodes[heightIndex, widthIndex + 1].HasType)
            {
                if(nodes[heightIndex, widthIndex + 1].CellType == CellType.RampLR || 
                   nodes[heightIndex, widthIndex + 1].CellType == CellType.RampRL)
                {
                    return cellDataProvider.GetCellPixelsSpawnRoot(nodes[heightIndex, widthIndex + 1].CellType)[Direction.Left].First().Key;
                }
                
                AddPossibleCases(cellDataProvider, nodes, heightIndex, widthIndex + 1, Direction.Left, possibleCellTypes);
            }

            double globalChance = 0f;

            foreach (var possibleCellType in possibleCellTypes)
            {
                globalChance += possibleCellType.Value;
            }

            float randomChance = Random.Range(0f, (float)globalChance);
            float currentChance = 0f;

            foreach (var possibleCellType in possibleCellTypes)
            {
                currentChance += (float)possibleCellType.Value;
                if (randomChance < currentChance)
                {
                    return possibleCellType.Key;
                }
            }
            
            return CellType.Plain;
        }

        private void AddPossibleCases(CellDataProvider cellDataProvider, Node[,] nodes, int heightIndex, int widthIndex, 
            Direction direction, Dictionary<CellType, double> possibleCellTypes)
        {
            foreach (var chanceMap in cellDataProvider.GetCellPixelsSpawnRoot(nodes[heightIndex, widthIndex].CellType)[direction])
            {
                if (possibleCellTypes.ContainsKey(chanceMap.Key))
                    possibleCellTypes[chanceMap.Key] += chanceMap.Value;
                else
                    possibleCellTypes.Add(chanceMap.Key, chanceMap.Value);
            }
        }

        private void DeleteRamps(Node[,] nodes, CellDataProvider cellDataProvider)
        {
            for (int i = 0; i < nodes.GetLength(0); i++)
            {
                for (int j = 0; j < nodes.GetLength(1); j++)
                {
                    if (nodes[i, j].CellType == CellType.RampBT || nodes[i, j].CellType == CellType.RampTB)
                    {
                        if (nodes[i - 1, j].CellType == CellType.Water)
                        {
                            nodes[i, j].CellType = nodes[i + 1, j].CellType;
                        }
                        if (nodes[i + 1, j].CellType == CellType.Water)
                        {
                            nodes[i, j].CellType = nodes[i - 1, j].CellType;
                        }
                    }

                    if (nodes[i, j].CellType == CellType.RampLR || nodes[i, j].CellType == CellType.RampRL)
                    {
                        if (nodes[i, j - 1].CellType == CellType.Water)
                        {
                            nodes[i, j].CellType = nodes[i, j + 1].CellType;
                        }
                        if (nodes[i, j + 1].CellType == CellType.Water)
                        {
                            nodes[i, j].CellType = nodes[i, j - 1].CellType;
                        }
                    }
                }
            }
        }

        private int SmoothOutPlayground(CellPresenter[,] playground)
        {
            int smoothedCells = 0;

            return smoothedCells;
        }
        
        
        
        

        private void SmoothOutPlayground2(CellPresenter[,] playground)
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