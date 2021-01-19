using System;
using System.Collections.Generic;
using System.Linq;

namespace Minimax
{
    public class Minimax
    {   
        private Random _rand = new Random(DateTime.Now.Millisecond);

        public int Compute(Node node, int depth, int a, int b, bool maximizingPlayer)
        {
            if (depth == 0 || node.IsTerminal)
                return GetHeuristicValue(node, maximizingPlayer);

            if (maximizingPlayer)
            {
                node.Value = int.MinValue;

                foreach (var child in node.Children)
                    node.Value = Max(node.Value, Compute(child, depth - 1, a, b, false));

                a = Max(a, node.Value);
                if (a > b)
                    node.Children.Clear();

                return node.Value;
            }
            else
            {
                node.Value = int.MaxValue;

                foreach (var child in node.Children)
                    node.Value = Min(node.Value, Compute(child, depth - 1, a, b, true));

                b = Min(b, node.Value);
                if (b < a)
                    node.Children.Clear(); ;

                return node.Value;
            }
        }

        public int Min(int value, int childValue)
        {
            return value > childValue ? childValue : value;
        }

        public int Max(int value, int childValue)
        {
            return value > childValue ? value : childValue;
        }

        public int GetHeuristicValue(Node node, bool maximizingPlayer)
        {
            int score = 0;
            int moveScore = 0;
            int blockScore = 0;
            int parentMoveScore = 0;
            int parentBlockScore = 0;
            char fieldState = node.GameBoardState[node.Coords.Y, node.Coords.X];

            parentMoveScore = GetNextMoveCount(node.Parent.Coords, node.Parent.GameBoardState).Count;
            moveScore = GetNextMoveCount(node.Coords, node.GameBoardState).Count;

            blockScore = GetNextMoveCount(
                GetCoords(!maximizingPlayer ? 'w' : 'b', node.GameBoardState)
                    .FirstOrDefault(),
                node.GameBoardState).Count;
            parentBlockScore = GetNextMoveCount(
                GetCoords(!maximizingPlayer ? 'w' : 'b', node.Parent.GameBoardState)
                    .FirstOrDefault(),
                node.Parent.GameBoardState).Count;

            //score = moveScore - parentMoveScore + parentBlockScore - blockScore; 
            //score = ((8 - moveScore) * 5 + parentMoveScore * 10) + (8 - blockScore) * 10 + 5; 
            score = (moveScore * 10 + parentMoveScore * 8) + (8 - blockScore) * 13; 

            return score;
        }

        private List<(int X, int Y)> GetCoords(char pawn, char[,] gameBoard)
        {
            var result = new List<(int X, int Y)>();

            for (int y = 0; y < gameBoard.GetLength(0); y++)
                for (int x = 0; x < gameBoard.GetLength(1); x++)
                    if (gameBoard[y, x] == pawn)
                        result.Add((X: x, Y: y));

            return result;
        }

        private List<(int X, int Y)> GetNextMoveCount((int X, int Y) center, char[,] gameBoard)
        {
            var uncheckedResult = new List<(int X, int Y)>();
            var result = new List<(int X, int Y)>();

            for (int i = -1; i < 2; i++)
                for (int j = -1; j < 2; j++)
                {
                    if (i == 0 && j == 0)
                        continue;

                    uncheckedResult.Add((center.X + j, center.Y + i));
                }

            // remove out out of boundary indexes
            for (int i = 0; i < 8; i++)
                if (InRange(uncheckedResult[i].X, gameBoard) &&
                    InRange(uncheckedResult[i].Y, gameBoard) &&
                    gameBoard[uncheckedResult[i].Y, uncheckedResult[i].X] == 'e')
                    result.Add(uncheckedResult[i]);

            return result;
        }

        private bool InRange(int i, char[,] gameBoard) => i >= 0 && i < gameBoard.GetLength(1);
    }
}