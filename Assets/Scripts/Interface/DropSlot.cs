using CardControllers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Interface
{
    public class DropSlot : MonoBehaviour, IDropHandler
    {
        public void OnDrop(PointerEventData eventData)
        {
            if (eventData.pointerDrag != null && transform.childCount < GameManager.Instance.players.Count)
            {
                GameManager.Instance.UpdatePlayersHand(eventData.pointerDrag.GetComponentInParent<Player>(),
                    eventData.pointerDrag.GetComponent<CardDisplay>().card);
                
                eventData.pointerDrag.GetComponent<Transform>().SetParent(transform);
                return;
            }

            eventData.pointerDrag.GetComponent<Transform>().position =
                eventData.pointerDrag.GetComponent<DragDrop>().originalPos;
        }
    }
}