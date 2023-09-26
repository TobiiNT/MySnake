using System.Drawing;
using System.Windows.Forms;
using Bussiness;
using System;
using System.Collections.Generic;
using System.Threading;
using GameCore.Entities.Enums;
using GameCore.Entities;
using GameCore.Utilities;
using GameCore.Loaders;
using System.Linq;
using GameCore.Entities.Interfaces;
using GameCore.Events;

namespace MySnake
{
    public partial class frmMain : Form
    {
        public Graphics Graphic;

        public List<Obstacle> Obstacles;
        public List<Food> Foods;

        public Snake Player_Snake;
        public List<Snake> SnakeList;

        public Map Board;
        private object LockObject = new object();
        public System.Timers.Timer Timer;

        public frmMain()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.Graphic = this.MainPanel.CreateGraphics();
            this.Obstacles = new List<Obstacle>();
            this.Foods = new List<Food>();


            MapLoader MapLoader = new MapLoader(42, 42);
            this.Obstacles = MapLoader.Load("level1.lev").OfType<Obstacle>().ToList();

            Reset();
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
                this.Board.ChangeCellType(Food.Position, CellType.EMPTY);
                this.Foods.Remove(Food);
            }
        }
        private void AddNewFood()
        {
            int X, Y;

            do
            {
                X = Randomizer.Next(1, this.Board.GetWidth() - 2);
                Y = Randomizer.Next(1, this.Board.GetHeight() - 2);
            } while (!this.Board.IsCellAvailable(new Point(X, Y)));

            Food NewFood = new Food(X, Y);
            NewFood.OnDisposed += Food_OnDisposed;
            this.Foods.Add(NewFood);

            Render.Draw(this.Graphic, NewFood);
            this.Board.ChangeCellType(NewFood.Position, CellType.FOOD);
        }

        private void Reset()
        {
            this.Board = new Map(42, 42);

            this.SnakeList = new List<Snake>();

            if (this.Player_Snake != null)
            {
                this.Player_Snake.Dispose();
            }
            this.Player_Snake = new Snake("Player", this.Graphic, Get_Random_Position(), 100);
            this.Player_Snake.Snake_Control += new Snake.SnakeControl(SnakeLogic);

            if (this.SnakeList.Count > 0)
            {
                this.SnakeList.Clear();
            }
            for (int i = 0; i < 2; i++)
            {
                Snake newSnake = new Snake($"Computer {i}", this.Graphic, Get_Random_Position(), 100, true);
                newSnake.Snake_Control += new Snake.SnakeControl(SnakeLogic);
                newSnake.Snake_FindPath += new Snake.FindPath(Caculate_Path);
                this.Board.ChangeCellType(newSnake.Bodies, CellType.OBSTACLE);
                this.SnakeList.Add(newSnake);
                Thread.Sleep(2);
            }

            foreach (var Obstacle in this.Obstacles)
            {
                this.Board.ChangeCellType(Obstacle.Position, CellType.OBSTACLE);
            }
            this.Board.ChangeCellType(this.Player_Snake.Bodies, CellType.OBSTACLE);
            this.AddNewFood();
            this.AddNewFood();

            lock (this.LockObject)
            {
                this.Graphic.Clear(Color.White);
            }

            foreach (IGameObject Object in this.Obstacles)
            {
                Render.Draw(this.Graphic, Object);
            }
            this.Player_Snake.Draw();
            foreach (Snake snake in this.SnakeList)
            {
                snake.Draw();
            }
            foreach (IGameObject Food in this.Foods)
            {
                Render.Draw(this.Graphic, Food);
            }
        }

        public Point Get_Random_Position()
        {
            Point position = new Point(Randomizer.Next(2, this.Board.GetWidth() - 1), Randomizer.Next(2, this.Board.GetHeight() - 1));
            Thread.Sleep(1);
            if (!this.Board.IsCellAvailable(position))
                Get_Random_Position();
            return position;
        }

        public void SnakeLogic(Snake CurrentSnake, Point[] Position, Point Tail, bool IsPlayer)
        {
            this.Board.ChangeCellType(Position, CellType.OBSTACLE);
            this.Board.ChangeCellType(Tail, CellType.EMPTY);

            CellType temp = this.Board.GetCellType(Position[0]);

            if (temp == CellType.OBSTACLE) //đụng vật cản
            {
                CurrentSnake.Dispose();
            }
            else if (temp == CellType.FOOD)
            {
                foreach (Food Food in this.Foods.Where(f => f.Position == Position[0]))
                {
                    Food.Dispose();
                }
                CurrentSnake.AddLength();

                this.AddNewFood();
            }
        }

        private void MainPanel_Paint(object sender, PaintEventArgs e)
        {
            foreach (IGameObject Object in this.Obstacles)
            {
                Render.Draw(this.Graphic, Object);
            }
            foreach (IGameObject Object in this.Foods)
            {
                Render.Draw(this.Graphic, Object);
            }
            this.Player_Snake.Draw();
            foreach (Snake snake in this.SnakeList)
            {
                snake.Draw();
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            bool bHandled = false;
            switch (keyData)
            {
                case Keys.D:
                    if (this.Player_Snake.Movement != Direction.LEFT)
                    {
                        this.Player_Snake.Movement = Direction.RIGHT;
                        bHandled = true;
                    }

                    break;
                case Keys.A:
                    if (this.Player_Snake.Movement != Direction.RIGHT)
                    {
                        this.Player_Snake.Movement = Direction.LEFT;
                        bHandled = true;
                    }
                    break;
                case Keys.W:
                    if (this.Player_Snake.Movement != Direction.DOWN)
                    {
                        this.Player_Snake.Movement = Direction.UP;
                        bHandled = true;
                    }
                    break;
                case Keys.S:
                    if (this.Player_Snake.Movement != Direction.UP)
                    {
                        this.Player_Snake.Movement = Direction.DOWN;
                        bHandled = true;
                    }
                    break;
            }
            return bHandled;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.Player_Snake.IsMoving)
            {
                this.Player_Snake.UpdateMovingState(false);
            }
            else
            {
                this.Player_Snake.UpdateMovingState(true);
            }
        }

        public void UpdateStatus()
        {
            if (this.Player_Snake != null)
                this.lblPlayer_Score.Text = $"Điểm của người chơi: {this.Player_Snake.Length - Constants.Snake_Default_Size}";


            this.table_Score.Items.Clear();

            foreach (Snake snake in this.SnakeList)
            {
                if (!snake.isDisposed)
                {
                    string[] items = new string[3];
                    items[0] = snake.isDisposed.ToString();
                    items[1] = $"{snake.Length - Constants.Snake_Default_Size}";

                    this.table_Score.Items.Insert(this.table_Score.Items.Count, new ListViewItem(items));
                }
            }

        }

        private void MainTimer_Tick(object sender, EventArgs e)
        {
            this.UpdateStatus();
        }

        public void Caculate_Path(Snake Current_Snake)
        {
            BFSAlgorithm algorithm = new BFSAlgorithm(this.Board, Current_Snake.Bodies[1]);
            Current_Snake.Computer_Path = algorithm.FindPath();
            if (Current_Snake.Computer_Path == null)
            {
                Current_Snake.Dispose();
                //Current_Snake.MovingState(false);
                return;
            }
            Current_Snake.Computer_Path.RemoveAt(Current_Snake.Computer_Path.Count - 1);
            DrawPath(Current_Snake);
        }

        public void DrawPath(Snake snake)
        {
            for (int i = 1; i < snake.Computer_Path.Count; i++)
                Render.Draw(this.Graphic, new Point(snake.Computer_Path[i].X * 20, snake.Computer_Path[i].Y * 20), Pens.Orange, Brushes.Lime, 20, 5);
        }

        private void btn_Find_Path_Click(object sender, EventArgs e)
        {
            foreach (Snake snake in this.SnakeList)
            {
                this.Caculate_Path(snake);
                snake.UpdateMovingState(true);
            }
        }

        private void numberColumnAndRow_ValueChanged(object sender, EventArgs e)
        {
            this.Player_Snake.UpdateMoveSpeed((int)this.numberColumnAndRow.Value);
            foreach (Snake snake in this.SnakeList)
            {
                snake.UpdateMoveSpeed((int)this.numberColumnAndRow.Value);
            }
        }
    }
}
