using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "LevelDataContainer", menuName = "ScriptableObjects/Data/LevelDataContainer", order = 1)]
    public class LevelDataContainer : ScriptableObject
    {
        public LevelDataContainer LevelData;
    }
}