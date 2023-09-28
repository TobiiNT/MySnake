using GameCore.Entities;
using GameCore.Entities.Enums;
using GameCore.Entities.Interfaces;
using GameCore.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Timers;

namespace MySnake
{
    public class Snake
    {
        public List<ISnakeBody> Bodies { private set; get; }
        public ISnakeBody Head
        {
            set { this.Bodies[0] = value; }
            get { return this.Bodies[0]; }
        }
        public ISnakeBody Tail
        {
            set { this.Bodies[this.Length - 1] = value; }
            get { return this.Bodies[this.Length - 1]; }
        }
        public int Length => this.Bodies.Count;
        public Direction Direction { private set; get; }

        public SnakeState State { private set; get; }

        public string Name { private set; get; }

        public Graphics Graphic { private set; get; }

        public int MoveSpeed { private set; get; }

        public bool IsBot { private set; get; }

        private Brush Color { set; get; }
        private int PendingBodies { set; get; }

        private DateTime LastMove { set; get; }

        public List<Point> Computer_Path = new List<Point>();

        public delegate void SnakeControl(Snake snake, List<ISnakeBody> OldSnake, List<ISnakeBody> NewSnake);
        public event SnakeControl Snake_Control;

        public delegate void FindPath(Snake snake);
        public event FindPath Snake_FindPath;

        public Snake(string Name, Graphics Graphic, Point HeadPosition, int MoveSpeed, bool IsBot = false)
        {
            this.Name = Name;
            this.Graphic = Graphic;
            this.Color = Randomizer.GetRandomObjectInEnums<Brush>(typeof(Brushes));
            this.IsBot = IsBot;
            this.State = SnakeState.IDLE;
            this.Direction = Direction.RIGHT;
            this.LastMove = DateTime.Now;

            this.Bodies = new List<ISnakeBody>();
            for (int i = Constants.Snake_Default_Size; i > 0; i--)
                Bodies.Add(new SnakeBody(null, HeadPosition.X + i, HeadPosition.Y));

            this.MoveSpeed = MoveSpeed;
        }

        public void Move()
        {
            if (this.State != SnakeState.MOVING) return;
            if (this.LastMove.AddMilliseconds(this.MoveSpeed) > DateTime.Now) return;
            this.LastMove = DateTime.Now;

            Render.Draw(this.Graphic, new Point(this.Tail.Position.X * Constants.Block_Size, this.Tail.Position.Y * Constants.Block_Size), Constants.Background_Border, Constants.Background_Color);

            List<ISnakeBody> OldBodies = this.Bodies.ToList();
            List<ISnakeBody> NewBodies = new List<ISnakeBody>();
            if (this.Direction == Direction.LEFT)
                NewBodies.Add(new SnakeBody(null, this.Head.Position.X - 1, this.Head.Position.Y));
            else if (this.Direction == Direction.RIGHT)
                NewBodies.Add(new SnakeBody(null, this.Head.Position.X + 1, this.Head.Position.Y));
            if (this.Direction == Direction.UP)
                NewBodies.Add(new SnakeBody(null, this.Head.Position.X, this.Head.Position.Y - 1));
            else if (this.Direction == Direction.DOWN)
                NewBodies.Add(new SnakeBody(null, this.Head.Position.X, this.Head.Position.Y + 1));

            foreach (var Part in this.Bodies.GetRange(0, this.Length - 1))
            {
                NewBodies.Add(Part);
            }
            if (PendingBodies > 0)
            {
                PendingBodies--;
                NewBodies.Add(Tail);
            }

            for (int i = 0; i < NewBodies.Count; i++)
            {
                if (i == 0)
                {
                    Render.Draw(this.Graphic, new Point(NewBodies[i].Position.X * Constants.Block_Size, NewBodies[i].Position.Y * Constants.Block_Size), Constants.Snake_Border, Constants.Snake_Head);
                }
                else
                {
                    Render.Draw(this.Graphic, new Point(NewBodies[i].Position.X * Constants.Block_Size, NewBodies[i].Position.Y * Constants.Block_Size), Constants.Snake_Border, this.Color);
                }
            }
            this.Bodies = NewBodies;

            if (Snake_Control != null)
            {
                Snake_Control(this, OldBodies, NewBodies);
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
                        if (this.Head.Position.X > NextStep.X)
                            this.Direction = Direction.LEFT;
                        else if (this.Head.Position.X < NextStep.X)
                            this.Direction = Direction.RIGHT;
                        if (this.Head.Position.Y > NextStep.Y)
                            this.Direction = Direction.UP;
                        else if (this.Head.Position.Y < NextStep.Y)
                            this.Direction = Direction.DOWN;
                    }
                }
                else
                {
                    //this.UpdateMovingState(false);
                }
            }
        }

        public void Draw()
        {
            Render.Draw(this.Graphic, new Point(this.Head.Position.X * Constants.Block_Size, this.Head.Position.Y * Constants.Block_Size), Constants.Snake_Border, Constants.Snake_Head);
            for (int i = 1; i < this.Length; i++)
                Render.Draw(this.Graphic, new Point(this.Bodies[i].Position.X * Constants.Block_Size, this.Bodies[i].Position.Y * Constants.Block_Size), Constants.Snake_Border, this.Color);
        }

        public void AddLength(int Length)
        {
            if (Length <= 0) return;

            this.PendingBodies += Length;
        }

        public void ChangeDirection(Direction Direction)
        {
            if (this.Direction == Direction.LEFT && Direction != Direction.RIGHT ||
                this.Direction == Direction.RIGHT && Direction != Direction.LEFT ||
                this.Direction == Direction.UP && Direction != Direction.DOWN ||
                this.Direction == Direction.DOWN && Direction != Direction.UP)
            {
                this.Direction = Direction;
            }
        }

        public void ChangeSpeed(int Speed)
        {
            if (Speed > 0) this.MoveSpeed = Speed;
        }

        public void ChangeState(SnakeState State)
        {
            this.State = State;
        }

        public void Die()
        {
            this.ChangeState(SnakeState.DIE);
        }

        public void Dispose()
        {
            for (int i = 0; i < this.Length; i++)
                Render.Draw(this.Graphic, new Point(this.Bodies[i].Position.X * Constants.Block_Size, this.Bodies[i].Position.Y * Constants.Block_Size), Constants.Background_Border, Constants.Background_Color);
            Render.Draw(this.Graphic, new Point(this.Head.Position.X * Constants.Block_Size, this.Head.Position.Y * Constants.Block_Size), Constants.Background_Border, Constants.Background_Color);
            this.ChangeState(SnakeState.DISPOSED);
        }
    }
}
