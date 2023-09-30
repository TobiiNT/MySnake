using GameCore.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCore.Events.Interfaces
{
    public interface ISnakeEvent
    {
        ISnake Snake { get; }
    }
}
