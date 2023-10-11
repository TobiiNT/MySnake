using GameCore.Entities.Enums;

namespace GameCore.Entities.Interfaces.Games
{
    public interface IMatrix
    {
        CellType[,] Matrix { get; }
        int Width { get; }
        int Height { get; }
    }
}
