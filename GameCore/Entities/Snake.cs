using GameCore.Entities.Enums;
using GameCore.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace GameCore.Entities
{
    public class Snake2 : ISnake
    {
        public List<ISnakeBody> Bodies { private set; get; }

        public SnakeState State { private set; get; }

        public bool IsMoving { private set; get; }

        public int MoveSpeed { private set; get; }

        public Direction Direction { private set; get; }

        public Point Position { private set; get; }

        public int PendingBodies { private set; get; }

        public int BorderWidth => 5;

        public Pen Border { private set; get; }

        public Brush Color { private set; get; }

        public event EventHandler<EventArgs> OnDisposed;

        public Snake2()
        {
            this.Bodies = new List<ISnakeBody>();
            this.State = SnakeState.IDLE;
            this.Direction = Direction.RIGHT;
            this.PendingBodies = 0;
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

        public void Move()
        {
            if (State != SnakeState.MOVING) return;

            // Adds any pending body parts. Note that this processes one body part at a time;
            // if PendingBodies > 1, it will require more than one frame to process completely.
            if (this.PendingBodies > 0)
            {
                ISnakeBody Tail = this.Bodies.Last(); // Adds the body part to the tail
                this.Bodies.Add(new SnakeBody(this, Tail.Position.X, Tail.Position.Y));
                this.PendingBodies--;
            }

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
            this.OnDisposed(this, EventArgs.Empty);
        }
    }
}
