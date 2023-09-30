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
    public class AStarController : ISnakeController, IHasPathAlgorithm
    {
        public Map Board { private set; get; }
        public IPathAlgorithm PathAlgorithm { get; }

        public AStarController(Map Board)
        {
            this.Board = Board;
            this.PathAlgorithm = new AStarAlgorithm(Board, CheckMoveablePosition, CheckGoalPosition, CheckAvoidPosition);
        }

        public Direction GetNextMove(Snake Snake)
        {
            List<Point> ShortestPath = PathAlgorithm.FindPath(Snake.Head.Position);
           
            // Have a path && the next point is not the head
            if (ShortestPath != null && ShortestPath.Count > 0)
            {
                Point NextStep = ShortestPath[ShortestPath.Count - 1];

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

        private bool CheckMoveablePosition(Point Position)
        {
            // Check if the position is empty or food
            CellType CellType = Board.GetCellType(Position);
            return (CellType == CellType.EMPTY || CellType == CellType.FOOD);
        }

        private bool CheckGoalPosition(Point Position)
        {
            // Check if the position is food
            return Board.GetCellType(Position) == CellType.FOOD;
        }

        private bool CheckAvoidPosition(Point Position)
        {
            // Check if the position is empty or food
            return Board.GetCellType(Position) != CellType.EMPTY;
        }
    }
}
