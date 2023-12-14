using System;
using Infrastructure.InputHandlerModule;
using Interfaces;
using UnityEngine;

namespace CharacterModule.ViewPart
{
    public class CharacterView : MonoBehaviour, IClickable
    {
        public event Action ClickedAction;
        public event Action<CharacterView> Destroy;
        
        public void DestroyView()
        {
            Destroy?.Invoke(this);
        }

        public void Click(InputMouseButtonType mouseButtonType)
        {
            ClickedAction?.Invoke();
        }
    }
}