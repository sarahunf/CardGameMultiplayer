using System;
using System.Collections.Generic;
using UnityEngine;

namespace CardControllers
{
    public abstract class ShowCards : MonoBehaviour
    {
        public abstract void DisplayCards(Player player, List<Card> hand);

        protected virtual void HideCards()
        {
            GameManager.Instance.reseter.ResetUI();
        }
    }
}