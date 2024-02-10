using System;
using UnityEngine;

namespace CastleModule.ViewPart
{
    public class CastleView : MonoBehaviour
    {
        public event Action<CastleView> Destroy;
        
        public void DestroyView()
        {
            Destroy?.Invoke(this);
        }
    }
}