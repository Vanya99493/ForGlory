using UnityEngine;
using UnityEngine.EventSystems;

namespace UIModule.Panels.CastleMenuModule
{
    public class CastleGarrisonPanel : MonoBehaviour, IDropHandler
    {
        public void OnDrop(PointerEventData eventData)
        {
            if (eventData.pointerDrag != null)
            {
                eventData.pointerDrag.transform.SetParent(gameObject.transform);
            }
        }
    }
}