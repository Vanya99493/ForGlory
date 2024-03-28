using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UIModule.Panels.CastleMenuModule
{
    public class TeamPositionArea : MonoBehaviour, IDropHandler
    {
        //public event Action<HeroCard> SetHero;
        
        public void OnDrop(PointerEventData eventData)
        {
            if (eventData.pointerDrag != null)
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).transform.SetParent(eventData.pointerDrag.GetComponent<HeroCard>().LastParent);
                }
                eventData.pointerDrag.transform.SetParent(gameObject.transform);
                eventData.pointerDrag.transform.position = gameObject.transform.position;
                
                //SetHero?.Invoke(eventData.pointerDrag.GetComponent<HeroCard>());
            }
        }
    }
}