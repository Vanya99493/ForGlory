using PlaygroundModule.ModelPart;

namespace PlaygroundModule.PresenterPart.WideSearchModule
{
    public class Node
    {
        public int HeightIndex;
        public int WidthIndex;
        public CellType CellType;
        public bool Visited;
        public bool IsBusy;
        public Node PrevNode;
        public int Distance;

        public Node(int heightIndex, int widthIndex, CellType cellType, bool isBusy)
        {
            HeightIndex = heightIndex;
            WidthIndex = widthIndex;
            CellType = cellType;
            Visited = false;
            IsBusy = isBusy;
            Distance = 0;
        }
        
        public static bool operator== (Node node1, Node node2) => 
            node1.HeightIndex == node2.HeightIndex && node1.WidthIndex == node2.WidthIndex;
        
        public static bool operator!= (Node node1, Node node2) => 
            node1.HeightIndex != node2.HeightIndex || node1.WidthIndex != node2.WidthIndex;
    }
}