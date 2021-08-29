using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace CardControllers
{
    public class ResetCards : MonoBehaviour
    {
        //working, but poor logic and performance. Check a better way later
        public void Reset()
        {
            //check if cards have been dealt
            
            ResetLogic();
            ResetUI();
            CardsDeck.ShowCurrentCardsOnDeck();
        }

        private void ResetLogic()
        {
            
            var handsDealt = FindObjectsOfType<CardsDealt>();
            foreach (var hand in handsDealt)
            {
                hand.currentCards.Clear();
            }
        }

        private void ResetUI()
        {
            var cardDisplay = FindObjectsOfType<CardDisplay>();
            foreach (var card in cardDisplay)
            {
                Destroy(card.gameObject);
            }
        }
    }
}