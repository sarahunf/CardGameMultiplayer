using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace CardControllers
{
    public abstract class CardsInTurn : MonoBehaviour
    {
       public List<Card> currentCardsInHand =  new List<Card>();
       public List<Card> cardsUsedInTurn = new List<Card>();
       public List<Card> cardsUsedInGame = new List<Card>();
       public List<Card> cardsUsedOnTable = new List<Card>();
       public Card lastUsedCard;
       public Button btUseChopstick;
       public Button btShowCardsOnHand;
       public Button btShowCardsOnTable;
       public Button btFinishTurn;
    }
}

