using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace CardControllers
{
    public struct CardsDeck
    {
         public static List<Card> Deck { get; private set; }

        private CardsDeck(Card[] cards)
        {
            Deck = new List<Card>();
            foreach (var card in cards)
            {
                for (var i = 0; i < card.maxQuantityInGame; i++)
                    Deck.Add(card);
            }
        }

        public static CardsDeck SetCurrentDeckInGame(Card[] cardsList)
        {
            return new CardsDeck(cardsList);
        }

        private CardsDeck(Card card)
        {
            if (Deck == null || !Deck.Contains(card)) return;
            Deck.Remove(card);
        }

        public static bool ContainsCardOnDeck(Card card)
        {
            return Deck.Contains(card);
        }

        public static CardsDeck RemoveCardFromDeck(Card card)
        {
            return new CardsDeck(card);
        }

        public static void ShowCurrentCardsOnDeck()
        {
            foreach (var card in Deck)
            {
                Debug.Log(card.name);
            }
        }

        public static void ShuffleDeck()
        {
             Deck.Shuffle();
        }
    }
}