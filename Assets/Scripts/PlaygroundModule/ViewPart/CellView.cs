using System;
using UnityEngine;

namespace PlaygroundModule.ViewPart
{
    public class CellView : MonoBehaviour
    {
        public event Action<CellView> Destroy;
    }
}