using CardControllers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Interface
{
    public class DropSlot : MonoBehaviour, IDropHandler
    {
        public void OnDrop(PointerEventData eventData)
        {
            //this should be in a game controller. Not in the drop slot
            //pode ser por um EVENTO AQUI!!!(add event)
            
            
            if (eventData.pointerDrag != null && transform.childCount < GameSetup.Instance.players.Count)
            {
                eventData.pointerDrag.GetComponentInParent<Player>().cardsUsedInTurn.Add(eventData.pointerDrag.GetComponent<CardDisplay>().card);
                eventData.pointerDrag.GetComponentInParent<Player>().currentCardsInHand.Remove(eventData.pointerDrag.GetComponent<CardDisplay>().card);
                eventData.pointerDrag.GetComponent<Transform>().SetParent(transform);
                return;
            }
            eventData.pointerDrag.GetComponent<Transform>().position = eventData.pointerDrag.GetComponent<DragDrop>().originalPos;
        }
    }
}