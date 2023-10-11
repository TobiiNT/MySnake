using GameCore.Entities.Enums;
using GameCore.Entities.Implements.Snakes;
using GameCore.Entities.Interfaces.Controllers;
using GameCore.Entities.Interfaces.Games;
using GameCore.Entities.Interfaces.Snakes;
using GameCore.Events;
using GameCore.Loaders;
using GameCore.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;

namespace GameCore.Entities.Implements.Games
{
    public class Map : IMatrix, IDisposable
    {
        private MapLoader MapLoader { set; get; }
        public int Width { get; }
        public int Height { get; }
        private CellType[,] Matrix { set; get; }
        public List<Obstacle> Obstacles { private set; get; }
        public List<IFood> Foods { private set; get; }

        public List<Snake> SnakeList { private set; get; }
        private Thread MainThread { set; get; }

        public CellType[,] GetMatrix() => Matrix;

        public Map(int Rows, int Columns)
        {
            this.Width = Rows;
            this.Height = Columns;
            this.Matrix = new CellType[Rows, Columns];

            this.Obstacles = new List<Obstacle>();
            this.Foods = new List<IFood>();
            this.SnakeList = new List<Snake>();

            this.MapLoader = new MapLoader(Rows, Columns);

            this.MainThread = new Thread(new ThreadStart(this.GameLoop));
            this.MainThread.Start();
        }

        private void GameLoop()
        {
            while (true)
            {
                foreach (var Snake in this.SnakeList.ToList())
                {
                    Snake.Move(this);
                }

                Thread.Sleep(10);
            }
        }

        public void LoadMap(string MapName)
        {
            this.Obstacles = MapLoader.Load(MapName).OfType<Obstacle>().ToList();

            foreach (var Obstacle in this.Obstacles)
            {
                this.ChangeCell(Obstacle.Position, CellType.OBSTACLE);
            }
        }

        public void ChangeCells(List<Point> Cells, CellType Type)
        {
            foreach (Point Location in Cells)
            {
                SetCellValue(Location, Type);
            }
        }

        public void ChangeCell(Point Cell, CellType Type)
        {
            SetCellValue(Cell, Type);
        }

        public bool IsCellAvailable(Point Cell)
        {
            return GetCellType(Cell) == CellType.EMPTY;
        }
        public CellType GetCellType(Point Cell)
        {
            if (Cell == null || Cell.X >= this.Width || Cell.Y >= this.Height)
                return CellType.OBSTACLE;
            return Matrix[Cell.X, Cell.Y];
        }

        private void SetCellValue(Point Cell, CellType Type)
        {
            if (Cell == null || Cell.X < 0 || Cell.Y < 0 || Cell.X >= this.Width || Cell.Y >= this.Height)
                throw new IndexOutOfRangeException();

            Matrix[Cell.X, Cell.Y] = Type;
        }

        private void SnakeEatFood(ISnake Snake, IFood Food)
        {
            if (this.Foods.Contains(Food))
            {
                Food.ApplyEffect(Snake);
                Food.Dispose();
                this.ChangeCell(Food.Position, CellType.EMPTY);
                this.Foods.Remove(Food);
            }
        }

        public void AddNewFood()
        {
            int X, Y;

            do
            {
                X = Randomizer.Next(1, this.Width - 2);
                Y = Randomizer.Next(1, this.Height - 2);
            } while (!this.IsCellAvailable(new Point(X, Y)));

            Food NewFood = new Food(X, Y);
            this.Foods.Add(NewFood);

            this.ChangeCell(NewFood.Position, CellType.FOOD);
        }

        public void ResetSnakes()
        {
            this.SnakeList.ForEach(s => s.Dispose());
            this.SnakeList.Clear();
        }

        public Snake NewSnake(Point Position, Direction Direction, Color BodyColor, ISnakeController Controller)
        {
            Snake NewSnake = new Snake(Position.X, Position.Y, Direction, 3, 100, 500);
            NewSnake.SetColor(Color.Black, BodyColor);
            NewSnake.SetController(Controller);

            this.ChangeCells(NewSnake.Bodies.Select(i => i.Position).ToList(), CellType.OBSTACLE);
            this.SnakeList.Add(NewSnake);
            //NewSnake.OnDisposed += NewSnake_OnDisposed;
            NewSnake.OnMoving += NewSnake_OnSnakeMoving;
            NewSnake.OnDirectionChanged += NewSnake_OnSnakeDirectionChanged;
            NewSnake.OnLengthChanged += NewSnake_OnSnakeLengthChanged;
            NewSnake.OnDied += NewSnake_OnSnakeDied;
            return NewSnake;
        }

        private void NewSnake_OnSnakeDirectionChanged(object sender, EventArgs e)
        {
            if (e is OnSnakeDirectionChanged DirectionChangedEvent)
            {
                
            }
        }

        private void NewSnake_OnSnakeDied(object sender, EventArgs e)
        {
            if (e is OnSnakeDied DiedEvent)
            {
                this.ChangeCells(DiedEvent.Snake.Bodies.Select(i => i.Position).ToList(), CellType.EMPTY);
            }
        }

        private void NewSnake_OnSnakeLengthChanged(object sender, EventArgs e)
        {
            if (e is OnSnakeLengthChanged LengthChangedEvent)
            {
                
            }
        }

        private void NewSnake_OnSnakeMoving(object sender, EventArgs e)
        {
            if (e is OnSnakeMoving MoveEvent)
            {
                ISnake CurrentSnake = MoveEvent.Snake;
                
                // When the snake moves, decrease its health by 1.
                CurrentSnake.Health.Decrease(1);
              
                CellType HeadPosition = GetCellType(CurrentSnake.Head.Position);

                ChangeCell(MoveEvent.LastTailPosition, CellType.EMPTY);
                ChangeCells(CurrentSnake.Bodies.Select(i => i.Position).ToList(), CellType.OBSTACLE);

                bool IsCollide = HeadPosition == CellType.OBSTACLE;
                bool IsDead = CurrentSnake.Health.RemainHealth <= 0;

                if (HeadPosition == CellType.FOOD)
                {
                    foreach (IFood Food in Foods.Where(f => f.Position == CurrentSnake.Head.Position).ToList())
                    {
                        SnakeEatFood(CurrentSnake, Food);
                        AddNewFood();
                    }
                }
                else if (IsDead || IsCollide)
                {
                    CurrentSnake.Die();
                    if (IsCollide) ChangeCell(CurrentSnake.Head.Position, CellType.OBSTACLE);
                }
            }
        }

        public Point GetSnakeStartPosition(int SnakeLength, Direction Direction)
        {
            Point StartPosition;

            while (true) // Keep searching until a valid position is found.
            {
                StartPosition = new Point(Randomizer.Next(1, this.Width - 1), Randomizer.Next(1, this.Height - 1));

                if (IsCellAvailable(StartPosition) && HasEnoughSpace(StartPosition, Direction, SnakeLength))
                    break;

                Thread.Sleep(1);
            }

            return StartPosition;
        }

        private bool HasEnoughSpace(Point Position, Direction Direction, int StartLength)
        {
            for (int i = 1; i <= StartLength; i++)
            {
                switch (Direction)
                {
                    case Direction.LEFT: Position.X++; break;
                    case Direction.RIGHT: Position.X--; break;
                    case Direction.UP: Position.Y++; break;
                    case Direction.DOWN: Position.Y--; break;
                }

                if (Position.X < 0 || Position.X >= this.Width || Position.Y < 0 || Position.Y >= this.Height)
                    return false;

                if (!IsCellAvailable(Position))
                    return false;
            }

            return true;
        }

        public void Dispose()
        {
            this.MainThread?.Abort();
            this.MainThread = null;
        }
    }
}
