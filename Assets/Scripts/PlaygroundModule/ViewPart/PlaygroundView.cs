using System;
using UnityEngine;

namespace PlaygroundModule.ViewPart
{
    public class PlaygroundView : MonoBehaviour
    {
        public event Action<PlaygroundView> Destroy;

        public void ActivatePlayground()
        {
            gameObject.SetActive(true);
        }

        public void DeactivatePlayground()
        {
            gameObject.SetActive(false);
        }
        
        public void DestroyPlayground()
        {
            Destroy?.Invoke(this);
        }
    }
}