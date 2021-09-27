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
                var player = eventData.pointerDrag.GetComponentInParent<Player>();
                var card = eventData.pointerDrag.GetComponent<CardDisplay>().card;
                
                GameManager.Instance.UpdatePlayersHand(player, card);
                eventData.pointerDrag.GetComponent<Transform>().SetParent(transform);
                player.btFinishTurn.interactable = true;
            }
        }
    }
}