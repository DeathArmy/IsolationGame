using System.Collections.Generic;

namespace Minimax
{
    public enum ActionType { Move, Block }
    public class Node
    {
        public int Id { get; set; }
        public bool IsTerminal { get; set; }
        public int Depth { get; set; }
        public (int X, int Y) Coords { get; set; }
        public int Value { get; set; }
        public ActionType ActionType { get; set; }
        public char[,] GameBoardState { get; set; }
        public Node Parent { get; set; }
        public List<Node> Children { get; set; }
    }
}