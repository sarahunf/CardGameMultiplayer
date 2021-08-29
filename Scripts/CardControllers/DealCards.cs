using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace CardControllers
{
    public class DealCards : MonoBehaviour
    {
        private int cardsToDeal;
        [SerializeField] private GameObject cardPrefab;
        public CardsDealt cardsDealt;

        private void Awake()
        {
            cardsDealt = this.GetComponent<CardsDealt>();
        }

        private void Start()
        {
            CalculateCardsQuantity();
        }

        private void CalculateCardsQuantity()
        {
            cardsToDeal = GameSetup.Instance.playersInGame switch
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
            for (var i = 0; i < cardsToDeal; i++)
            {
                var card = CardsDeck.Deck[Random.Range(0, CardsDeck.Deck.Count)];
                switch (CardsDeck.ContainsCardOnDeck(card))
                {
                    case true:
                        cardsDealt.currentCards.Add(card);
                        CardsDeck.RemoveCardFromDeck(card);
                        break;
                    case false:
                        return;
                }
            }
        }

        private void ShowCardsInHand()
        {
            foreach (var card in cardsDealt.currentCards)
            {
                cardPrefab.GetComponent<CardDisplay>().card = card;
                Instantiate(cardPrefab, this.transform);
            }
        }

        public void Deal()
        {
            SetHand();
            ShowCardsInHand();
        }
    }
}