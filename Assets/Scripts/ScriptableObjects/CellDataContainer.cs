using System.Collections.Generic;
using PlaygroundModule.ModelPart;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "CellDataContainer", menuName = "ScriptableObjects/Data/CellDataContainer", order = 3)]
    public class CellDataContainer : ScriptableObject
    {
        [SerializeField] private List<Cell> cellExamples;

        public List<Cell> CellExamples => cellExamples;
    }
}