using System;
using System.Collections.Generic;
using PlaygroundModule.ModelPart;
using ScriptableObjects;
using UnityEngine;

namespace Infrastructure.Providers
{
    [Serializable]
    public class CellDataProvider
    {
        [SerializeField] private CellDataContainer cellDataContainer;

        public List<Cell> GetCellDataExamples() => cellDataContainer.CellExamples;
    }
}