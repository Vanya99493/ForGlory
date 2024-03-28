using UIModule.Panels.CastleMenuModule;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "UIPrefabContainer", menuName = "ScriptableObjects/Data/UIPrefabContainer", order = 2)]
    public class UIPrefabsContainer : ScriptableObject
    {
        public HeroCard HeroCardPrefab;
    }
}