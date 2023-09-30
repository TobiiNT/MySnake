using System.Drawing;
using System.Windows.Forms;
using System;
using System.Collections.Generic;
using System.Threading;
using GameCore.Entities.Enums;
using GameCore.Utilities;
using System.Linq;
using GameCore.Entities.Interfaces;
using GameCore.Entities.Implements.Snakes;
using GameCore.Entities.Implements.Games;
using GameCore.Entities.Implements.Controllers;

namespace MySnake
{
    public partial class FormMain : Form
    {
        public Graphics Graphic;

        public Map Map;

        private PlayerController PlayerController;

        private Thread GraphicThread;

        public FormMain()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.Graphic = this.MainPanel.CreateGraphics();


            this.PlayerController = new PlayerController();
            this.PlayerController.OnDirectionInput += Direction =>
            {
                Console.WriteLine($"Direction changed to: {Direction}");
            };

            Reset();

            this.GraphicThread = new Thread(new ThreadStart(this.GraphicLoop));
            this.GraphicThread.Start();
        }

        private void GraphicLoop()
        {
            while (true)
            {
                foreach (var Snake in Map.SnakeList)
                {
                    this.DrawSnake(Snake);
                }

                this.MainPanel.Invalidate();
                Thread.Sleep(1000 / Constants.FPS);
            }
        }

        private void Reset()
        {
            this.Map = new Map(42, 42);
            this.Map.LoadMap("level1.lev");
            this.Map.AddNewFood();
            this.Map.AddNewFood();
            this.Map.ResetSnakes();

            var Position = GetRandomPosition();
            Snake NewSnake = this.Map.NewSnake(Position, Color.Green, this.PlayerController);
            this.DrawSnake(NewSnake);
            this.Map.ChangeCells(NewSnake.Bodies.Select(i => i.Position).ToList(), CellType.OBSTACLE);

            for (int i = 0; i < 2; i++)
            {
                Position = GetRandomPosition();
                ISnakeController Bfs = new BfsController(this.Map);
                Snake NewBotSnake = this.Map.NewSnake(Position, Color.Blue, Bfs);
                this.DrawSnake(NewBotSnake);
                this.Map.ChangeCells(NewBotSnake.Bodies.Select(s => s.Position).ToList(), CellType.OBSTACLE);
                Thread.Sleep(1);
            }
        }

        public Point GetRandomPosition()
        {
            Point Position = new Point(Randomizer.Next(2, this.Map.GetWidth() - 1), Randomizer.Next(2, this.Map.GetHeight() - 1));
            Thread.Sleep(1);
            if (!this.Map.IsCellAvailable(Position))
                GetRandomPosition();
            return Position;
        }

       

        private void MainPanel_Paint(object sender, PaintEventArgs e)
        {
            foreach (IGameObject Object in this.Map.Obstacles)
            {
                Render.Draw(this.Graphic, Object);
            }
            foreach (IGameObject Object in this.Map.Foods)
            {
                Render.Draw(this.Graphic, Object);
            }
            foreach (Snake Snake in this.Map.SnakeList)
            {
                Render.DrawSnake(this.Graphic, Snake);
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            bool IsHandled = false;
            switch (keyData)
            {
                case Keys.Right:
                case Keys.D:
                    this.PlayerController.SetDirection(Direction.RIGHT);
                    IsHandled = true;
                    break;
                case Keys.Left:
                case Keys.A:
                    this.PlayerController.SetDirection(Direction.LEFT);
                    IsHandled = true;
                    break;
                case Keys.Up:
                case Keys.W:
                    this.PlayerController.SetDirection(Direction.UP);
                    IsHandled = true;
                    break;
                case Keys.Down:
                case Keys.S:
                    this.PlayerController.SetDirection(Direction.DOWN);
                    IsHandled = true;
                    break;
            }
            return IsHandled;
        }



        public void UpdateStatus()
        {
            Snake PlayerSnake = this.Map.SnakeList.Where(s => s.Controller == this.PlayerController).FirstOrDefault();
            if (PlayerSnake != null)
                this.lblPlayer_Score.Text = $"Điểm của người chơi: {PlayerSnake.Length}";


            this.TableBotStatus.Items.Clear();

            foreach (Snake Snake in this.Map.SnakeList)
            {
                string[] items = new string[3];
                items[0] = Snake.State.ToString();
                items[1] = $"{Snake.Length}";

                this.TableBotStatus.Items.Insert(this.TableBotStatus.Items.Count, new ListViewItem(items));
            }
        }

        private void MainTimer_Tick(object sender, EventArgs e)
        {
            this.UpdateStatus();
        }

        private void NumericSpeed_ValueChanged(object sender, EventArgs e)
        {
            foreach (Snake Snake in this.Map.SnakeList)
            {
                Snake.ChangeSpeed((int)this.NumericSpeed.Value);
            }
        }

        private void btnStartSnakeMove_Click(object sender, EventArgs e)
        {
            foreach (Snake PlayerSnake in this.Map.SnakeList.Where(i => i.Controller is PlayerController))
            {
                if (PlayerSnake.State == SnakeState.MOVING)
                {
                    PlayerSnake.ChangeState(SnakeState.IDLE);
                }
                else if (PlayerSnake.State == SnakeState.IDLE)
                {
                    PlayerSnake.ChangeState(SnakeState.MOVING);
                }
            }
        }
        private void btnFindPath_Click(object sender, EventArgs e)
        {
            foreach (Snake Snake in this.Map.SnakeList.Where(i => i.Controller is BfsController))
            {
                Snake.ChangeState(SnakeState.MOVING);
                Snake.ChangeSpeed((int)this.NumericSpeed.Value);
            }
        }
        private void btnRestart_Click(object sender, EventArgs e)
        {
            this.Reset();
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.GraphicThread?.Abort();
            this.GraphicThread = null;
        }
    }
}
