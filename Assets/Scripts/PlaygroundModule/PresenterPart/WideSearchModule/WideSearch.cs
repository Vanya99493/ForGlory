using System.Collections.Generic;
using CustomClasses;
using Infrastructure.Providers;
using Infrastructure.ServiceLocatorModule;
using PlaygroundModule.ModelPart;

namespace PlaygroundModule.PresenterPart.WideSearchModule
{
    public class WideSearch : IService
    {
        private CellDataProvider _cellDataProvider;

        public WideSearch(CellDataProvider cellDataProvider)
        {
            _cellDataProvider = cellDataProvider;
        }
        
        public List<Node> GetCellsByLength(int length, Node startNode, PlaygroundPresenter playgroundPresenter, bool considerCharacters)
        {
            List<Node> cells = new List<Node>();
            
            Node[,] bfsArray = TranslatePlayground(playgroundPresenter);

            Queue<Node> bfsQueue = new Queue<Node>();
            bfsQueue.Enqueue(startNode);
            bfsArray[startNode.HeightIndex, startNode.WidthIndex].Visited = true;
            cells.Add(bfsArray[startNode.HeightIndex, startNode.WidthIndex]);
            
            while (bfsQueue.Count > 0)
            {
                Node currentNode = bfsQueue.Dequeue();

                if (currentNode.Distance >= length)
                {
                    continue;
                }

                if (!IsExtreme(currentNode, bfsArray, Direction.Up) && !bfsArray[currentNode.HeightIndex - 1, currentNode.WidthIndex].Visited && CanMove(
                        bfsArray[currentNode.HeightIndex, currentNode.WidthIndex],
                        bfsArray[currentNode.HeightIndex - 1, currentNode.WidthIndex],
                        Direction.Up,
                        considerCharacters
                    ))
                {
                    bfsArray[currentNode.HeightIndex - 1, currentNode.WidthIndex].Visited = true;
                    bfsArray[currentNode.HeightIndex - 1, currentNode.WidthIndex].Distance = currentNode.Distance + 1;
                    cells.Add(bfsArray[currentNode.HeightIndex - 1, currentNode.WidthIndex]);
                    if (!bfsArray[currentNode.HeightIndex - 1, currentNode.WidthIndex].IsBusy)
                    {
                        bfsQueue.Enqueue(bfsArray[currentNode.HeightIndex - 1, currentNode.WidthIndex]);
                    }
                }
                if (!IsExtreme(currentNode, bfsArray, Direction.Down) && !bfsArray[currentNode.HeightIndex + 1, currentNode.WidthIndex].Visited && CanMove(
                        bfsArray[currentNode.HeightIndex, currentNode.WidthIndex],
                        bfsArray[currentNode.HeightIndex + 1, currentNode.WidthIndex],
                        Direction.Down,
                        considerCharacters
                    ))
                {
                    bfsArray[currentNode.HeightIndex + 1, currentNode.WidthIndex].Visited = true;
                    bfsArray[currentNode.HeightIndex + 1, currentNode.WidthIndex].Distance = currentNode.Distance + 1;
                    cells.Add(bfsArray[currentNode.HeightIndex + 1, currentNode.WidthIndex]);
                    if (!bfsArray[currentNode.HeightIndex + 1, currentNode.WidthIndex].IsBusy)
                    {
                        bfsQueue.Enqueue(bfsArray[currentNode.HeightIndex + 1, currentNode.WidthIndex]);
                    }
                }
                if (!IsExtreme(currentNode, bfsArray, Direction.Left) && !bfsArray[currentNode.HeightIndex, currentNode.WidthIndex - 1].Visited && CanMove(
                        bfsArray[currentNode.HeightIndex, currentNode.WidthIndex],
                        bfsArray[currentNode.HeightIndex, currentNode.WidthIndex - 1],
                        Direction.Left,
                        considerCharacters
                    ))
                {
                    bfsArray[currentNode.HeightIndex, currentNode.WidthIndex - 1].Visited = true;
                    bfsArray[currentNode.HeightIndex, currentNode.WidthIndex - 1].Distance = currentNode.Distance + 1;
                    cells.Add(bfsArray[currentNode.HeightIndex, currentNode.WidthIndex - 1]);
                    if (!bfsArray[currentNode.HeightIndex, currentNode.WidthIndex - 1].IsBusy)
                    {
                        bfsQueue.Enqueue(bfsArray[currentNode.HeightIndex, currentNode.WidthIndex - 1]);
                    }
                }
                if (!IsExtreme(currentNode, bfsArray, Direction.Right) && !bfsArray[currentNode.HeightIndex, currentNode.WidthIndex + 1].Visited && CanMove(
                        bfsArray[currentNode.HeightIndex, currentNode.WidthIndex],
                        bfsArray[currentNode.HeightIndex, currentNode.WidthIndex + 1],
                        Direction.Right,
                        considerCharacters
                    ))
                {
                    bfsArray[currentNode.HeightIndex, currentNode.WidthIndex + 1].Visited = true;
                    bfsArray[currentNode.HeightIndex, currentNode.WidthIndex + 1].Distance = currentNode.Distance + 1;
                    cells.Add(bfsArray[currentNode.HeightIndex, currentNode.WidthIndex + 1]);
                    if (!bfsArray[currentNode.HeightIndex, currentNode.WidthIndex + 1].IsBusy)
                    {
                        bfsQueue.Enqueue(bfsArray[currentNode.HeightIndex, currentNode.WidthIndex + 1]);
                    }
                }
            }

            return cells;
        }
        
        public bool TryBuildRoute(Node startNode, Node targetNode, PlaygroundPresenter playgroundPresenter, bool considerCharacters, out List<Pair<int, int>> route)
        {
            if (targetNode.CellType == CellType.Water)
            {
                route = new List<Pair<int, int>>();
                return false;
            }
            
            Node[,] bfsArray = TranslatePlayground(playgroundPresenter);

            Queue<Node> bfsQueue = new Queue<Node>();
            bfsQueue.Enqueue(startNode);
            bfsArray[startNode.HeightIndex, startNode.WidthIndex].Visited = true;
            
            while (bfsQueue.Count > 0)
            {
                Node currentNode = bfsQueue.Dequeue();

                if (currentNode == targetNode)
                {
                    route = BuildRoute(startNode, currentNode);
                    return true;
                }

                if (!IsExtreme(currentNode, bfsArray, Direction.Up) && !bfsArray[currentNode.HeightIndex - 1, currentNode.WidthIndex].Visited && CanMove(
                        bfsArray[currentNode.HeightIndex, currentNode.WidthIndex],
                        bfsArray[currentNode.HeightIndex - 1, currentNode.WidthIndex],
                        targetNode,
                        Direction.Up,
                        considerCharacters
                    ))
                {
                    bfsArray[currentNode.HeightIndex - 1, currentNode.WidthIndex].Visited = true;
                    bfsArray[currentNode.HeightIndex - 1, currentNode.WidthIndex].PrevNode = currentNode;
                    bfsQueue.Enqueue(bfsArray[currentNode.HeightIndex - 1, currentNode.WidthIndex]);
                }
                if (!IsExtreme(currentNode, bfsArray, Direction.Down) && !bfsArray[currentNode.HeightIndex + 1, currentNode.WidthIndex].Visited && CanMove(
                        bfsArray[currentNode.HeightIndex, currentNode.WidthIndex],
                        bfsArray[currentNode.HeightIndex + 1, currentNode.WidthIndex],
                        targetNode,
                        Direction.Down,
                        considerCharacters
                    ))
                {
                    bfsArray[currentNode.HeightIndex + 1, currentNode.WidthIndex].Visited = true;
                    bfsArray[currentNode.HeightIndex + 1, currentNode.WidthIndex].PrevNode = currentNode;
                    bfsQueue.Enqueue(bfsArray[currentNode.HeightIndex + 1, currentNode.WidthIndex]);
                }
                if (!IsExtreme(currentNode, bfsArray, Direction.Left) && !bfsArray[currentNode.HeightIndex, currentNode.WidthIndex - 1].Visited && CanMove(
                        bfsArray[currentNode.HeightIndex, currentNode.WidthIndex],
                        bfsArray[currentNode.HeightIndex, currentNode.WidthIndex - 1],
                        targetNode,
                        Direction.Left,
                        considerCharacters
                    ))
                {
                    bfsArray[currentNode.HeightIndex, currentNode.WidthIndex - 1].Visited = true;
                    bfsArray[currentNode.HeightIndex, currentNode.WidthIndex - 1].PrevNode = currentNode;
                    bfsQueue.Enqueue(bfsArray[currentNode.HeightIndex, currentNode.WidthIndex - 1]);
                }
                if (!IsExtreme(currentNode, bfsArray, Direction.Right) && !bfsArray[currentNode.HeightIndex, currentNode.WidthIndex + 1].Visited && CanMove(
                        bfsArray[currentNode.HeightIndex, currentNode.WidthIndex],
                        bfsArray[currentNode.HeightIndex, currentNode.WidthIndex + 1],
                        targetNode,
                        Direction.Right,
                        considerCharacters
                    ))
                {
                    bfsArray[currentNode.HeightIndex, currentNode.WidthIndex + 1].Visited = true;
                    bfsArray[currentNode.HeightIndex, currentNode.WidthIndex + 1].PrevNode = currentNode;
                    bfsQueue.Enqueue(bfsArray[currentNode.HeightIndex, currentNode.WidthIndex + 1]);
                }
            }

            route = new List<Pair<int, int>>();
            return false;
        }

        private Node[,] TranslatePlayground(PlaygroundPresenter playgroundPresenter)
        {
            Node[,] bfsArray = new Node[playgroundPresenter.Model.PlaygroundHeight, playgroundPresenter.Model.PlaygroundWidth];

            for (int i = 0; i < bfsArray.GetLength(0); i++)
            {
                for (int j = 0; j < bfsArray.GetLength(1); j++)
                {
                    bfsArray[i, j] = new Node(i, j, 
                        playgroundPresenter.Model.GetCellPresenter(i, j).Model.CellType, 
                        playgroundPresenter.CheckCellOnCharacter(i, j));
                }
            }

            return bfsArray;
        }

        private bool IsExtreme(Node node, Node[,] bfsArray, Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    if (node.HeightIndex == 0)
                        return true;
                    break;
                case Direction.Down:
                    if (node.HeightIndex == bfsArray.GetLength(0) - 1)
                        return true;
                    break;
                case Direction.Left:
                    if (node.WidthIndex == 0)
                        return true;
                    break;
                case Direction.Right:
                    if (node.WidthIndex == bfsArray.GetLength(1) - 1)
                        return true; 
                    break;
            }

            return false;
        }

        private List<Pair<int, int>> BuildRoute(Node startNode, Node lastRouteNode)
        {
            List<Pair<int, int>> route = new List<Pair<int, int>>();

            Node currentNode = lastRouteNode;

            while (currentNode != startNode)
            {
                route.Add(new Pair<int, int>(currentNode.HeightIndex, currentNode.WidthIndex));
                currentNode = currentNode.PrevNode;
            }
            
            route.Reverse();
            
            return route;
        }
        
        private bool CanMove(Node start, Node target, Direction direction, bool considerCharacters)
        {
            return _cellDataProvider.GetCellPixelsMoveRoots(start.CellType)[direction].Contains(target.CellType) && 
                   (!target.IsBusy || !considerCharacters);
        }

        private bool CanMove(Node start, Node target, Node finalTarget, Direction direction, bool considerCharacters)
        {
            return _cellDataProvider.GetCellPixelsMoveRoots(start.CellType)[direction].Contains(target.CellType) && 
                   (!target.IsBusy || (!considerCharacters && IsFinalPoint(target, finalTarget)));
        }

        private bool IsFinalPoint(Node node, Node finalPoint) => node.HeightIndex == finalPoint.HeightIndex && node.WidthIndex == finalPoint.WidthIndex;
    }
}