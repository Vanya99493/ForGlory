using CastleModule.ViewPart;
using UnityEngine;

namespace CastleModule.PresenterPart
{
    public class CastleFactory
    {
        public CastleView InstantiateCastle(CastleView castlePrefab, Transform parent)
        {
            CastleView view = Object.Instantiate(castlePrefab, parent);
            view.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
            view.Destroy += OnDestroy;
            return view;
        }

        private void OnDestroy(CastleView view)
        {
            Object.Destroy(view);
        }
    }
}