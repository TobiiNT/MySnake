using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCore.Entities.Interfaces
{
    public interface IFood : IGameObject
    {
        void ApplyEffect(ISnake Snake);
    }
}
