using PlaygroundModule.ViewPart;
using UnityEngine;

namespace PlaygroundModule.PresenterPart
{
    public class PlaygroundFactory
    {
        public PlaygroundView InstantiatePlayground()
        {
            PlaygroundView playgroundView = new GameObject().AddComponent<PlaygroundView>();
            playgroundView.Destroy += OnDestroy;
            playgroundView.name = "PlaygroundView";
            return playgroundView;
        }

        private void OnDestroy(PlaygroundView gameObject)
        {
            Object.Destroy(gameObject.gameObject);
        }
    }
}