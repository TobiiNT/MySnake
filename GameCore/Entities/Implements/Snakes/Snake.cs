using GameCore.Entities.Enums;
using GameCore.Entities.Implements.Games;
using GameCore.Entities.Interfaces;
using GameCore.Events;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace GameCore.Entities.Implements.Snakes
{
    public class Snake : ISnake
    {
        public ISnakeController Controller { private set; get; }

        public ISnakeBody Head => Bodies[0];

        public ISnakeBody Tail => Bodies[Length - 1];

        public List<ISnakeBody> Bodies { private set; get; }

        public int Length => this.Bodies.Count;

        public SnakeState State { private set; get; }

        public bool IsMoving { private set; get; }

        public int MoveSpeed { private set; get; }

        public Direction Direction { private set; get; }

        public Point Position { private set; get; }

        public int PendingBodies { private set; get; }

        public DateTime LastMoveTime { private set; get; }

        public int BorderWidth => 5;

        public Pen Border { private set; get; }

        public Brush Color { private set; get; }

        public event EventHandler<EventArgs> OnDisposed;

        public event EventHandler<EventArgs> OnSnakeMoving;

        public event EventHandler<EventArgs> OnSnakeLengthChanged;

        public Snake(int X, int Y, Direction Direction, int StartLength)
        {
            this.Bodies = new List<ISnakeBody>();
            this.State = SnakeState.IDLE;
            this.Direction = Direction;
            this.PendingBodies = 0;
            this.LastMoveTime = DateTime.Now;

            this.Bodies.Add(new SnakeHead(this, X, Y));
            for (int i = 1; i <= StartLength; i++)
            {
                switch (Direction)
                {
                    case Direction.LEFT: X++; break;
                    case Direction.RIGHT: X--; break;
                    case Direction.UP: Y++; break;
                    case Direction.DOWN: Y--; break;
                }
                this.Bodies.Add(new SnakeBody(this, X, Y));
            }
        }

        public void SetColor(Color Border, Color Body)
        {
            this.Border = new Pen(Border);
            this.Color = new SolidBrush(Body);
        }

        public void SetController(ISnakeController Controller)
        {
            this.Controller = Controller;
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
            this.AddLength(1);
        }

        public void ChangeState(SnakeState State)
        {
            this.State = State;
        }

        public void Move()
        {
            if (State != SnakeState.MOVING) return;
            if (this.LastMoveTime.AddMilliseconds(this.MoveSpeed) > DateTime.Now) return;
            this.LastMoveTime = DateTime.Now;

            // If the snake has a controller, get the next move from the controller
            if (this.Controller != null)
            {
                Direction NextDirection = this.Controller.GetNextMove(this);
                
                this.ChangeDirection(NextDirection);
            }
          
            Point LastTailPosition = this.Tail.Position;
            for (int i = Length - 1; i > 0; i--)
            {
                ISnakeBody Part = this.Bodies[i];
                ISnakeBody NextPart = this.Bodies[i - 1];
                Part.MoveTo(NextPart.Position);
            }
            if (this.Direction == Direction.LEFT)
                this.Head.MoveTo(new Point(this.Head.Position.X - 1, this.Head.Position.Y));
            else if (this.Direction == Direction.RIGHT)
                this.Head.MoveTo(new Point(this.Head.Position.X + 1, this.Head.Position.Y));
            else if(this.Direction == Direction.UP)
                this.Head.MoveTo(new Point(this.Head.Position.X, this.Head.Position.Y - 1));
            else if (this.Direction == Direction.DOWN)
                this.Head.MoveTo(new Point(this.Head.Position.X, this.Head.Position.Y + 1));
         
            if (PendingBodies > 0)
            {
                PendingBodies--;
                Bodies.Add(new SnakeBody(this, LastTailPosition.X, LastTailPosition.Y));
                this.OnSnakeLengthChanged?.Invoke(this, new OnSnakeLengthChanged(this, this.Bodies.Count));
            }

            this.OnSnakeMoving?.Invoke(this, new OnSnakeMoving(this, LastTailPosition));

            if (IsSelfCollision()) // Check for collisions with itself
                Die();
        }

        public bool IsSelfCollision()
        {
            // Check each snake body part with every other snake body part
            for (int i = 0; i < Bodies.Count; i++)
            {
                for (int j = 0; j < Bodies.Count; j++)
                {
                    if (i == j) // Do not want to check a body part with itself
                        continue;
                    ISnakeBody Part1 = Bodies[i];
                    ISnakeBody Part2 = Bodies[j];

                    // Collision check logic
                    if (Part1.Position == Part2.Position)
                        return true;
                }
            }
            return false;
        }

        public void Die()
        {
            this.ChangeState(SnakeState.DIE);
        }

        public void Dispose()
        {
            this.ChangeState(SnakeState.DISPOSED);
            this.OnDisposed?.Invoke(this, EventArgs.Empty);
        }
    }
}
