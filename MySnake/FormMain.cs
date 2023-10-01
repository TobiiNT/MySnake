﻿using System.Drawing;
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
            for (int i = 0; i < Map.GetMatrix().GetLength(0); i++)
            {
                for (int j = 0; j < Map.GetMatrix().GetLength(1); j++)
                {
                    if (Map.GetCellType(new Point(i, j)) == CellType.OBSTACLE)
                        OpenGL.DrawObject(new Obstacle(i, j));
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
            foreach (var Snake in Map.SnakeList.Where(s => s.State < SnakeState.DIE))
            {
                OpenGL.DrawObject(Snake.Head);
                for (int i = 1; i < Snake.Length; i++)
                    OpenGL.DrawObject(Snake.Bodies[i]);
            }
        }

        private void Reset()
        {
            this.Map = new Map(42, 42);
            this.Map.LoadMap("level1.lev");
            this.Map.AddNewFood();
            this.Map.ResetSnakes();
            this.OpenGL = new OpenGL(this.Map, Color.White);

            for (int i = 0; i <= this.NumericBotCount.Value; i++)
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
                    Controller = new AlmightyMoveController(this.Map);
                    RandomColor = Randomizer.GetRandomObject(new List<Color>() { Color.Blue, Color.Violet, Color.Pink, Color.Purple, Color.Gray });
                }
                Direction Direction = Randomizer.GetRandomObjectInEnums<Direction>();
                ISnake Snake = this.Map.NewSnake(Map.GetSnakeStartPosition(Constants.SnakeInitSize, Direction), Direction, RandomColor, Controller);
                Snake.ChangeSpeed(1000 / (int)this.NumericSpeed.Value);

                Thread.Sleep(1);
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

        private void MainTimer_Tick(object sender, EventArgs e)
        {
            this.TableBotStatus.Items.Clear();
            int Index = 1;
            foreach (Snake Snake in this.Map.SnakeList)
            {
                string[] Status = new string[]
                {
                    Index++.ToString(),
                    Snake.Controller.GetType().Name,
                    Snake.Length.ToString(),
                    Snake.State.ToString(),
                };

                this.TableBotStatus.Items.Insert(this.TableBotStatus.Items.Count, new ListViewItem(Status));
            }
        }

        private void NumericSpeed_ValueChanged(object sender, EventArgs e)
        {
            foreach (Snake Snake in this.Map.SnakeList)
            {
                Snake.ChangeSpeed(1000 / (int)this.NumericSpeed.Value);
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
            foreach (Snake Snake in this.Map.SnakeList.Where(i => i.Controller != PlayerController))
            {
                Snake.ChangeState(SnakeState.MOVING);
                Snake.ChangeSpeed(1000 /(int)this.NumericSpeed.Value);
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
            this.Map.Dispose();
        }
    }
}
