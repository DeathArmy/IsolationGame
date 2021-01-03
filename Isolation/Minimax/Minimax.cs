using System;
using System.Linq;

namespace Minimax
{
    public class Minimax
    {   
        private Random _rand = new Random(DateTime.Now.Millisecond);

        public int Compute(Node node, int depth, bool maximizingPlayer)
        {
            if(depth == 0 || node.IsTerminal)
                return GetHeuristicValue(node);

            if(maximizingPlayer)
            {
                node.Value = int.MinValue;

                foreach(var child in node.Children)
                    node.Value = Max(node.Value, Compute(child, depth - 1, false));

                return node.Value;
            }
            else
            {
                node.Value = int.MaxValue;

                foreach(var child in node.Children)
                    node.Value = Min(node.Value, Compute(child, depth - 1, true));
                    
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

        public int GetHeuristicValue(Node node)
        {    
            int value = 0;

            value = _rand.Next();

            return value;
        }
    }
}