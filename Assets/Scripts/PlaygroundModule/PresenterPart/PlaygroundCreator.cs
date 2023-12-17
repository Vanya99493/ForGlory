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
            nodes[heightStartIndex, widthStartIndex].Visited = true;
            nodes[heightStartIndex, widthStartIndex].CellType = CellType.Plain;
            nodes[heightStartIndex, widthStartIndex].HasType = true;

            while (nodesQueue.Count > 0)
            {
                Node pickedNode = nodesQueue.Dequeue();

                if (pickedNode.HeightIndex > 0 && 
                    !nodes[pickedNode.HeightIndex - 1, pickedNode.WidthIndex].Visited)
                {
                    nodes[pickedNode.HeightIndex - 1, pickedNode.WidthIndex].Visited = true;
                    nodes[pickedNode.HeightIndex - 1, pickedNode.WidthIndex].CellType =
                        ChooseCellType(cellDataProvider, pickedNode.HeightIndex - 1, pickedNode.WidthIndex, nodes);
                    nodes[pickedNode.HeightIndex - 1, pickedNode.WidthIndex].HasType = true;
                    nodesQueue.Enqueue(nodes[pickedNode.HeightIndex - 1, pickedNode.WidthIndex]);
                }
                if (pickedNode.HeightIndex < nodes.GetLength(0) - 1 && 
                    !nodes[pickedNode.HeightIndex + 1, pickedNode.WidthIndex].Visited)
                {
                    nodes[pickedNode.HeightIndex + 1, pickedNode.WidthIndex].Visited = true;
                    nodes[pickedNode.HeightIndex + 1, pickedNode.WidthIndex].CellType =
                        ChooseCellType(cellDataProvider, pickedNode.HeightIndex + 1, pickedNode.WidthIndex, nodes);
                    nodes[pickedNode.HeightIndex + 1, pickedNode.WidthIndex].HasType = true;
                    nodesQueue.Enqueue(nodes[pickedNode.HeightIndex + 1, pickedNode.WidthIndex]);
                }
                if (pickedNode.WidthIndex > 0 && 
                    !nodes[pickedNode.HeightIndex, pickedNode.WidthIndex - 1].Visited)
                {
                    nodes[pickedNode.HeightIndex, pickedNode.WidthIndex - 1].Visited = true;
                    nodes[pickedNode.HeightIndex, pickedNode.WidthIndex - 1].CellType =
                        ChooseCellType(cellDataProvider, pickedNode.HeightIndex, pickedNode.WidthIndex - 1, nodes);
                    nodes[pickedNode.HeightIndex, pickedNode.WidthIndex - 1].HasType = true;
                    nodesQueue.Enqueue(nodes[pickedNode.HeightIndex, pickedNode.WidthIndex - 1]);
                }
                if (pickedNode.WidthIndex < nodes.GetLength(1) - 1 && 
                    !nodes[pickedNode.HeightIndex, pickedNode.WidthIndex + 1].Visited)
                {
                    nodes[pickedNode.HeightIndex, pickedNode.WidthIndex + 1].Visited = true;
                    nodes[pickedNode.HeightIndex, pickedNode.WidthIndex + 1].CellType =
                        ChooseCellType(cellDataProvider, pickedNode.HeightIndex, pickedNode.WidthIndex + 1, nodes);
                    nodes[pickedNode.HeightIndex, pickedNode.WidthIndex + 1].HasType = true;
                    nodesQueue.Enqueue(nodes[pickedNode.HeightIndex, pickedNode.WidthIndex + 1]);
                }
            }
            
            CellPresenter[,] playground = new CellPresenter[height, width];

            for (int i = 0; i < playground.GetLength(0); i++)
            {
                for (int j = 0; j < playground.GetLength(1); j++)
                {
                    playground[i, j] = new CellPresenter(new CellModel(nodes[i, j].CellType, nodes[i, j].HeightIndex, nodes[i, j].WidthIndex));
                }
            }

            return playground;
        }

        private void FillWater(Node[,] nodes, int lengthOfWater)
        {
            for (int i = 0; i < lengthOfWater; i++)
            {
                for (int j = 0; j < nodes.GetLength(1); j++)
                {
                    nodes[i, j].Visited = true;
                    nodes[i, j].CellType = CellType.Water;
                    nodes[i, j].HasType = true;
                }
                for (int j = 0; j < nodes.GetLength(0); j++)
                {
                    nodes[j, i].Visited = true;
                    nodes[j, i].CellType = CellType.Water;
                    nodes[j, i].HasType = true;
                }
            }

            for (int i = nodes.GetLength(0) - 1, k = 0; k < lengthOfWater; i--, k++)
            {
                for (int j = 0; j < nodes.GetLength(1); j++)
                {
                    nodes[i, j].Visited = true;
                    nodes[i, j].CellType = CellType.Water;
                    nodes[i, j].HasType = true;
                }
            }
            
            for (int i = nodes.GetLength(1) - 1, k = 0; k < lengthOfWater; i--, k++)
            {
                for (int j = 0; j < nodes.GetLength(0); j++)
                {
                    nodes[j, i].Visited = true;
                    nodes[j, i].CellType = CellType.Water;
                    nodes[j, i].HasType = true;
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
                nodes[j, lengthOfWater].Visited = true;
                nodes[j, lengthOfWater].CellType = CellType.Water;
                nodes[j, lengthOfWater].HasType = true;
            }
            for (int i = lengthOfWater + 1; i < nodes.GetLength(1) - lengthOfWater - 1; i++)
            {
                for (int j = lengthOfWater; j < lengthOfCoast + lengthOfWater - coastCurrentPoint; j++)
                {
                    nodes[j, i].Visited = true;
                    nodes[j, i].CellType = CellType.Water;
                    nodes[j, i].HasType = true;
                }
                
                coastCurrentPoint = Random.Range(0, 2) == 0
                    ? coastCurrentPoint + 1 > lengthOfCoast ? coastCurrentPoint : ++coastCurrentPoint
                    : coastCurrentPoint - 1 < 0 ? coastCurrentPoint : --coastCurrentPoint;
            }
            for (int j = lengthOfWater; j < lengthOfCoast + lengthOfWater; j++)
            {
                nodes[j, nodes.GetLength(1) - lengthOfWater - 1].Visited = true;
                nodes[j, nodes.GetLength(1) - lengthOfWater - 1].CellType = CellType.Water;
                nodes[j, nodes.GetLength(1) - lengthOfWater - 1].HasType = true;
            }

            // left s right
            coastCurrentPoint = coastStartPoint;
            int coastCurrentPoint2 = coastStartPoint;
            
            for (int i = lengthOfWater + lengthOfCoast; i < nodes.GetLength(0) - (lengthOfWater + lengthOfCoast); i++)
            {
                for (int j = lengthOfWater; j < lengthOfCoast + lengthOfWater - coastCurrentPoint; j++)
                {
                    nodes[i, j].Visited = true;
                    nodes[i, j].CellType = CellType.Water;
                    nodes[i, j].HasType = true;
                }
                
                coastCurrentPoint = Random.Range(0, 2) == 0
                    ? coastCurrentPoint + 1 > lengthOfCoast ? coastCurrentPoint : ++coastCurrentPoint
                    : coastCurrentPoint - 1 < 0 ? coastCurrentPoint : --coastCurrentPoint;

                for (int j = nodes.GetLength(1) - lengthOfWater; j > nodes.GetLength(1) - (lengthOfWater + lengthOfCoast) + coastCurrentPoint2; j--)
                {
                    nodes[i, j].Visited = true;
                    nodes[i, j].CellType = CellType.Water;
                    nodes[i, j].HasType = true;
                }
                
                coastCurrentPoint2 = Random.Range(0, 2) == 0
                    ? coastCurrentPoint2 + 1 > lengthOfCoast ? coastCurrentPoint2 : ++coastCurrentPoint2
                    : coastCurrentPoint2 - 1 < 0 ? coastCurrentPoint2 : --coastCurrentPoint2;
            }
            
            // bottom
            coastCurrentPoint = coastStartPoint;
            
            for (int j = nodes.GetLength(0) - lengthOfWater - 1; j > nodes.GetLength(0) - (lengthOfCoast + lengthOfWater) - 1; j--)
            {
                nodes[j, lengthOfWater].Visited = true;
                nodes[j, lengthOfWater].CellType = CellType.Water;
                nodes[j, lengthOfWater].HasType = true;
            }
            for (int i = lengthOfWater + 1; i < nodes.GetLength(1) - lengthOfWater - 1; i++)
            {
                for (int j = nodes.GetLength(1) - lengthOfWater - 1; j > nodes.GetLength(1) - lengthOfWater - lengthOfCoast - 1 + coastCurrentPoint; j--)
                {
                    nodes[j, i].Visited = true;
                    nodes[j, i].CellType = CellType.Water;
                    nodes[j, i].HasType = true;
                }
                
                coastCurrentPoint = Random.Range(0, 2) == 0
                    ? coastCurrentPoint + 1 > lengthOfCoast ? coastCurrentPoint : ++coastCurrentPoint
                    : coastCurrentPoint - 1 < 0 ? coastCurrentPoint : --coastCurrentPoint;
            }
            for (int j = nodes.GetLength(0) - lengthOfWater - 1; j > nodes.GetLength(0) - (lengthOfCoast + lengthOfWater) - 1; j--)
            {
                nodes[j, nodes.GetLength(1) - lengthOfWater - 1].Visited = true;
                nodes[j, nodes.GetLength(1) - lengthOfWater - 1].CellType = CellType.Water;
                nodes[j, nodes.GetLength(1) - lengthOfWater - 1].HasType = true;
            }
        }

        private CellType ChooseCellType(CellDataProvider cellDataProvider, int heightIndex, int widthIndex, Node[,] nodes)
        {
            Dictionary<CellType, double> possibleCellTypes = new Dictionary<CellType, double>();
            
            if (heightIndex > 0 && nodes[heightIndex - 1, widthIndex].HasType)
            {
                if(new List<CellType>{CellType.RampBT, CellType.RampTB}.Contains(nodes[heightIndex - 1, widthIndex].CellType))
                {
                    return cellDataProvider.GetCellPixelsSpawnRoot(nodes[heightIndex - 1, widthIndex].CellType)[Direction.Down].First().Key;
                }
                
                foreach (var chanceMap in cellDataProvider.GetCellPixelsSpawnRoot(nodes[heightIndex - 1, widthIndex].CellType)[Direction.Down])
                {
                    if (possibleCellTypes.ContainsKey(chanceMap.Key))
                        possibleCellTypes[chanceMap.Key] += chanceMap.Value;
                    else
                        possibleCellTypes.Add(chanceMap.Key, chanceMap.Value);
                }
            }

            if (heightIndex < nodes.GetLength(0) - 1 && nodes[heightIndex + 1, widthIndex].HasType)
            {
                if(new List<CellType>{CellType.RampBT, CellType.RampTB}.Contains(nodes[heightIndex + 1, widthIndex].CellType))
                {
                    return cellDataProvider.GetCellPixelsSpawnRoot(nodes[heightIndex + 1, widthIndex].CellType)[Direction.Up].First().Key;
                }
                
                foreach (var chanceMap in cellDataProvider.GetCellPixelsSpawnRoot(nodes[heightIndex + 1, widthIndex].CellType)[Direction.Up])
                {
                    if (possibleCellTypes.ContainsKey(chanceMap.Key))
                        possibleCellTypes[chanceMap.Key] += chanceMap.Value;
                    else
                        possibleCellTypes.Add(chanceMap.Key, chanceMap.Value);
                }
            }

            if (widthIndex > 0 && nodes[heightIndex, widthIndex - 1].HasType)
            {
                if (new List<CellType> { CellType.RampLR, CellType.RampRL }.Contains(nodes[heightIndex, widthIndex - 1]
                        .CellType))
                {
                    return cellDataProvider.GetCellPixelsSpawnRoot(nodes[heightIndex, widthIndex - 1].CellType)[Direction.Right].First().Key;
                }
                
                foreach (var chanceMap in cellDataProvider.GetCellPixelsSpawnRoot(nodes[heightIndex, widthIndex - 1].CellType)[Direction.Right])
                {
                    if (possibleCellTypes.ContainsKey(chanceMap.Key))
                        possibleCellTypes[chanceMap.Key] += chanceMap.Value;
                    else
                        possibleCellTypes.Add(chanceMap.Key, chanceMap.Value);
                }
            }

            if (widthIndex < nodes.GetLength(1) - 1 && nodes[heightIndex, widthIndex + 1].HasType)
            {
                if(new List<CellType>{CellType.RampLR, CellType.RampRL}.Contains(nodes[heightIndex, widthIndex + 1].CellType))
                {
                    return cellDataProvider.GetCellPixelsSpawnRoot(nodes[heightIndex, widthIndex + 1].CellType)[Direction.Left].First().Key;
                }
                
                foreach (var chanceMap in cellDataProvider.GetCellPixelsSpawnRoot(nodes[heightIndex, widthIndex + 1].CellType)[Direction.Left])
                {
                    if (possibleCellTypes.ContainsKey(chanceMap.Key))
                        possibleCellTypes[chanceMap.Key] += chanceMap.Value;
                    else
                        possibleCellTypes.Add(chanceMap.Key, chanceMap.Value);
                }
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
    }
}