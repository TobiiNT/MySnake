using System.Drawing;
using System.Windows.Forms;
using System;
using System.Threading;
using GameCore.Entities.Enums;
using GameCore.Utilities;
using System.Linq;
using GameCore.Entities.Implements.Snakes;
using GameCore.Entities.Implements.Games;
using GameCore.Entities.Implements.Controllers;
using GameCore.Entities.Interfaces.Controllers;
using GameCore.Entities.Interfaces.Games;
using GameCore.Entities.Interfaces.Snakes;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;

namespace MySnake
{
    public partial class FormMain : Form
    {
        public Map Map;

        private PlayerController PlayerController;

        private Thread GraphicThread;

        private OpenGL OpenGL;

        private bool IsRunning = false;

        public FormMain()
        {
            InitializeComponent();

            this.GraphicControl.Paint += GraphicControl_Paint;

            this.PlayerController = new PlayerController();

            Reset();

            this.GraphicThread = new Thread(new ThreadStart(this.GraphicLoop));
            this.GraphicThread.Start();
        }

        private void GraphicLoop()
        {
            while (true)
            {
                this.GraphicControl.Invalidate();

                Thread.Sleep(1000 / Constants.FPS);
            }
        }

        private void GraphicControl_Paint(object sender, PaintEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            // Better point and line drawing
            GL.Hint(HintTarget.PointSmoothHint, HintMode.Nicest);
            GL.Hint(HintTarget.LineSmoothHint, HintMode.Nicest);
            GL.Enable(EnableCap.PointSmooth);
            GL.Enable(EnableCap.LineSmooth);

            OpenGL.DrawBackground();
            this.DrawObstacles();
            this.DrawFoods();
            this.DrawSnakes();

            ErrorCode Error = GL.GetError();
            if (Error != ErrorCode.NoError)
            {
                Console.WriteLine($"OpenGL Error: {Error}");
            }

            this.GraphicControl.SwapBuffers();
        }

        private void DrawObstacles()
        {
            foreach (var Obstacle in Map.Obstacles)
            {
                OpenGL.DrawObject(Obstacle);
            }
            if (this.CheckShowObstacleBorder.Checked)
            {
                for (int i = 0; i < Map.GetMatrix().GetLength(0); i++)
                {
                    for (int j = 0; j < Map.GetMatrix().GetLength(1); j++)
                    {
                        if (Map.GetCellType(new Point(i, j)) == CellType.OBSTACLE)
                            OpenGL.DrawObject(new Obstacle(i, j));
                    }
                }
            }
        }

        private void DrawFoods()
        {
            foreach (var Food in Map.Foods)
            {
                OpenGL.DrawObject(Food);
            }
        }

        private void DrawSnakes()
        {
            foreach (var Snake in Map.SnakeList.Where(s => s.State <= (this.CheckShowDeadSnake.Checked ? SnakeState.DIE : SnakeState.MOVING)))
            {
                OpenGL.DrawObject(Snake.Head);
                for (int i = 1; i < Snake.Length; i++)
                    OpenGL.DrawObject(Snake.Bodies[i]);
            }
        }

        private void Reset()
        {
            this.IsRunning = false;

            this.Map = new Map(42, 42);
            this.Map.LoadMap("level1.lev");
            this.Map.AddNewFood();
            this.OpenGL = new OpenGL(this.Map, Color.White);

            this.GenerateSnakes();
        }



        private void MainTimer_Tick(object sender, EventArgs e)
        {
            int SnakeCount = this.Map.SnakeList.Count;
            int ItemCount = this.TableBotStatus.Items.Count;

            // Remove excess items
            while (ItemCount > SnakeCount)
            {
                this.TableBotStatus.Items.RemoveAt(ItemCount - 1);
                ItemCount--;
            }

            // Update or add items
            int Index = 1;
            foreach (Snake Snake in this.Map.SnakeList)
            {
                string[] Status = new string[]
                {
        Index.ToString(),
        Snake.Controller.GetType().Name,
        Snake.Length.ToString(),
        Snake.State.ToString(),
                };

                if (Index <= ItemCount)
                {
                    // Update existing item
                    ListViewItem Item = this.TableBotStatus.Items[Index - 1];
                    Item.SubItems[0].Text = Status[0];
                    Item.SubItems[1].Text = Status[1];
                    Item.SubItems[2].Text = Status[2];
                    Item.SubItems[3].Text = Status[3];
                }
                else
                {
                    // Add new item
                    this.TableBotStatus.Items.Add(new ListViewItem(Status));
                }
                Index++;
            }
        }

        private void NumericSpeed_ValueChanged(object sender, EventArgs e)
        {
            foreach (Snake Snake in this.Map.SnakeList)
            {
                Snake.ChangeSpeed(1000 / (int)this.NumericSpeed.Value);
            }
        }



        private void CheckHasPlayer_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.IsRunning)
            {
                this.Reset();
            }
        }

        private void GenerateSnakes()
        {
            this.Map.ResetSnakes();
            for (int i = this.CheckHasPlayer.Checked ? 0 : 1; i <= this.NumericBotCount.Value; i++)
            {
                ISnakeController Controller;
                Color RandomColor;
                if (i == 0)
                {
                    Controller = this.PlayerController;
                    RandomColor = Color.Green;
                }
                else
                {
                    Controller = new BfsController(this.Map);
                    RandomColor = Randomizer.GetRandomObject(new List<Color>() { Color.Blue, Color.Violet, Color.Pink, Color.Purple, Color.Gray });
                }
                Direction Direction = Randomizer.GetRandomObjectInEnums<Direction>();
                ISnake Snake = this.Map.NewSnake(Map.GetSnakeStartPosition(Constants.SnakeInitSize, Direction), Direction, RandomColor, Controller);
                Snake.ChangeSpeed(1000 / (int)this.NumericSpeed.Value);

                Thread.Sleep(1);
            }
        }

        private void btnStartSnakeMove_Click(object sender, EventArgs e)
        {
            if (!this.IsRunning)
            {
                this.btnStartSnake.Text = "Stop";
                this.IsRunning = true;
                this.NumericBotCount.Enabled = false;
                this.CheckHasPlayer.Enabled = false;

                foreach (Snake Snake in this.Map.SnakeList)
                {
                    if (Snake.Controller is PlayerController)
                    {
                        if (this.CheckStartPlayerSnake.Checked && Snake.State == SnakeState.IDLE)
                        {
                            Snake.ChangeState(SnakeState.MOVING);
                        }
                    }
                    else
                    {
                        if (this.CheckStartBotSnake.Checked && Snake.State == SnakeState.IDLE)
                        {
                            Snake.ChangeState(SnakeState.MOVING);
                        }
                    }
                    Snake.ChangeSpeed(1000 / (int)this.NumericSpeed.Value);
                }
            }
            else
            {
                this.btnStartSnake.Text = "Start";
                this.IsRunning = false;
                this.NumericBotCount.Enabled = true;
                this.CheckHasPlayer.Enabled = true;

                foreach (Snake Snake in this.Map.SnakeList)
                {
                    if (Snake.State == SnakeState.MOVING)
                    {
                        Snake.ChangeState(SnakeState.IDLE);
                    }
                }
            }
        }

        private void CheckStartPlayerSnake_CheckedChanged(object sender, EventArgs e)
        {
            if (this.IsRunning)
            {
                foreach (Snake Snake in this.Map.SnakeList.Where(s => s.Controller is PlayerController))
                {
                    if (this.CheckStartPlayerSnake.Checked)
                    {
                        Snake.ChangeState(SnakeState.MOVING);
                    }
                    else if (Snake.State == SnakeState.MOVING)
                    {
                        Snake.ChangeState(SnakeState.IDLE);
                    }
                    Snake.ChangeSpeed(1000 / (int)this.NumericSpeed.Value);
                }
            }
        }

        private void CheckStartBotSnake_CheckedChanged(object sender, EventArgs e)
        {
            if (this.IsRunning)
            {
                foreach (Snake Snake in this.Map.SnakeList.Where(s => !(s.Controller is PlayerController)))
                {
                    if (this.CheckStartBotSnake.Checked)
                    {
                        Snake.ChangeState(SnakeState.MOVING);
                    }
                    else if (Snake.State == SnakeState.MOVING)
                    {
                        Snake.ChangeState(SnakeState.IDLE);
                    }
                    Snake.ChangeSpeed(1000 / (int)this.NumericSpeed.Value);
                }
            }
        }

        private void btnRestart_Click(object sender, EventArgs e)
        {
            if (this.IsRunning)
            {
                this.btnStartSnakeMove_Click(sender, e);
            }
            this.Reset();
        }

        private void NumericBotCount_ValueChanged(object sender, EventArgs e)
        {
            if (!this.IsRunning)
            {
                this.Reset();
            }
        }


        #region Other functions
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
                    this.PlayerController.SetDirection(Direction.DOWN);
                    IsHandled = true;
                    break;
                case Keys.Down:
                case Keys.S:
                    this.PlayerController.SetDirection(Direction.UP);
                    IsHandled = true;
                    break;
            }
            return IsHandled;
        }
        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.GraphicThread?.Abort();
            this.GraphicThread = null;
            this.Map.Dispose();
        }
        #endregion
    }
}
