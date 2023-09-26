using GameCore.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace GameCore.Entities
{
    public class Map
    {
        private int[,] Matrix { set; get; }

        public Map(int Rows, int Columns)
        {
            this.Matrix = new int[Rows, Columns];
        }

        public void ChangeCellType(Point[] Cells, CellType type)
        {
            // This is only used for the snake so skip the snake's head at cells[0]
            for (int i = 1; i < Cells.Length; i++)
            {
                SetCellValue(Cells[i], type);
            }
        }
        public void ChangeCellType(List<Point> Cells, CellType type)
        {
            // This is only used for the snake so skip the snake's head at cells[0]
            foreach (Point Location in Cells)
            {
                SetCellValue(Location, type);
            }
        }

        public void ChangeCellType(Point cell, CellType type)
        {
            SetCellValue(cell, type);
        }

        public bool IsCellAvailable(Point cell)
        {
            return GetCellType(cell) == CellType.EMPTY;
        }
        public CellType GetCellType(int X, int Y) => GetCellType(new Point(X, Y));
        public CellType GetCellType(Point cell)
        {
            try
            {
                return (CellType)Matrix[cell.X, cell.Y];
            }
            catch (IndexOutOfRangeException)
            {
                return CellType.OBSTACLE;
            }
        }

        public int GetWidth() => Matrix.GetLength(0);
        public int GetHeight() => Matrix.GetLength(1);


        private void SetCellValue(Point Cell, CellType type)
        {
            try
            {
                if (Cell == null || Cell.X > Matrix.GetLength(0) || Cell.Y > Matrix.GetLength(1))
                    return;
                Matrix[Cell.X, Cell.Y] = (int)type;
            }
            catch (IndexOutOfRangeException)
            {
                // Log error or handle it appropriately.
            }
        }
    }
}
