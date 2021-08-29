using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace CardControllers
{
    public class DealCards : MonoBehaviour
    {
        private int cardsToDeal;
        [SerializeField] private GameObject cardPrefab;

        private void CalculateCardsQuantity()
        {
            cardsToDeal = GameSetup.Instance.PlayersInGame switch
            {
                2 => 10,
                3 => 9,
                4 => 8,
                5 => 7,
                _ => 10
            };
        }

        private void SetHand()
        {
            foreach (var player in GameSetup.Instance.players)
            {
                for (var i = 0; i < cardsToDeal; i++)
                {
                    var card = CardsDeck.Deck[Random.Range(0, CardsDeck.Deck.Count)];
                    switch (CardsDeck.ContainsCardOnDeck(card))
                    {
                        case true:
                            player.currentCards.Add(card);
                            CardsDeck.RemoveCardFromDeck(card);
                            
                            ShowCardInHand(player,card);
                            break;
                        case false:
                            return;
                    }
                }
            }
        }

        private void ShowCardInHand(Component player, Card card)
        {
                Instantiate(cardPrefab, player.transform);
                cardPrefab.GetComponent<CardDisplay>().card = card;
        }

        public void Deal()
        {
            CalculateCardsQuantity();
            SetHand();
        }
    }
}