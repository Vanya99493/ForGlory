using UnityEngine;
using UnityEngine.UI;

namespace UIModule.Panels.GameHudModule
{
    public class NextStepButton : MonoBehaviour
    {
        [SerializeField] private Image nextStepImage;
        [SerializeField] private Image waitStepImage;

        public Button Button;

        public void ShowNextStepImage()
        {
            nextStepImage.gameObject.SetActive(true);
            waitStepImage.gameObject.SetActive(false);
        }

        public void ShowWaitStepImage()
        {
            waitStepImage.gameObject.SetActive(true);
            nextStepImage.gameObject.SetActive(false);
        }
    }
}