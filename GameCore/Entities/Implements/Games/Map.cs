﻿using GameCore.Entities.Enums;
using GameCore.Entities.Implements.Snakes;
using GameCore.Entities.Interfaces;
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
    public class Map
    {
        private MapLoader MapLoader { set; get; }
        private CellType[,] Matrix { set; get; }
        public List<Obstacle> Obstacles { private set; get; }
        public List<Food> Foods { private set; get; }

        public List<Snake> SnakeList { private set; get; }
        
        private Thread MainThread { set; get; }

        private const int SLEEP_DURATION = 100;

        public Map(int Rows, int Columns)
        {
            this.Matrix = new CellType[Rows, Columns];

            this.Obstacles = new List<Obstacle>();
            this.Foods = new List<Food>();
            this.SnakeList = new List<Snake>();

            this.MapLoader = new MapLoader(Rows, Columns);

            this.MainThread = new Thread(new ThreadStart(this.GameLoop));
            this.MainThread.Start();
        }

        private void GameLoop()
        {
            while (true)
            {
                foreach (var Snake in this.SnakeList)
                {
                    Snake.Move();
                }

                Thread.Sleep(SLEEP_DURATION);
            }
        }

        public void LoadMap(string MapName)
        {
            this.Obstacles = MapLoader.Load(MapName).OfType<Obstacle>().ToList();

            foreach (var Obstacle in this.Obstacles)
            {
                this.ChangeCellType(Obstacle.Position, CellType.OBSTACLE);
            }
        }

        public void ChangeCellsType(List<Point> Cells, CellType Type)
        {
            foreach (Point Location in Cells)
            {
                SetCellValue(Location, Type);
            }
        }

        public void ChangeCellType(Point Cell, CellType Type)
        {
            SetCellValue(Cell, Type);
        }

        public bool IsCellAvailable(Point Cell)
        {
            return GetCellType(Cell) == CellType.EMPTY;
        }
        public CellType GetCellType(Point Cell)
        {
            if (Cell == null || Cell.X >= Matrix.GetLength(0) || Cell.Y >= Matrix.GetLength(1))
                return CellType.OBSTACLE;
            return Matrix[Cell.X, Cell.Y];
        }

        public int GetWidth() => Matrix.GetLength(0);
        public int GetHeight() => Matrix.GetLength(1);


        private void SetCellValue(Point Cell, CellType Type)
        {
            if (Cell == null || Cell.X >= Matrix.GetLength(0) || Cell.Y >= Matrix.GetLength(1))
                throw new IndexOutOfRangeException();

            Matrix[Cell.X, Cell.Y] = Type;
        }

        private void Food_OnDisposed(object sender, EventArgs e)
        {
            if (e is OnFoodEaten)
            {
                this.DestroyFood((Food)sender);
            }
        }
        private void DestroyFood(Food Food)
        {
            if (this.Foods.Contains(Food))
            {
                this.ChangeCellType(Food.Position, CellType.EMPTY);
                this.Foods.Remove(Food);
            }
        }

        public void AddNewFood()
        {
            int X, Y;

            do
            {
                X = Randomizer.Next(1, this.GetWidth() - 2);
                Y = Randomizer.Next(1, this.GetHeight() - 2);
            } while (!this.IsCellAvailable(new Point(X, Y)));

            Food NewFood = new Food(X, Y);
            NewFood.OnDisposed += Food_OnDisposed;
            this.Foods.Add(NewFood);

            this.ChangeCellType(NewFood.Position, CellType.FOOD);
        }

        public void ResetSnakes()
        {
            this.SnakeList.ForEach(s => s.Dispose());
            this.SnakeList.Clear();
        }

        public Snake NewSnake(Point Position, Color BodyColor, ISnakeController Controller)
        {
            Snake NewSnake = new Snake(Position.X, Position.Y, Direction.RIGHT, 3);
            NewSnake.SetColor(Color.Black, BodyColor);
            NewSnake.SetController(Controller);

            this.SnakeList.Add(NewSnake);
            //NewSnake.OnDisposed += NewSnake_OnDisposed;
            //NewSnake.OnSnakeMoved += NewSnake_OnSnakeMoved;
            //NewSnake.OnSnakeLengthChanged += NewSnake_OnSnakeLengthChanged;
            return NewSnake;
        }
    }
}