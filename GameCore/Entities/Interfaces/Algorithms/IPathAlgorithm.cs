using GameCore.Entities.Interfaces.Games;
using System.Collections.Generic;
using System.Drawing;

namespace GameCore.Entities.Interfaces.Algorithms
{
    public interface IPathAlgorithm
    {
        IMatrix Matrix { get; }
        List<Point> Goals { get; }
        List<Point> FindPath(Point StartPosition);
    }
}
