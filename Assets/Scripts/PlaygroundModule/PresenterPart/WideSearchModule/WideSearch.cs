using System.Collections.Generic;
using CustomClasses;
using PlaygroundModule.ModelPart;

namespace PlaygroundModule.PresenterPart.WideSearchModule
{
    public class WideSearch
    {
        public List<Node> GetCellsByLength(int length, Node startNode, PlaygroundPresenter playgroundPresenter)
        {
            List<Node> cells = new List<Node>();
            
            Node[,] bfsArray = TranslatePlayground(playgroundPresenter);

            Queue<Node> bfsQueue = new Queue<Node>();
            bfsQueue.Enqueue(startNode);
            bfsArray[startNode.HeightIndex, startNode.WidthIndex].Visited = true;
            
            while (bfsQueue.Count > 0)
            {
                Node currentNode = bfsQueue.Dequeue();

                if (currentNode.Distance > length)
                {
                    continue;
                }

                if (currentNode.HeightIndex != 0 && !bfsArray[currentNode.HeightIndex - 1, currentNode.WidthIndex].Visited && CanMove(
                        bfsArray[currentNode.HeightIndex, currentNode.WidthIndex].CellType,
                        bfsArray[currentNode.HeightIndex - 1, currentNode.WidthIndex].CellType,
                        Direction.Up
                    ))
                {
                    bfsArray[currentNode.HeightIndex - 1, currentNode.WidthIndex].Visited = true;
                    bfsArray[currentNode.HeightIndex - 1, currentNode.WidthIndex].PrevNode = currentNode;
                    bfsArray[currentNode.HeightIndex - 1, currentNode.WidthIndex].Distance = bfsArray[currentNode.HeightIndex, currentNode.WidthIndex].Distance + 1;
                    bfsQueue.Enqueue(bfsArray[currentNode.HeightIndex - 1, currentNode.WidthIndex]);
                    cells.Add(bfsArray[currentNode.HeightIndex - 1, currentNode.WidthIndex]);
                }
                if (currentNode.HeightIndex != bfsArray.GetLength(0) - 1 && !bfsArray[currentNode.HeightIndex + 1, currentNode.WidthIndex].Visited && CanMove(
                        bfsArray[currentNode.HeightIndex, currentNode.WidthIndex].CellType,
                        bfsArray[currentNode.HeightIndex + 1, currentNode.WidthIndex].CellType,
                        Direction.Down
                    ))
                {
                    bfsArray[currentNode.HeightIndex + 1, currentNode.WidthIndex].Visited = true;
                    bfsArray[currentNode.HeightIndex + 1, currentNode.WidthIndex].PrevNode = currentNode;
                    bfsArray[currentNode.HeightIndex + 1, currentNode.WidthIndex].Distance = bfsArray[currentNode.HeightIndex, currentNode.WidthIndex].Distance + 1;
                    bfsQueue.Enqueue(bfsArray[currentNode.HeightIndex + 1, currentNode.WidthIndex]);
                    cells.Add(bfsArray[currentNode.HeightIndex + 1, currentNode.WidthIndex]);
                }
                if (currentNode.WidthIndex != 0 && !bfsArray[currentNode.HeightIndex, currentNode.WidthIndex - 1].Visited && CanMove(
                        bfsArray[currentNode.HeightIndex, currentNode.WidthIndex].CellType,
                        bfsArray[currentNode.HeightIndex, currentNode.WidthIndex - 1].CellType,
                        Direction.Left
                    ))
                {
                    bfsArray[currentNode.HeightIndex, currentNode.WidthIndex - 1].Visited = true;
                    bfsArray[currentNode.HeightIndex, currentNode.WidthIndex - 1].PrevNode = currentNode;
                    bfsArray[currentNode.HeightIndex, currentNode.WidthIndex - 1].Distance = bfsArray[currentNode.HeightIndex, currentNode.WidthIndex].Distance + 1;
                    bfsQueue.Enqueue(bfsArray[currentNode.HeightIndex, currentNode.WidthIndex - 1]);
                    cells.Add(bfsArray[currentNode.HeightIndex, currentNode.WidthIndex - 1]);
                }
                if (currentNode.WidthIndex != bfsArray.GetLength(1) - 1 && !bfsArray[currentNode.HeightIndex, currentNode.WidthIndex + 1].Visited && CanMove(
                        bfsArray[currentNode.HeightIndex, currentNode.WidthIndex].CellType,
                        bfsArray[currentNode.HeightIndex, currentNode.WidthIndex + 1].CellType,
                        Direction.Right
                    ))
                {
                    bfsArray[currentNode.HeightIndex, currentNode.WidthIndex + 1].Visited = true;
                    bfsArray[currentNode.HeightIndex, currentNode.WidthIndex + 1].PrevNode = currentNode;
                    bfsArray[currentNode.HeightIndex, currentNode.WidthIndex+ 1].Distance = bfsArray[currentNode.HeightIndex, currentNode.WidthIndex].Distance + 1;
                    bfsQueue.Enqueue(bfsArray[currentNode.HeightIndex, currentNode.WidthIndex + 1]);
                    cells.Add(bfsArray[currentNode.HeightIndex, currentNode.WidthIndex + 1]);
                }
            }

            return cells;
        }
        
        public bool TryBuildRoute(Node startNode, Node targetNode, PlaygroundPresenter playgroundPresenter, out List<Pair<int, int>> route)
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

                if (currentNode.HeightIndex != 0 && !bfsArray[currentNode.HeightIndex - 1, currentNode.WidthIndex].Visited && CanMove(
                        bfsArray[currentNode.HeightIndex, currentNode.WidthIndex].CellType,
                        bfsArray[currentNode.HeightIndex - 1, currentNode.WidthIndex].CellType,
                        Direction.Up
                    ))
                {
                    bfsArray[currentNode.HeightIndex - 1, currentNode.WidthIndex].Visited = true;
                    bfsArray[currentNode.HeightIndex - 1, currentNode.WidthIndex].PrevNode = currentNode;
                    bfsQueue.Enqueue(bfsArray[currentNode.HeightIndex - 1, currentNode.WidthIndex]);
                }
                if (currentNode.HeightIndex != bfsArray.GetLength(0) - 1 && !bfsArray[currentNode.HeightIndex + 1, currentNode.WidthIndex].Visited && CanMove(
                        bfsArray[currentNode.HeightIndex, currentNode.WidthIndex].CellType,
                        bfsArray[currentNode.HeightIndex + 1, currentNode.WidthIndex].CellType,
                        Direction.Down
                    ))
                {
                    bfsArray[currentNode.HeightIndex + 1, currentNode.WidthIndex].Visited = true;
                    bfsArray[currentNode.HeightIndex + 1, currentNode.WidthIndex].PrevNode = currentNode;
                    bfsQueue.Enqueue(bfsArray[currentNode.HeightIndex + 1, currentNode.WidthIndex]);
                }
                if (currentNode.WidthIndex != 0 && !bfsArray[currentNode.HeightIndex, currentNode.WidthIndex - 1].Visited && CanMove(
                        bfsArray[currentNode.HeightIndex, currentNode.WidthIndex].CellType,
                        bfsArray[currentNode.HeightIndex, currentNode.WidthIndex - 1].CellType,
                        Direction.Left
                    ))
                {
                    bfsArray[currentNode.HeightIndex, currentNode.WidthIndex - 1].Visited = true;
                    bfsArray[currentNode.HeightIndex, currentNode.WidthIndex - 1].PrevNode = currentNode;
                    bfsQueue.Enqueue(bfsArray[currentNode.HeightIndex, currentNode.WidthIndex - 1]);
                }
                if (currentNode.WidthIndex != bfsArray.GetLength(1) - 1 && !bfsArray[currentNode.HeightIndex, currentNode.WidthIndex + 1].Visited && CanMove(
                        bfsArray[currentNode.HeightIndex, currentNode.WidthIndex].CellType,
                        bfsArray[currentNode.HeightIndex, currentNode.WidthIndex + 1].CellType,
                        Direction.Right
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
            Node[,] bfsArray = new Node[playgroundPresenter.Model.Height, playgroundPresenter.Model.Width];

            for (int i = 0; i < bfsArray.GetLength(0); i++)
            {
                for (int j = 0; j < bfsArray.GetLength(1); j++)
                {
                    bfsArray[i, j] = new Node(i, j, playgroundPresenter.Model.GetCellPresenter(i, j).Model.CellType);
                }
            }

            return bfsArray;
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

        private bool CanMove(CellType start, CellType target, Direction direction)
        {
            if (start == target)
                return true;
            
            if (target == CellType.Water)
                return false;
            if (start == CellType.Plain)
            {
                switch (direction)
                {
                    case Direction.Up:
                        if (target == CellType.RampBT)
                            return true;
                        break;
                    case Direction.Down:
                        if (target == CellType.RampTB)
                            return true;
                        break;
                    case Direction.Left:
                        if (target == CellType.RampRL)
                            return true;
                        break;
                    case Direction.Right:
                        if (target == CellType.RampLR)
                            return true;
                        break;
                }
            }
            if (start == CellType.Hill)
            {
                switch (direction)
                {
                    case Direction.Up:
                        if (target == CellType.RampTB)
                            return true;
                        break;
                    case Direction.Down:
                        if (target == CellType.RampBT)
                            return true;
                        break;
                    case Direction.Left:
                        if (target == CellType.RampLR)
                            return true;
                        break;
                    case Direction.Right:
                        if (target == CellType.RampRL)
                            return true;
                        break;
                }
            }

            switch (start)
            {
                case CellType.RampBT:
                    if ((direction == Direction.Up && target == CellType.Hill) || 
                        (direction == Direction.Down && target == CellType.Plain))
                        return true;
                    break;
                case CellType.RampTB:
                    if ((direction == Direction.Up && target == CellType.Plain) || 
                        (direction == Direction.Down && target == CellType.Hill))
                        return true;
                    break;
                case CellType.RampLR:
                    if ((direction == Direction.Left && target == CellType.Plain) ||
                        (direction == Direction.Right && target == CellType.Hill))
                        return true;
                    break;
                case CellType.RampRL:
                    if ((direction == Direction.Left && target == CellType.Hill) ||
                        (direction == Direction.Right && target == CellType.Plain))
                        return true;
                    break;
            }

            return false;
        }
    }
}