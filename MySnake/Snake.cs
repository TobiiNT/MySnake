using GameCore.Entities.Enums;
using GameCore.Utilities;
using System.Collections.Generic;
using System.Drawing;
using System.Timers;

namespace MySnake
{
    public class Snake
    {
        public Point[] Bodies { private set; get; }

        public string Name { private set; get; }

        public Graphics Graphic { private set; get; }

        public int Length { private set; get; }

        public int MoveSpeed { private set; get; }

        public bool IsMoving { private set; get; }

        public bool IsBot { private set; get; }

        private Brush Color { set; get; }

        private Timer MainTimer { set; get; }

        public Direction Movement { get; set; } = Direction.RIGHT;


        public bool isDisposed = false;

        public List<Point> Computer_Path = new List<Point>();

        public delegate void SnakeControl(Snake snake, Point[] Position, Point Tail, bool Computer);
        public event SnakeControl Snake_Control;

        public delegate void FindPath(Snake snake);
        public event FindPath Snake_FindPath;

        public Point Head
        {
            set { this.Bodies[1] = value; }
            get { return this.Bodies[1]; }
        }
        public Point Tail 
        {
            set { this.Bodies[this.Length] = value; }
            get { return this.Bodies[this.Length]; }
        }

        public Snake(string Name, Graphics Graphic, Point HeadPosition, int MoveSpeed, bool IsBot = false)
        {
            this.Name = Name;
            this.Graphic = Graphic;
            this.Color = Randomizer.GetRandomObjectInEnums<Brush>(typeof(Brushes));
            this.IsBot = IsBot;
            this.Length = Constants.Snake_Default_Size;

            this.Bodies = new Point[1000];
            for (int i = this.Length; i > 0; i--)
                Bodies[i] = new Point(HeadPosition.X - i, HeadPosition.Y);

            this.MoveSpeed = MoveSpeed;
            this.MainTimer = new Timer(this.MoveSpeed);
            this.MainTimer.Enabled = false;
            this.MainTimer.Elapsed += new ElapsedEventHandler(MoveTimer);
            this.IsMoving = false;
        }

        private void MoveTimer(object sender, ElapsedEventArgs e)
        {
            this.Move();
        }

        public void Move()
        {
            if (this.IsMoving)
            {
                Render.Draw(this.Graphic, new Point(this.Tail.X * Constants.Block_Size, this.Tail.Y * Constants.Block_Size), Constants.Background_Border, Constants.Background_Color);

                for (int i = this.Length; i > 1; i--)
                {
                    this.Bodies[i] = this.Bodies[i - 1];
                    Render.Draw(this.Graphic, new Point(this.Bodies[i].X * Constants.Block_Size, this.Bodies[i].Y * Constants.Block_Size), Constants.Snake_Border, this.Color);
                }

                if (this.Movement == Direction.LEFT)
                    this.Head = new Point(this.Head.X - 1, this.Head.Y);
                else if (this.Movement == Direction.RIGHT)
                    this.Head = new Point(this.Head.X + 1, this.Head.Y);
                if (this.Movement == Direction.UP)
                    this.Head = new Point(this.Head.X, this.Head.Y - 1);
                else if (this.Movement == Direction.DOWN)
                    this.Head = new Point(this.Head.X, this.Head.Y + 1);

                Render.Draw(this.Graphic, new Point(this.Head.X * Constants.Block_Size, this.Head.Y * Constants.Block_Size), Constants.Snake_Border, Constants.Snake_Head);

                if (Snake_Control != null)
                {
                    Point[] _NewSnake = new Point[this.Length];
                    for (int i = 1; i <= this.Length; i++)
                        _NewSnake[i - 1] = this.Bodies[i];
                    Snake_Control(this, _NewSnake, Tail, this.IsBot);
                }

                if (this.IsBot)
                {
                    Snake_FindPath(this);
                    if (this.Computer_Path != null)
                    {
                        if (this.Computer_Path.Count > 0)
                        {
                            Point NextStep = this.Computer_Path[this.Computer_Path.Count - 1];
                            this.Computer_Path.RemoveAt(this.Computer_Path.Count - 1);
                            if (this.Head.X > NextStep.X)
                                this.Movement = Direction.LEFT;
                            else if (this.Head.X < NextStep.X)
                                this.Movement = Direction.RIGHT;
                            if (this.Head.Y > NextStep.Y)
                                this.Movement = Direction.UP;
                            else if (this.Head.Y < NextStep.Y)
                                this.Movement = Direction.DOWN;
                        }
                    }
                    else
                    {
                        //this.UpdateMovingState(false);
                    }
                }
            }
        }

        public void Dispose()
        {
            for (int i = 2; i <= this.Length; i++)
                Render.Draw(this.Graphic, new Point(this.Bodies[i].X * Constants.Block_Size, this.Bodies[i].Y * Constants.Block_Size), Constants.Background_Border, Constants.Background_Color);
            Render.Draw(this.Graphic, new Point(this.Head.X * Constants.Block_Size, this.Head.Y * Constants.Block_Size), Constants.Background_Border, Constants.Background_Color);
            this.MainTimer.Enabled = false;
            this.MainTimer.Dispose();
            this.isDisposed = true;
        }

        public void Draw()
        {
            Render.Draw(this.Graphic, new Point(this.Head.X * Constants.Block_Size, this.Head.Y * Constants.Block_Size), Constants.Snake_Border, Constants.Snake_Head);
            for (int i = 2; i <= this.Length; i++)
                Render.Draw(this.Graphic, new Point(this.Bodies[i].X * Constants.Block_Size, this.Bodies[i].Y * Constants.Block_Size), Constants.Snake_Border, this.Color);
        }

        public void AddLength()
        {
            this.Length += 1;
            this.Tail = new Point(Bodies[this.Length - 1].X, Bodies[this.Length - 1].Y);
            //Console.WriteLine($"Add length ({this.Length}) {Bodies[this.Length - 1].X},{Bodies[this.Length - 1].Y}");
        }
        public void UpdateMoveSpeed(int Speed)
        {
            this.MainTimer.Interval = Speed;
            this.MoveSpeed = Speed;
        }
        public void UpdateMovingState(bool Start)
        {
            try
            {
                this.IsMoving = Start;
                if (this.MainTimer != null)
                    this.MainTimer.Enabled = Start;
            }
            catch
            {

            }

        }
    }
}
