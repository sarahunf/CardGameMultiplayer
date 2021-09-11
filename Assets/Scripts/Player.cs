using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using CardControllers;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : CardsInTurn
{
    protected int pid { get; private set; }

    public int score;
    public int cardsToPlayCount = 1;

    internal Dictionary<Card, int> cardRoundsActive = new Dictionary<Card, int>();
    private bool _canUseChopstick;
    private bool _canUseWasabi;

    public void UpdateCards(Card card)
    {
        this.cardsUsedInTurn.Add(card);
        this.cardsUsedOnTable.Add(card);
        this.currentCardsInHand.Remove(card);
        this.lastUsedCard = card;

        UpdateTurn(card);
    }

    private void UpdateTurn(Card card)
    {
        if (!cardRoundsActive.ContainsKey(card))
            cardRoundsActive.Add(card, 0);

        var listOfCardSToModify = cardRoundsActive.Select(kvp => kvp.Key).ToList();

        foreach (var key in listOfCardSToModify)
        {
            cardRoundsActive[key]++;
        }

        UpdateChopstick();
        UpdateWasabi();
    }

    private void UpdateChopstick()
    {
        var card = GameManager.Instance.chopsticks;

        if (!cardRoundsActive.ContainsKey(card)) return;
        var value = cardRoundsActive[card];

        if (value > 1)
        {
            _canUseChopstick = true;
            useChopstick.interactable = true;
        }
    }

    private void UpdateWasabi()
    {
        var card = GameManager.Instance.wasabi;

        if (!cardRoundsActive.ContainsKey(card)) return;
        var value = cardRoundsActive[card];

        if (value > 1)
            _canUseWasabi = true;
    }

    internal bool CanUseChopstick()
    {
        return _canUseChopstick;
    }

    internal bool CanUseWasabi()
    {
        return _canUseWasabi;
    }
}