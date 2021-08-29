using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace CardControllers
{
    public abstract class CardsDealt : MonoBehaviour
    {
       public List<Card> currentCards =  new List<Card>();
    }
}

