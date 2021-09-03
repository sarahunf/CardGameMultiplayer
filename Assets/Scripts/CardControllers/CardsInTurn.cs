using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace CardControllers
{
    public abstract class CardsInTurn : MonoBehaviour
    {
       public List<Card> currentCardsInHand =  new List<Card>();
       public List<Card> cardsUsedInTurn = new List<Card>();
       public List<Card> cardsUsedInGame = new List<Card>();
       public Card lastUsed;
    }
}

