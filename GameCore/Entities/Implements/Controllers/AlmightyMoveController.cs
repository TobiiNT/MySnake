using GameCore.Entities.Enums;
using GameCore.Entities.Implements.Algorithms;
using GameCore.Entities.Implements.Games;
using GameCore.Entities.Implements.Snakes;
using GameCore.Entities.Interfaces.Algorithms;
using GameCore.Entities.Interfaces.Controllers;
using System.Collections.Generic;
using System.Drawing;

namespace GameCore.Entities.Implements.Controllers
{
    public class AlmightyMoveController : ISnakeController, IHasPathAlgorithm
    {
        public Map Board { private set; get; }
        public IPathAlgorithm PathAlgorithm { get;}

        public AlmightyMoveController(Map Board)
        {
            this.Board = Board;
            this.PathAlgorithm = new AlmightyMoveAlgorithm(Board);
        }

        public Direction GetNextMove(Snake Snake)
        {
            List<Point> ShortestPath = PathAlgorithm.FindPath(Snake.Head.Position);

            // Have a path && the next point is not the head
            if (ShortestPath != null && ShortestPath.Count == 1)
            {
                Point NextStep = ShortestPath[0];

                if (Snake.Head.Position.X > NextStep.X)
                    return Direction.LEFT;
                else if (Snake.Head.Position.X < NextStep.X)
                    return Direction.RIGHT;
                if (Snake.Head.Position.Y > NextStep.Y)
                    return Direction.UP;
                else if (Snake.Head.Position.Y < NextStep.Y)
                    return Direction.DOWN;
            }
            return Snake.Direction;
        }
    }
}
