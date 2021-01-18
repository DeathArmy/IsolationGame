using System.Collections.Generic;
using System.Linq;

namespace Minimax
{
    public class GameTree : IGameTree
    {
        private readonly char[,] _gameBoard;

        public GameTree(char[,] gameBoard)
        {
            _gameBoard = gameBoard;
        }

        public Node CreateTree(bool maximizingPlayer, int maxDepth, Node node)
        {
            if(maxDepth == 0)
                return node;
            
            if(node == null)
            {
                node = CreateLevel(maximizingPlayer);
                maxDepth -= 1;
            }

            if(node.Children == null)
                node = CreateLevel(maximizingPlayer);

            for(int i = 0; i < node.Children.Count; i++)
            {
                maximizingPlayer = !maximizingPlayer;
                var childrenGameState = new GameTree(node.Children[i].GameBoardState);
                node.Children[i] = childrenGameState.CreateTree(maximizingPlayer, maxDepth - 1, node.Children[i]);
            }

            return node;
        }

        public Node CreateLevel(bool maximizingPlayer)
        {
            var tree = new Node();
            char pawn = maximizingPlayer ? 'w' : 'b';

            tree.Coords = GetCoords(pawn).First();
            tree.Depth = 0;
            tree.GameBoardState = _gameBoard;
            tree.Children = SetChildren(tree, -1);

            return tree;
        }

        private List<Node> SetChildren(Node parent, int maxDepth)
        {
            if(parent.Depth <= maxDepth)
                return null;

            var children = new List<Node>();
            var neighboursCoords = GetNeighboursCoords(parent.Coords);
            var emptyFields = GetCoords('e');

            foreach(var n in neighboursCoords)
            {
                var boardState = (char[,])parent.GameBoardState.Clone();
                boardState[n.Y, n.X] = boardState[parent.Coords.Y, parent.Coords.X];
                boardState[parent.Coords.Y, parent.Coords.X] = 'e';

                var node = new Node
                    {
                        Parent = parent,
                        Coords = (n.X, n.Y),
                        Depth = parent.Depth - 1,
                        ActionType = ActionType.Move,
                        GameBoardState = boardState
                    };
                node.Children = SetChildren(node, maxDepth);

                children.Add(node);
            }

            foreach(var n in emptyFields)
            {
                var boardState = (char[,])parent.GameBoardState.Clone();
                boardState[n.Y, n.X] = 'd';

                var node = new Node
                    {
                        Parent = parent,
                        Coords = (n.X, n.Y),
                        Depth = parent.Depth - 1,
                        ActionType = ActionType.Block,
                        GameBoardState = boardState
                    };
                node.Children = SetChildren(node, maxDepth);

                children.Add(node);
            }

            return children;
        }

        public List<(int X, int Y)> GetCoords(char pawn)
        {
            var result = new List<(int X, int Y)>();

            for(int y = 0; y < _gameBoard.GetLength(0); y++)
                for(int x = 0; x < _gameBoard.GetLength(1); x++)
                    if(_gameBoard[y,x] == pawn)
                        result.Add((X: x, Y: y));
                    
            return result;
        }

        public List<(int X, int Y)> GetNeighboursCoords((int X, int Y) center)
        {
            var uncheckedResult = new List<(int X, int Y)>();
            var result = new List<(int X, int Y)>();

            for(int i = -1; i < 2; i++)
                for(int j = -1; j < 2; j++)
                {
                    if(i == 0 && j == 0)
                        continue;

                    uncheckedResult.Add((center.X + j, center.Y + i));
                }

            // remove out out of boundary indexes
            for(int i = 0; i < 8; i++)
                if(InRange(uncheckedResult[i].X) && 
                    InRange(uncheckedResult[i].Y) && 
                    _gameBoard[uncheckedResult[i].Y, uncheckedResult[i].X] == 'e')
                        result.Add(uncheckedResult[i]);


            return result;
        }

        private bool InRange(int i) => i >= 0 && i < _gameBoard.GetLength(1);
    }
}