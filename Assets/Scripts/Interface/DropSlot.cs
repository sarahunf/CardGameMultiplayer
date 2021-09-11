using System.Collections;
using CardControllers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Interface
{
    public class DropSlot : MonoBehaviour, IDropHandler
    {
        public void OnDrop(PointerEventData eventData)
        {
            var slotsAvailable = GameManager.Instance.activePlayer.cardsToPlayCount;

            if (eventData.pointerDrag != null && transform.childCount < slotsAvailable)
            {
                GameManager.Instance.UpdatePlayersHand(eventData.pointerDrag.GetComponentInParent<Player>(),
                    eventData.pointerDrag.GetComponent<CardDisplay>().card);
                eventData.pointerDrag.GetComponent<Transform>().SetParent(transform);
                
                Turn.Instance.CallNextPlayer.Invoke();
                return;
            }

            eventData.pointerDrag.GetComponent<Transform>().position =
                eventData.pointerDrag.GetComponent<DragDrop>().originalPos;
        }
    }
}