using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace Array2DEditor
{
    [System.Serializable]
    public class Array2DInt : Array2D<int>
    {
        [SerializeField]
        CellRowInt[] cells;

        public Array2DInt(int[,] array)
        {
            cells = new CellRowInt[array.GetLength(1)];

            // Iterate over each row of the given array
            for (int i = 0; i < array.GetLength(1); i++)
            {
                // Initialize a new CellRowInt for each row
                cells[i] = new CellRowInt(new int[array.GetLength(0)]);

                // Iterate over each column of the given array
                for (int j = 0; j < array.GetLength(0); j++)
                {
                    // Assign the value from the given array to the corresponding cell with world space rotation
                    cells[i][j] = array[j, i];
                }
            }

            Array.Reverse(cells);

            this.gridSize = new Vector2Int(array.GetLength(0),array.GetLength(1));
        }

        protected override CellRow<int> GetCellRow(int idx)
        {
            return cells[idx];
        }

       public Vector2Int getGridSize ()
        {
            return this.GridSize;
        }

        public int[,] toIntArrary()
        {
            int[,] result = new int[this.GridSize.x,this.GridSize.y];
            for (int i = 0; i < this.GridSize.x; i++)
            {
                for (int j = 0; j < this.GridSize.y; j++)
                {
                    result[i, j] = GetCell(i,j);
                }
            }
            return result;
        }
    }
}
