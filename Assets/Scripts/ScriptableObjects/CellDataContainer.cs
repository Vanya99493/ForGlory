using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "CellDataContainer", menuName = "ScriptableObjects/Prefabs/CellDataContainer", order = 2)]
    public class CellDataContainer : ScriptableObject
    {
        [SerializeField] private List<PlaygroundModule.ModelPart.CellData> cellPixels;

        public List<PlaygroundModule.ModelPart.CellData> CellData() => cellPixels;
    }
}