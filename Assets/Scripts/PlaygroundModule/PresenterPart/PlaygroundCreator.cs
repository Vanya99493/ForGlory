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
        
        
        
        
        
        class Bundle
        {
            public CellType[,] BundleTypes;
            
            public CellType[] LeftSide => new CellType[] { BundleTypes[0, 0], BundleTypes[1, 0] };
            public CellType[] RightSide => new CellType[] { BundleTypes[0, 1], BundleTypes[1, 1] };
            public CellType[] TopSide => new CellType[] { BundleTypes[0, 0], BundleTypes[0, 1] };
            public CellType[] BottomSide => new CellType[] { BundleTypes[1, 0], BundleTypes[1, 1] };

            public Bundle(CellType first, CellType second, CellType third, CellType fourth)
            {
                BundleTypes = new CellType[,]
                {
                    { first, second },
                    { third, fourth }
                };
            }

            public static bool CompareLRSides(CellType[] leftBundleSide, CellType[] rightBundleSide)
            {
                bool condition = true;

                for (int i = 0; i < leftBundleSide.Length; i++)
                {
                    if (leftBundleSide[i] == rightBundleSide[i])
                    {
                        condition = true;
                    }
                    else if ((leftBundleSide[i] == CellType.Water && rightBundleSide[i] == CellType.Plain) ||
                             (rightBundleSide[i] == CellType.Water && leftBundleSide[i] == CellType.Plain))
                    {
                        condition = true;
                    }
                    else if (leftBundleSide[i] == CellType.RampBT || leftBundleSide[i] == CellType.RampTB ||
                        rightBundleSide[i] == CellType.RampBT || rightBundleSide[i] == CellType.RampTB)
                    {
                        condition = true;
                    }
                    else if ((leftBundleSide[i] == CellType.RampLR && rightBundleSide[i] == CellType.Hill) ||
                             (leftBundleSide[i] == CellType.RampRL && rightBundleSide[i] == CellType.Plain) ||
                             (leftBundleSide[i] == CellType.Hill && rightBundleSide[i] == CellType.RampRL) ||
                             (leftBundleSide[i] == CellType.Plain && rightBundleSide[i] == CellType.RampLR))
                    {
                        condition = true;
                    }
                    else
                    {
                        condition = false;
                        return false;
                    }
                }

                return condition;
            }
            
            public static bool CompareTBSides(CellType[] topBundleSide, CellType[] bottomBundleSide)
            {
                bool condition = true;

                for (int i = 0; i < topBundleSide.Length; i++)
                {
                    if (topBundleSide[i] == bottomBundleSide[i])
                    {
                        condition = true;
                    }
                    else if ((topBundleSide[i] == CellType.Water && bottomBundleSide[i] == CellType.Plain) ||
                             (bottomBundleSide[i] == CellType.Water && topBundleSide[i] == CellType.Plain))
                    {
                        condition = true;
                    }
                    else if (topBundleSide[i] == CellType.RampLR || topBundleSide[i] == CellType.RampRL ||
                             bottomBundleSide[i] == CellType.RampLR || bottomBundleSide[i] == CellType.RampRL)
                    {
                        condition = true;
                    }
                    else if ((topBundleSide[i] == CellType.RampTB && bottomBundleSide[i] == CellType.Hill) ||
                             (topBundleSide[i] == CellType.RampBT && bottomBundleSide[i] == CellType.Plain) ||
                             (topBundleSide[i] == CellType.Hill && bottomBundleSide[i] == CellType.RampBT) ||
                             (topBundleSide[i] == CellType.Plain && bottomBundleSide[i] == CellType.RampTB))
                    {
                        condition =  true;
                    }
                    else
                    {
                        condition = false;
                        return condition;
                    }
                }
                
                return condition;
            }
        }
        
        public CellPresenter[,] CreatePlaygroundByBundles(int height, int width)
        {
            Bundle[,] bundles = new Bundle[height / 2, width / 2];
            List<Bundle> allBundles = new List<Bundle>()
            {
                new Bundle(CellType.Hill, CellType.Hill, CellType.Hill, CellType.Hill),
                new Bundle(CellType.Plain, CellType.Plain, CellType.Plain, CellType.Plain),
                
                new Bundle(CellType.Hill, CellType.Hill, CellType.Plain, CellType.Plain),
                new Bundle(CellType.Plain, CellType.Plain, CellType.Hill, CellType.Hill),
                new Bundle(CellType.Plain,CellType.Hill,  CellType.Plain, CellType.Hill),
                new Bundle(CellType.Hill,  CellType.Plain, CellType.Hill, CellType.Plain),
                
                new Bundle(CellType.Hill, CellType.Plain, CellType.Plain, CellType.Plain),
                new Bundle(CellType.Plain, CellType.Hill, CellType.Plain, CellType.Plain),
                new Bundle(CellType.Plain, CellType.Plain, CellType.Hill, CellType.Plain),
                new Bundle(CellType.Plain, CellType.Plain, CellType.Plain, CellType.Hill),
                
                new Bundle(CellType.Plain, CellType.Hill, CellType.Hill, CellType.Hill),
                new Bundle(CellType.Hill, CellType.Plain, CellType.Hill, CellType.Hill),
                new Bundle(CellType.Hill, CellType.Hill, CellType.Plain, CellType.Hill),
                new Bundle(CellType.Hill, CellType.Hill, CellType.Hill, CellType.Plain),
                
                new Bundle(CellType.Hill, CellType.Hill, CellType.RampBT, CellType.Water),
                new Bundle(CellType.Hill, CellType.Hill, CellType.RampBT, CellType.Plain),
                new Bundle(CellType.Hill, CellType.Plain, CellType.RampBT, CellType.Plain),
                new Bundle(CellType.Hill, CellType.Hill, CellType.Water, CellType.RampBT),
                new Bundle(CellType.Hill, CellType.Hill, CellType.Plain, CellType.RampBT),
                new Bundle(CellType.Plain, CellType.Hill, CellType.Plain, CellType.RampBT),
                
                new Bundle(CellType.RampTB, CellType.Water, CellType.Hill, CellType.Hill),
                new Bundle(CellType.RampTB, CellType.Plain, CellType.Hill, CellType.Hill),
                new Bundle(CellType.RampTB, CellType.Plain, CellType.Hill, CellType.Plain),
                new Bundle(CellType.Water, CellType.RampTB, CellType.Hill, CellType.Hill),
                new Bundle(CellType.Plain, CellType.RampTB, CellType.Hill, CellType.Hill),
                new Bundle(CellType.Plain, CellType.RampTB, CellType.Plain, CellType.Hill),
                
                new Bundle(CellType.RampLR, CellType.Hill, CellType.Water, CellType.Hill),
                new Bundle(CellType.RampLR, CellType.Hill, CellType.Plain, CellType.Hill),
                new Bundle(CellType.RampLR, CellType.Hill, CellType.Plain, CellType.Plain),
                new Bundle(CellType.Water, CellType.Hill, CellType.RampLR, CellType.Hill),
                new Bundle(CellType.Plain, CellType.Hill, CellType.RampLR, CellType.Hill),
                new Bundle(CellType.Plain, CellType.Plain, CellType.RampLR, CellType.Hill),
                
                new Bundle(CellType.Hill, CellType.RampRL, CellType.Hill, CellType.Water),
                new Bundle(CellType.Hill, CellType.RampRL, CellType.Hill, CellType.Plain),
                new Bundle(CellType.Hill, CellType.RampRL, CellType.Plain, CellType.Plain),
                new Bundle(CellType.Hill, CellType.Water, CellType.Hill, CellType.RampRL),
                new Bundle(CellType.Hill, CellType.Plain, CellType.Hill, CellType.RampRL),
                new Bundle(CellType.Plain, CellType.Plain, CellType.Hill, CellType.RampRL),
            };
            List<Bundle> matchedBundles;
            
            for (int i = 0; i < bundles.GetLength(0); i++)
            {
                for (int j = 0; j < bundles.GetLength(1); j++)
                {
                    matchedBundles = new List<Bundle>();
                    foreach (Bundle bundle in allBundles)
                    {
                        bool canAdd = true;
                        if (i != 0)
                            if(bundles[i - 1, j] == null || !Bundle.CompareTBSides(bundles[i - 1, j].BottomSide, bundle.TopSide))
                                continue;
                        if (j != 0)
                            if(bundles[i, j - 1] == null || !Bundle.CompareLRSides(bundles[i, j - 1].RightSide, bundle.LeftSide))
                               continue;
                        matchedBundles.Add(bundle);
                    }

                    if (matchedBundles.Count > 0)
                    {
                        bundles[i, j] = matchedBundles[Random.Range(0, matchedBundles.Count)];
                    }
                    else
                    {
                        bundles[i, j] = BuildBundle(bundles[i, j - 1], bundles[i - 1, j]);
                    }
                }
            }

            CellPresenter[,] playground = new CellPresenter[height, width];
            for (int i = 0; i < bundles.GetLength(0); i++)
            {
                for (int j = 0; j < bundles.GetLength(1); j++)
                {
                    playground[i * 2, j * 2] = new CellPresenter(new CellModel(bundles[i, j].BundleTypes[0, 0], i * 2, j * 2));
                    playground[i * 2, j * 2 + 1] = new CellPresenter(new CellModel(bundles[i, j].BundleTypes[0, 1], i * 2, j * 2 + 1));
                    playground[i * 2 + 1, j * 2] = new CellPresenter(new CellModel(bundles[i, j].BundleTypes[1, 0], i * 2 + 1, j * 2));
                    playground[i * 2 + 1, j * 2 + 1] = new CellPresenter(new CellModel(bundles[i, j].BundleTypes[1, 1], i * 2 + 1, j * 2 + 1));
                }
            }
            
            return playground;
        }

        private Bundle BuildBundle(Bundle leftBundle, Bundle topBundle)
        {
            CellType tl, tr, bl, br;

            if (leftBundle.BundleTypes[0, 1] == CellType.RampRL)
            {
                if (topBundle.BundleTypes[1, 0] == CellType.RampTB)
                    leftBundle.BundleTypes[0, 1] = CellType.Plain;
                tl = CellType.Plain;
            }
            else if (leftBundle.BundleTypes[0, 1] == CellType.RampLR)
            {
                if (topBundle.BundleTypes[1, 0] != CellType.RampBT)
                    topBundle.BundleTypes[1, 0] = CellType.Hill;
                tl = CellType.Hill;
            }
            else if (topBundle.BundleTypes[1, 0] == CellType.RampTB)
            {
                tl = CellType.Hill;
            }
            else if (topBundle.BundleTypes[1, 0] == CellType.RampBT)
            {
                tl = CellType.Plain;
            }
            else
            {
                tl = Random.Range(0, 2) == 0 ? leftBundle.BundleTypes[0, 1] : topBundle.BundleTypes[1, 0];
            }

            if (leftBundle.BundleTypes[1, 1] == CellType.RampLR)
            {
                bl = CellType.Hill;
            }
            else if (leftBundle.BundleTypes[1, 1] == CellType.RampRL)
            {
                bl = CellType.Plain;
            }
            else
            {
                bl = Random.Range(0, 2) == 0 ? leftBundle.BundleTypes[1, 1] : tl;
            }
            
            if (topBundle.BundleTypes[1, 1] == CellType.RampTB)
            {
                tr = CellType.Hill;
            }
            else if (topBundle.BundleTypes[1, 1] == CellType.RampBT)
            {
                tr = CellType.Plain;
            }
            else
            {
                tr = Random.Range(0, 2) == 0 ? topBundle.BundleTypes[1, 1] : tl;
            }
            
            br = Random.Range(0, 2) == 0 ? bl : tr;

            Bundle bundle = new Bundle(tl, tr, bl, br);
            return bundle;
        }
        
        
        
        
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