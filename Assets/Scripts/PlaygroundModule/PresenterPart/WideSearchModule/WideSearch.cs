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

        public Dictionary<int, int> DividePlaygroundByZones(MoveNode[,] playgroundMoveNodes)
        {
            Dictionary<int, int> zones = new Dictionary<int, int>();
            int currentZoneIndex = 1;
            
            for (int i = 0; i < playgroundMoveNodes.GetLength(0); i++)
            {
                for (int j = 0; j < playgroundMoveNodes.GetLength(1); j++)
                {
                    if (playgroundMoveNodes[i, j].CellType == CellType.Water)
                    {
                        playgroundMoveNodes[i, j].Zone = -1;
                        continue;
                    }

                    if (playgroundMoveNodes[i, j].Zone == 0)
                    {
                        zones.Add(currentZoneIndex, 1);
                        
                        Queue<MoveNode> bfsQueue = new Queue<MoveNode>();
                        bfsQueue.Enqueue(playgroundMoveNodes[i, j]);
                        playgroundMoveNodes[i, j].Visited = true;
                        playgroundMoveNodes[i, j].Zone = currentZoneIndex;
                        
                        while (bfsQueue.Count > 0)
                        {
                            MoveNode currentMoveNode = bfsQueue.Dequeue();
                            
                            if (!IsExtreme(currentMoveNode, playgroundMoveNodes, Direction.Up) && 
                                !playgroundMoveNodes[currentMoveNode.HeightIndex - 1, currentMoveNode.WidthIndex].Visited && 
                                CanMove(
                                    playgroundMoveNodes[currentMoveNode.HeightIndex, currentMoveNode.WidthIndex],
                                    playgroundMoveNodes[currentMoveNode.HeightIndex - 1, currentMoveNode.WidthIndex],
                                    Direction.Up
                                ))
                            {
                                playgroundMoveNodes[currentMoveNode.HeightIndex - 1, currentMoveNode.WidthIndex].Visited = true;
                                playgroundMoveNodes[currentMoveNode.HeightIndex - 1, currentMoveNode.WidthIndex].Zone = currentZoneIndex;
                                zones[currentZoneIndex]++;
                                bfsQueue.Enqueue(playgroundMoveNodes[currentMoveNode.HeightIndex - 1, currentMoveNode.WidthIndex]);
                            }
                            if (!IsExtreme(currentMoveNode, playgroundMoveNodes, Direction.Down) && 
                                !playgroundMoveNodes[currentMoveNode.HeightIndex + 1, currentMoveNode.WidthIndex].Visited && 
                                CanMove(
                                    playgroundMoveNodes[currentMoveNode.HeightIndex, currentMoveNode.WidthIndex],
                                    playgroundMoveNodes[currentMoveNode.HeightIndex + 1, currentMoveNode.WidthIndex],
                                    Direction.Down
                                ))
                            {
                                playgroundMoveNodes[currentMoveNode.HeightIndex + 1, currentMoveNode.WidthIndex].Visited = true;
                                playgroundMoveNodes[currentMoveNode.HeightIndex + 1, currentMoveNode.WidthIndex].Zone = currentZoneIndex;
                                zones[currentZoneIndex]++;
                                bfsQueue.Enqueue(playgroundMoveNodes[currentMoveNode.HeightIndex + 1, currentMoveNode.WidthIndex]);
                            }
                            if (!IsExtreme(currentMoveNode, playgroundMoveNodes, Direction.Left) && 
                                !playgroundMoveNodes[currentMoveNode.HeightIndex, currentMoveNode.WidthIndex - 1].Visited && 
                                CanMove(
                                    playgroundMoveNodes[currentMoveNode.HeightIndex, currentMoveNode.WidthIndex],
                                    playgroundMoveNodes[currentMoveNode.HeightIndex, currentMoveNode.WidthIndex - 1],
                                    Direction.Left
                                ))
                            {
                                playgroundMoveNodes[currentMoveNode.HeightIndex, currentMoveNode.WidthIndex - 1].Visited = true;
                                playgroundMoveNodes[currentMoveNode.HeightIndex, currentMoveNode.WidthIndex - 1].Zone = currentZoneIndex;
                                zones[currentZoneIndex]++;
                                bfsQueue.Enqueue(playgroundMoveNodes[currentMoveNode.HeightIndex, currentMoveNode.WidthIndex - 1]);
                            }
                            if (!IsExtreme(currentMoveNode, playgroundMoveNodes, Direction.Right) && 
                                !playgroundMoveNodes[currentMoveNode.HeightIndex, currentMoveNode.WidthIndex + 1].Visited && 
                                CanMove(
                                    playgroundMoveNodes[currentMoveNode.HeightIndex, currentMoveNode.WidthIndex],
                                    playgroundMoveNodes[currentMoveNode.HeightIndex, currentMoveNode.WidthIndex + 1],
                                    Direction.Right
                                ))
                            {
                                playgroundMoveNodes[currentMoveNode.HeightIndex, currentMoveNode.WidthIndex + 1].Visited = true;
                                playgroundMoveNodes[currentMoveNode.HeightIndex, currentMoveNode.WidthIndex + 1].Zone = currentZoneIndex;
                                zones[currentZoneIndex]++;
                                bfsQueue.Enqueue(playgroundMoveNodes[currentMoveNode.HeightIndex, currentMoveNode.WidthIndex + 1]);
                            }
                        }

                        currentZoneIndex++;
                    }
                }
            }

            return zones;
        }
        
        public List<MoveNode> GetCellsByLength(int length, MoveNode startMoveNode, PlaygroundPresenter playgroundPresenter, bool considerCharacters, bool considerCastle)
        {
            List<MoveNode> cells = new List<MoveNode>();
            
            MoveNode[,] bfsArray = TranslatePlayground(playgroundPresenter);

            Queue<MoveNode> bfsQueue = new Queue<MoveNode>();
            bfsQueue.Enqueue(startMoveNode);
            bfsArray[startMoveNode.HeightIndex, startMoveNode.WidthIndex].Visited = true;
            cells.Add(bfsArray[startMoveNode.HeightIndex, startMoveNode.WidthIndex]);
            
            while (bfsQueue.Count > 0)
            {
                MoveNode currentMoveNode = bfsQueue.Dequeue();

                if (currentMoveNode.Distance >= length || 
                    (considerCastle && playgroundPresenter.Model
                        .GetCellPresenter(currentMoveNode.HeightIndex, currentMoveNode.WidthIndex).Model
                        .CheckCellOnCastle()))
                {
                    continue;
                }

                if (!IsExtreme(currentMoveNode, bfsArray, Direction.Up) && !bfsArray[currentMoveNode.HeightIndex - 1, currentMoveNode.WidthIndex].Visited && CanMove(
                        bfsArray[currentMoveNode.HeightIndex, currentMoveNode.WidthIndex],
                        bfsArray[currentMoveNode.HeightIndex - 1, currentMoveNode.WidthIndex],
                        Direction.Up,
                        considerCharacters
                    ))
                {
                    bfsArray[currentMoveNode.HeightIndex - 1, currentMoveNode.WidthIndex].Visited = true;
                    bfsArray[currentMoveNode.HeightIndex - 1, currentMoveNode.WidthIndex].Distance = currentMoveNode.Distance + 1;
                    cells.Add(bfsArray[currentMoveNode.HeightIndex - 1, currentMoveNode.WidthIndex]);
                    if (!bfsArray[currentMoveNode.HeightIndex - 1, currentMoveNode.WidthIndex].IsBusy)
                    {
                        bfsQueue.Enqueue(bfsArray[currentMoveNode.HeightIndex - 1, currentMoveNode.WidthIndex]);
                    }
                }
                if (!IsExtreme(currentMoveNode, bfsArray, Direction.Down) && !bfsArray[currentMoveNode.HeightIndex + 1, currentMoveNode.WidthIndex].Visited && CanMove(
                        bfsArray[currentMoveNode.HeightIndex, currentMoveNode.WidthIndex],
                        bfsArray[currentMoveNode.HeightIndex + 1, currentMoveNode.WidthIndex],
                        Direction.Down,
                        considerCharacters
                    ))
                {
                    bfsArray[currentMoveNode.HeightIndex + 1, currentMoveNode.WidthIndex].Visited = true;
                    bfsArray[currentMoveNode.HeightIndex + 1, currentMoveNode.WidthIndex].Distance = currentMoveNode.Distance + 1;
                    cells.Add(bfsArray[currentMoveNode.HeightIndex + 1, currentMoveNode.WidthIndex]);
                    if (!bfsArray[currentMoveNode.HeightIndex + 1, currentMoveNode.WidthIndex].IsBusy)
                    {
                        bfsQueue.Enqueue(bfsArray[currentMoveNode.HeightIndex + 1, currentMoveNode.WidthIndex]);
                    }
                }
                if (!IsExtreme(currentMoveNode, bfsArray, Direction.Left) && !bfsArray[currentMoveNode.HeightIndex, currentMoveNode.WidthIndex - 1].Visited && CanMove(
                        bfsArray[currentMoveNode.HeightIndex, currentMoveNode.WidthIndex],
                        bfsArray[currentMoveNode.HeightIndex, currentMoveNode.WidthIndex - 1],
                        Direction.Left,
                        considerCharacters
                    ))
                {
                    bfsArray[currentMoveNode.HeightIndex, currentMoveNode.WidthIndex - 1].Visited = true;
                    bfsArray[currentMoveNode.HeightIndex, currentMoveNode.WidthIndex - 1].Distance = currentMoveNode.Distance + 1;
                    cells.Add(bfsArray[currentMoveNode.HeightIndex, currentMoveNode.WidthIndex - 1]);
                    if (!bfsArray[currentMoveNode.HeightIndex, currentMoveNode.WidthIndex - 1].IsBusy)
                    {
                        bfsQueue.Enqueue(bfsArray[currentMoveNode.HeightIndex, currentMoveNode.WidthIndex - 1]);
                    }
                }
                if (!IsExtreme(currentMoveNode, bfsArray, Direction.Right) && !bfsArray[currentMoveNode.HeightIndex, currentMoveNode.WidthIndex + 1].Visited && CanMove(
                        bfsArray[currentMoveNode.HeightIndex, currentMoveNode.WidthIndex],
                        bfsArray[currentMoveNode.HeightIndex, currentMoveNode.WidthIndex + 1],
                        Direction.Right,
                        considerCharacters
                    ))
                {
                    bfsArray[currentMoveNode.HeightIndex, currentMoveNode.WidthIndex + 1].Visited = true;
                    bfsArray[currentMoveNode.HeightIndex, currentMoveNode.WidthIndex + 1].Distance = currentMoveNode.Distance + 1;
                    cells.Add(bfsArray[currentMoveNode.HeightIndex, currentMoveNode.WidthIndex + 1]);
                    if (!bfsArray[currentMoveNode.HeightIndex, currentMoveNode.WidthIndex + 1].IsBusy)
                    {
                        bfsQueue.Enqueue(bfsArray[currentMoveNode.HeightIndex, currentMoveNode.WidthIndex + 1]);
                    }
                }
            }

            return cells;
        }
        
        public bool TryBuildRoute(MoveNode startMoveNode, MoveNode targetMoveNode, PlaygroundPresenter playgroundPresenter, bool considerCharacters, bool considerCastle, out List<Pair<int, int>> route)
        {
            if (targetMoveNode.CellType == CellType.Water)
            {
                route = new List<Pair<int, int>>();
                return false;
            }
            
            MoveNode[,] bfsArray = TranslatePlayground(playgroundPresenter);

            Queue<MoveNode> bfsQueue = new Queue<MoveNode>();
            bfsQueue.Enqueue(startMoveNode);
            bfsArray[startMoveNode.HeightIndex, startMoveNode.WidthIndex].Visited = true;
            
            while (bfsQueue.Count > 0)
            {
                MoveNode currentMoveNode = bfsQueue.Dequeue();

                if (considerCastle && playgroundPresenter.Model
                        .GetCellPresenter(currentMoveNode.HeightIndex, currentMoveNode.WidthIndex).Model
                        .CheckCellOnCastle())
                {
                    continue;
                }
                
                if (currentMoveNode == targetMoveNode)
                {
                    route = BuildRoute(startMoveNode, currentMoveNode);
                    return true;
                }

                if (!IsExtreme(currentMoveNode, bfsArray, Direction.Up) && !bfsArray[currentMoveNode.HeightIndex - 1, currentMoveNode.WidthIndex].Visited && CanMove(
                        bfsArray[currentMoveNode.HeightIndex, currentMoveNode.WidthIndex],
                        bfsArray[currentMoveNode.HeightIndex - 1, currentMoveNode.WidthIndex],
                        targetMoveNode,
                        Direction.Up,
                        considerCharacters
                    ))
                {
                    bfsArray[currentMoveNode.HeightIndex - 1, currentMoveNode.WidthIndex].Visited = true;
                    bfsArray[currentMoveNode.HeightIndex - 1, currentMoveNode.WidthIndex].PrevMoveNode = currentMoveNode;
                    bfsQueue.Enqueue(bfsArray[currentMoveNode.HeightIndex - 1, currentMoveNode.WidthIndex]);
                }
                if (!IsExtreme(currentMoveNode, bfsArray, Direction.Down) && !bfsArray[currentMoveNode.HeightIndex + 1, currentMoveNode.WidthIndex].Visited && CanMove(
                        bfsArray[currentMoveNode.HeightIndex, currentMoveNode.WidthIndex],
                        bfsArray[currentMoveNode.HeightIndex + 1, currentMoveNode.WidthIndex],
                        targetMoveNode,
                        Direction.Down,
                        considerCharacters
                    ))
                {
                    bfsArray[currentMoveNode.HeightIndex + 1, currentMoveNode.WidthIndex].Visited = true;
                    bfsArray[currentMoveNode.HeightIndex + 1, currentMoveNode.WidthIndex].PrevMoveNode = currentMoveNode;
                    bfsQueue.Enqueue(bfsArray[currentMoveNode.HeightIndex + 1, currentMoveNode.WidthIndex]);
                }
                if (!IsExtreme(currentMoveNode, bfsArray, Direction.Left) && !bfsArray[currentMoveNode.HeightIndex, currentMoveNode.WidthIndex - 1].Visited && CanMove(
                        bfsArray[currentMoveNode.HeightIndex, currentMoveNode.WidthIndex],
                        bfsArray[currentMoveNode.HeightIndex, currentMoveNode.WidthIndex - 1],
                        targetMoveNode,
                        Direction.Left,
                        considerCharacters
                    ))
                {
                    bfsArray[currentMoveNode.HeightIndex, currentMoveNode.WidthIndex - 1].Visited = true;
                    bfsArray[currentMoveNode.HeightIndex, currentMoveNode.WidthIndex - 1].PrevMoveNode = currentMoveNode;
                    bfsQueue.Enqueue(bfsArray[currentMoveNode.HeightIndex, currentMoveNode.WidthIndex - 1]);
                }
                if (!IsExtreme(currentMoveNode, bfsArray, Direction.Right) && !bfsArray[currentMoveNode.HeightIndex, currentMoveNode.WidthIndex + 1].Visited && CanMove(
                        bfsArray[currentMoveNode.HeightIndex, currentMoveNode.WidthIndex],
                        bfsArray[currentMoveNode.HeightIndex, currentMoveNode.WidthIndex + 1],
                        targetMoveNode,
                        Direction.Right,
                        considerCharacters
                    ))
                {
                    bfsArray[currentMoveNode.HeightIndex, currentMoveNode.WidthIndex + 1].Visited = true;
                    bfsArray[currentMoveNode.HeightIndex, currentMoveNode.WidthIndex + 1].PrevMoveNode = currentMoveNode;
                    bfsQueue.Enqueue(bfsArray[currentMoveNode.HeightIndex, currentMoveNode.WidthIndex + 1]);
                }
            }

            route = new List<Pair<int, int>>();
            return false;
        }

        private MoveNode[,] TranslatePlayground(PlaygroundPresenter playgroundPresenter)
        {
            MoveNode[,] bfsArray = new MoveNode[playgroundPresenter.Model.PlaygroundHeight, playgroundPresenter.Model.PlaygroundWidth];

            for (int i = 0; i < bfsArray.GetLength(0); i++)
            {
                for (int j = 0; j < bfsArray.GetLength(1); j++)
                {
                    bfsArray[i, j] = new MoveNode(i, j, 
                        playgroundPresenter.Model.GetCellPresenter(i, j).Model.CellType, 
                        playgroundPresenter.CheckCellOnCharacter(i, j));
                }
            }

            return bfsArray;
        }

        private bool IsExtreme(MoveNode moveNode, MoveNode[,] bfsArray, Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    if (moveNode.HeightIndex == 0)
                        return true;
                    break;
                case Direction.Down:
                    if (moveNode.HeightIndex == bfsArray.GetLength(0) - 1)
                        return true;
                    break;
                case Direction.Left:
                    if (moveNode.WidthIndex == 0)
                        return true;
                    break;
                case Direction.Right:
                    if (moveNode.WidthIndex == bfsArray.GetLength(1) - 1)
                        return true; 
                    break;
            }

            return false;
        }

        private List<Pair<int, int>> BuildRoute(MoveNode startMoveNode, MoveNode lastRouteMoveNode)
        {
            List<Pair<int, int>> route = new List<Pair<int, int>>();

            MoveNode currentMoveNode = lastRouteMoveNode;

            while (currentMoveNode != startMoveNode)
            {
                route.Add(new Pair<int, int>(currentMoveNode.HeightIndex, currentMoveNode.WidthIndex));
                currentMoveNode = currentMoveNode.PrevMoveNode;
            }
            
            route.Reverse();
            
            return route;
        }

        private bool CanMove(MoveNode start, MoveNode target, Direction direction)
        {
            return _cellDataProvider.GetCellPixelsMoveRoots(start.CellType)[direction].Contains(target.CellType);
        }
        
        private bool CanMove(MoveNode start, MoveNode target, Direction direction, bool considerCharacters)
        {
            return _cellDataProvider.GetCellPixelsMoveRoots(start.CellType)[direction].Contains(target.CellType) && 
                   (!target.IsBusy || !considerCharacters);
        }

        private bool CanMove(MoveNode start, MoveNode target, MoveNode finalTarget, Direction direction, bool considerCharacters)
        {
            return _cellDataProvider.GetCellPixelsMoveRoots(start.CellType)[direction].Contains(target.CellType) && 
                   (!target.IsBusy || (!considerCharacters && target == finalTarget));
        }
    }
}