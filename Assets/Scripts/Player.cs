using System.Collections.Generic;
using System.Linq;
using CardControllers;
using UnityEngine.UI;

public class Player : CardsInTurn
{
    protected int pid { get; private set; }

    public int score;
    public int cardsToPlayCount = 1;

    internal Dictionary<Card, int> cardRoundsActive = new Dictionary<Card, int>();
    private bool _canUseChopstick;
    private bool _canUseWasabi;

 

    private void Start()
    {
        btShowCardsOnHand.onClick.AddListener(ShowMyHand);
        btShowCardsOnTable.onClick.AddListener(ShowMyTable);
        btFinishTurn.interactable = false;
        btFinishTurn.onClick.AddListener(FinishMyTurn);
    }

    public void UpdateCards(Card card)
    {
        cardsUsedInTurn.Add(card);
        cardsUsedOnTable.Add(card);
        currentCardsInHand.Remove(card);
        lastUsedCard = card;

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
            btUseChopstick.interactable = true;
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

    private void FinishMyTurn()
    {
        Turn.Instance.callNextPlayer.Invoke();
        btFinishTurn.interactable = false;
    }

    private void ShowMyHand()
    {
        GameManager.Instance.dealer.DisplayCards(this,currentCardsInHand);
    }

    private void ShowMyTable()
    {
        GameManager.Instance.dealer.DisplayCards(this,cardsUsedInTurn);
    }

    internal void ResetAfterTurn()
    {
        currentCardsInHand.Clear();
        cardsUsedInGame.AddRange(cardsUsedInTurn);
        cardsUsedInTurn.Clear();
    }
}