using GameCore.Entities.Interfaces.Snakes;
using System;
using System.Drawing;

namespace GameCore.Entities.Implements.Games
{
    public class SnakeHealth : ISnakeHealth
    {
        public int RemainHealth { private set; get; }
        public int MaxHealth { private set; get; }

        public SnakeHealth(int InitHealth, int MaxHealth)
        {
            this.RemainHealth = InitHealth;
            this.MaxHealth = MaxHealth;
        }

        public void Decrease(int Value)
        {
            if (Value > 0) this.RemainHealth -= Value;
            if (this.RemainHealth <= 0) this.RemainHealth = 0;
        }

        public void Increase(int Value)
        {
            if (Value > 0) this.RemainHealth += Value;
            if (this.RemainHealth > this.MaxHealth) this.RemainHealth = this.MaxHealth;
        }

        public void Reset()
        {
            this.RemainHealth = this.MaxHealth;
        }
    }
}
