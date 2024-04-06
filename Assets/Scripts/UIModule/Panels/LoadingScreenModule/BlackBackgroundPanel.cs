using System;
using System.Collections;
using UnityEngine;

namespace UIModule.Panels.LoadingScreenModule
{
    public class BlackBackgroundPanel : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private float disappearanceTime;
        
        public void StartDisappear()
        {
            canvasGroup.alpha = 1f;
            StartCoroutine(DisappearCoroutine());
        }

        private IEnumerator DisappearCoroutine()
        {
            float passedTime = 0;
            
            while (canvasGroup.alpha > 0)
            {
                float time = Time.fixedDeltaTime;
                passedTime += time;
                canvasGroup.alpha = 1f - (passedTime / disappearanceTime > 1f ? 1f : passedTime / disappearanceTime);
                yield return new WaitForSeconds(time);
            }
        }
    }
}