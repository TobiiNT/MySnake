using GameCore.Entities;
using GameCore.Entities.Interfaces;
using System.Collections.Generic;
using System.IO;

namespace GameCore.Loaders
{
    public class MapLoader
    {
        public int MapHeight { private set; get; }
        public int MapWidth { private set; get; }

        public MapLoader(int MapWidth, int MapHeight)
        {
            this.MapHeight = MapHeight;
            this.MapWidth = MapWidth;
        }

        public List<IGameObject> Load(string Path)
        {
            List<IGameObject> Objects = new List<IGameObject>();
            using (StreamReader Reader = new StreamReader(Path))
            {
                for (int Row = 0; Row < MapHeight; Row++)
                {
                    for (int Column = 0; Column < MapWidth; Column++)
                    {
                        if (Reader.Read() == 49 || Row == 0 || Column == 0 || Row == MapHeight - 1 || Column == MapWidth - 1)
                        {
                            Objects.Add(new Obstacle(Column, Row));
                        }
                    }
                    Reader.Read();
                    Reader.Read();
                }
            }
            return Objects;
        }
    }
}
