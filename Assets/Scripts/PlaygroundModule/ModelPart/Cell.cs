using System;
using UnityEngine;

namespace PlaygroundModule.ModelPart
{
    [Serializable]
    public class Cell
    {
        [SerializeField] private CellPixelInfo[,] _cellPixels;

        public Cell(CellPixelInfo[,] cellPixels)
        {
            _cellPixels = new CellPixelInfo[cellPixels.GetLength(0), cellPixels.GetLength(1)];

            for (int i = 0; i < _cellPixels.GetLength(0); i++)
            {
                for (int j = 0; j < _cellPixels.GetLength(1); j++)
                {
                    _cellPixels[i, j] = cellPixels[i, j];
                }
            }
        } 
    }
}