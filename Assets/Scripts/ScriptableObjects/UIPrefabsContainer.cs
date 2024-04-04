using UIModule.Panels.CastleMenuModule;
using UIModule.Panels.LoadLevelMenuModule;
using UnityEngine;
using UnityEngine.UI;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "UIPrefabContainer", menuName = "ScriptableObjects/Data/UIPrefabContainer", order = 2)]
    public class UIPrefabsContainer : ScriptableObject
    {
        public HeroCard HeroCardPrefab;
        public LevelSaveCard LevelSaveCardPrefab;
        public Image QueueCharacterIcon;
    }
}