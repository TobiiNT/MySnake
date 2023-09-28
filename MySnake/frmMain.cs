using System.Drawing;
using System.Windows.Forms;
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

        public Snake PlayerSnake;
        public List<Snake> SnakeList;

        public Map Board;
        private object LockObject = new object();

        private Thread MainThread;

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

            this.MainThread = new Thread(new ThreadStart(this.GameLoop));
            this.MainThread.Start();
        }

        private void GameLoop()
        {
            while (true)
            {
                this.PlayerSnake?.Move();
                this.DrawSnake(PlayerSnake);
                foreach (var Bot in SnakeList)
                {
                    Bot.Move();
                }

                this.MainPanel.Invalidate();
                Thread.Sleep(1000 / Constants.FPS);
            }
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

            if (this.PlayerSnake != null)
            {
                this.PlayerSnake.Dispose();
            }

            var Position = Get_Random_Position();
            this.PlayerSnake = new Snake(Position.X, Position.Y, Direction.RIGHT, 3);
            this.PlayerSnake.SetColor(Color.Black, Color.Blue);
            //this.PlayerSnake.Snake_Control += new Snake.SnakeControl(SnakeLogic);
            this.DrawSnake(PlayerSnake);

            //if (this.SnakeList.Count > 0)
            //{
            //    this.SnakeList.Clear();
            //}
            //for (int i = 0; i < 2; i++)
            //{
            //    Snake newSnake = new Snake($"Computer {i}", this.Graphic, Get_Random_Position(), 500, true);
            //    newSnake.Snake_Control += new Snake.SnakeControl(SnakeLogic);
            //    newSnake.Snake_FindPath += new Snake.FindPath(Caculate_Path);
            //    this.Board.ChangeCellsType(newSnake.Bodies.Select(s => s.Position).ToList(), CellType.OBSTACLE);
            //    this.SnakeList.Add(newSnake);
            //    Thread.Sleep(2);
            //}

            foreach (var Obstacle in this.Obstacles)
            {
                this.Board.ChangeCellType(Obstacle.Position, CellType.OBSTACLE);
            }
            this.Board.ChangeCellsType(this.PlayerSnake.Bodies.Select(i => i.Position).ToList(), CellType.OBSTACLE);
            this.AddNewFood();
            this.AddNewFood();

            lock (this.LockObject)
            {
                this.Graphic.Clear(Color.White);
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

        public void SnakeLogic(Snake CurrentSnake, List<ISnakeBody> OldSnake, List<ISnakeBody> NewSnake)
        {
            //this.Board.ChangeCellsType(OldSnake, CellType.EMPTY);

            CellType temp = this.Board.GetCellType(NewSnake.First().Position);

            if (temp == CellType.OBSTACLE) //đụng vật cản
            {
                CurrentSnake.Dispose();
            }
            else if (temp == CellType.FOOD)
            {
                foreach (Food Food in this.Foods.Where(f => f.Position == NewSnake[0].Position).ToList())
                {
                    Food.Dispose();
                    CurrentSnake.AddLength(1);
                    this.AddNewFood();
                }
            }

            this.Board.ChangeCellsType(OldSnake.Select(i => i.Position).ToList(), CellType.EMPTY);
            this.Board.ChangeCellsType(NewSnake.Select(i => i.Position).ToList(), CellType.OBSTACLE);
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
            this.DrawSnake(PlayerSnake);
            foreach (Snake snake in this.SnakeList)
            {
                snake.Draw();
            }
        }

        private void DrawSnake(ISnake Snake)
        {
            Render.Draw(this.Graphic, new Point(Snake.Head.Position.X * Constants.Block_Size, Snake.Head.Position.Y * Constants.Block_Size), Snake.Head.Border, Snake.Head.Color);
            for (int i = 1; i < Snake.Length; i++)
                Render.Draw(this.Graphic, new Point(Snake.Bodies[i].Position.X * Constants.Block_Size, Snake.Bodies[i].Position.Y * Constants.Block_Size), Snake.Bodies[i].Border, Snake.Bodies[i].Color);

        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            bool bHandled = false;
            switch (keyData)
            {
                case Keys.D:
                    this.PlayerSnake.ChangeDirection(Direction.RIGHT);
                    bHandled = true;
                    break;
                case Keys.A:
                    this.PlayerSnake.ChangeDirection(Direction.LEFT);
                    bHandled = true;
                    break;
                case Keys.W:
                    this.PlayerSnake.ChangeDirection(Direction.UP);
                    bHandled = true;
                    break;
                case Keys.S:
                    this.PlayerSnake.ChangeDirection(Direction.DOWN);
                    bHandled = true;
                    break;
            }
            return bHandled;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.PlayerSnake.State == SnakeState.MOVING)
            {
                this.PlayerSnake.ChangeState(SnakeState.IDLE);
            }
            else if (this.PlayerSnake.State == SnakeState.IDLE)
            {
                this.PlayerSnake.ChangeState(SnakeState.MOVING);
            }
        }

        public void UpdateStatus()
        {
            if (this.PlayerSnake != null)
                this.lblPlayer_Score.Text = $"Điểm của người chơi: {this.PlayerSnake.Length - Constants.Snake_Default_Size}";


            this.table_Score.Items.Clear();

            foreach (Snake snake in this.SnakeList)
            {
                string[] items = new string[3];
                items[0] = snake.State.ToString();
                items[1] = $"{snake.Length - Constants.Snake_Default_Size}";

                this.table_Score.Items.Insert(this.table_Score.Items.Count, new ListViewItem(items));
            }

        }

        private void MainTimer_Tick(object sender, EventArgs e)
        {
            this.UpdateStatus();
        }

        public void Caculate_Path(Snake Current_Snake)
        {
            BFSAlgorithm algorithm = new BFSAlgorithm(this.Board, Current_Snake.Bodies[1].Position);
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
                snake.ChangeState(SnakeState.MOVING);
            }
        }

        private void numberColumnAndRow_ValueChanged(object sender, EventArgs e)
        {
            this.PlayerSnake.ChangeSpeed((int)this.numberColumnAndRow.Value);
            foreach (Snake snake in this.SnakeList)
            {
                snake.ChangeSpeed((int)this.numberColumnAndRow.Value);
            }
        }
    }
}
