using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace CardControllers
{
    public class DealCards : MonoBehaviour
    {
        private int cardsToDeal;
        [SerializeField] private GameObject cardPrefab;

        private void Start()
        {
           GameManager.Instance.dealButton.onClick.AddListener(Deal);
        }

        internal int CalculateCardsQuantity()
        {
            cardsToDeal = GameManager.Instance.PlayersInGame switch
            {
                2 => 10,
                3 => 9,
                4 => 8,
                5 => 7,
                _ => 10
            };
            return cardsToDeal;
        }

        //está instanciando uma ultima carta errada sempre
        private void SetHand()
        {
            CardsDeck.ShuffleDeck();
            foreach (var player in GameManager.Instance.players.Where(player => player.currentCardsInHand.Count == 0))
            {
                for (var i = 0; i < cardsToDeal; i++)
                {
                    var card = CardsDeck.Deck[0];
                    player.currentCardsInHand.Add(card);
                    CardsDeck.RemoveCardFromDeck(card);
                }
                ShowCardInHand(player, player.currentCardsInHand);
            }
        }

        public void ShowCardInHand(Player player, List<Card> hand)
        {
            foreach (var card in hand)
            {
                cardPrefab.GetComponent<CardDisplay>().card = card;
                Instantiate(cardPrefab, player.transform);
            }
        }

        internal void Deal()
        {
            CalculateCardsQuantity();
            SetHand();
        }
    }
}