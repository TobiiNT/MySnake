using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCore.Entities.Interfaces.Snakes
{
    public interface ISnakeHealth
    {
        int RemainHealth { get; }
        int MaxHealth { get; }
        void Increase(int Value);
        void Decrease(int Value);
        void Reset();
    }
}
