using GameCore.Entities.Enums;
using GameCore.Entities.Implements.Games;
using GameCore.Entities.Interfaces.Controllers;
using GameCore.Entities.Interfaces.Games;
using GameCore.Entities.Interfaces.Snakes;
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

        public ISnakeHealth Health { private set; get; }

        public int Length => this.Bodies.Count;

        public SnakeState State { private set; get; }

        public bool IsMoving { private set; get; }

        public int MoveSpeed { private set; get; }

        public Direction Direction { private set; get; }

        public Point Position { private set; get; }

        public int PendingBodies { private set; get; }

        public DateTime LastMoveTime { private set; get; }

        public float Size => 0.8f;

        public int BorderWidth => 5;

        public Color BorderColor { private set; get; }

        public Color FillColor { private set; get; }

        public event EventHandler<EventArgs> OnMoving;

        public event EventHandler<EventArgs> OnDirectionChanged;

        public event EventHandler<EventArgs> OnLengthChanged;

        public event EventHandler<EventArgs> OnDied;

        public event EventHandler<EventArgs> OnDisposed;

        public Snake(int X, int Y, Direction Direction, int StartLength, int InitHealth, int MaxHealth)
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

            this.Health = new SnakeHealth(InitHealth, MaxHealth);
        }

        public void SetColor(Color Border, Color Body)
        {
            this.BorderColor = Border;
            this.FillColor = Body;
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
            if (this.Direction != Direction &&
                (this.Direction == Direction.LEFT && Direction != Direction.RIGHT ||
                this.Direction == Direction.RIGHT && Direction != Direction.LEFT ||
                this.Direction == Direction.UP && Direction != Direction.DOWN ||
                this.Direction == Direction.DOWN && Direction != Direction.UP))
            {
                this.Direction = Direction;
                this.OnDirectionChanged?.Invoke(this, new OnSnakeDirectionChanged(this, Direction));
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

        public void Move(IMatrix Matrix)
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

            int NewX = this.Head.Position.X;
            int NewY = this.Head.Position.Y;
            if (this.Direction == Direction.LEFT)
            {
                NewX--;
                if (NewX < 0) NewX = Matrix.Width - 1;
            }
            else if (this.Direction == Direction.RIGHT)
            {
                NewX++;
                if (NewX >= Matrix.Width) NewX = 0;
            }
            else if (this.Direction == Direction.UP)
            {
                NewY--;
                if (NewY < 0) NewY = Matrix.Height - 1;
            }
            else if (this.Direction == Direction.DOWN)
            {
                NewY++;
                if (NewY >= Matrix.Height) NewY = 0;
            }
            this.Head.MoveTo(new Point(NewX, NewY));

            if (PendingBodies > 0)
            {
                PendingBodies--;
                Bodies.Add(new SnakeBody(this, LastTailPosition.X, LastTailPosition.Y));
                this.OnLengthChanged?.Invoke(this, new OnSnakeLengthChanged(this, this.Bodies.Count));
            }

            this.OnMoving?.Invoke(this, new OnSnakeMoving(this, LastTailPosition));

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
            this.OnDied?.Invoke(this, new OnSnakeDied(this));
        }

        public void Dispose()
        {
            this.ChangeState(SnakeState.DISPOSED);
            this.OnDisposed?.Invoke(this, EventArgs.Empty);
        }
    }
}
