using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace CardControllers
{
    public class ResetCards : MonoBehaviour
    {
        private void Start()
        {
            GameManager.Instance.resetButton.onClick.AddListener(Reset);
        }
        public void Reset()
        {
            //check if cards have been dealt
            ResetLogic();
            ResetUI();
        }

        private void ResetLogic()
        {
            
            var handsDealt = FindObjectsOfType<CardsInTurn>();
            foreach (var hand in handsDealt)
            {
                hand.currentCardsInHand.Clear();
            }
        }

        internal void ResetUI()
        {
            var cardDisplay = FindObjectsOfType<CardDisplay>();
            foreach (var card in cardDisplay)
            {
                Destroy(card.gameObject);
            }
        }
    }
}