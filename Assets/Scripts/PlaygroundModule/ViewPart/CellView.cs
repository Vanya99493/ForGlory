using System;
using Inrefaces;
using UnityEngine;

namespace PlaygroundModule.ViewPart
{
    public class CellView : MonoBehaviour, IClickable
    {
        public event Action<CellView> Destroy;
        
        public void Click()
        {
            
        }
    }
}