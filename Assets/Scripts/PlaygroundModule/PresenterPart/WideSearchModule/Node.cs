using PlaygroundModule.ModelPart;

namespace PlaygroundModule.PresenterPart.WideSearchModule
{
    public class Node
    {
        public int HeightIndex;
        public int WidthIndex;
        public CellType CellType;
        public bool Visited;
        public Node PrevNode;

        public Node(int heightIndex, int widthIndex, CellType cellType)
        {
            HeightIndex = heightIndex;
            WidthIndex = widthIndex;
            CellType = cellType;
            Visited = false;
        }
        
        public static bool operator== (Node node1, Node node2) => 
            node1.HeightIndex == node2.HeightIndex && node1.WidthIndex == node2.WidthIndex;
        
        public static bool operator!= (Node node1, Node node2) => 
            node1.HeightIndex != node2.HeightIndex && node1.WidthIndex != node2.WidthIndex;
    }
}