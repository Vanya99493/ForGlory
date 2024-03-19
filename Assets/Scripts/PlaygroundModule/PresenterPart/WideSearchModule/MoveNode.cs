using PlaygroundModule.ModelPart;

namespace PlaygroundModule.PresenterPart.WideSearchModule
{
    public class MoveNode
    {
        public int HeightIndex;
        public int WidthIndex;
        public CellType CellType;
        public bool Visited;
        public bool IsBusy;
        public MoveNode PrevMoveNode;
        public int Distance;
        public int Zone;

        public MoveNode(int heightIndex, int widthIndex, CellType cellType, bool isBusy)
        {
            HeightIndex = heightIndex;
            WidthIndex = widthIndex;
            CellType = cellType;
            Visited = false;
            IsBusy = isBusy;
            Distance = 0;
            Zone = 0;
        }
        
        public static bool operator== (MoveNode node1, MoveNode node2) => 
            node1.HeightIndex == node2.HeightIndex && node1.WidthIndex == node2.WidthIndex;
        
        public static bool operator!= (MoveNode node1, MoveNode node2) => 
            node1.HeightIndex != node2.HeightIndex || node1.WidthIndex != node2.WidthIndex;
    }
}