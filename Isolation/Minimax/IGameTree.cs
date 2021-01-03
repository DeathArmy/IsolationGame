using System.Collections.Generic;

namespace Minimax
{
    public interface IGameTree
    {
        Node CreateLevel(bool maximizingPlayer);
    }
}