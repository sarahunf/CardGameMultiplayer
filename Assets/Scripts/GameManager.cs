using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using CardControllers;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] internal Canvas canvas;
    [SerializeField] internal Button dealButton;
    [SerializeField] internal Button resetButton;
    [SerializeField] internal Button compareButton;
    [SerializeField] internal Button finishTurnButton;

    public UnityEvent CallEndTurn;
    public UnityEvent CallExchangeCards;

    public Transform playersParent;
    public List<Player> players;
    [SerializeField] internal int PlayersInGame;

    internal int round;
    internal int lastRound;
    private int handsChange;

    [SerializeField] internal Card chopsticks;
    [SerializeField] internal Card wasabi;
    internal Player activePlayer;

    internal ResetCards reseter;
    private DealCards dealer;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        reseter = this.GetComponent<ResetCards>();
        dealer = this.GetComponent<DealCards>();
        var playersInScene = playersParent.GetComponentsInChildren<Player>(true);
        foreach (var player in playersInScene)
        {
            players.Add(player);
        }

        PlayersInGame = players.Count;
        activePlayer = playersInScene[0];
        finishTurnButton.onClick.AddListener(EndTurn);
        CallEndTurn.AddListener(EndTurn);
        CallExchangeCards.AddListener(ExchangePlayersHand);
    }

    public void UpdatePlayersHand(Player player, Card card)
    {
        player.UpdateCards(card);
    }

    private void EndTurn()
    {
        handsChange++;

        if (handsChange > dealer.CalculateCardsQuantity() - 1)
        {
            handsChange = 0;
            reseter.Reset();
            dealer.Deal();
            var rules = this.GetComponent<Rules>();
            rules.CalculateRound();
            return;
        }

        ExchangePlayersHand();

        //if last card on hand call deal cards
    }

    private void ExchangePlayersHand()
    {
        var handsList = players.Select(player => player.currentCardsInHand).ToList();

        foreach (var player in players)
        {
            player.currentCardsInHand = handsList.Last();
            handsList.RemoveAt(handsList.Count - 1);
        }

        foreach (var player in players)
        {
            player.cardsToPlayCount = 1;
            dealer.ShowCardInHand(player, player.currentCardsInHand);
        }
    }

    internal void ShowNextPlayerHand()
    {
        reseter.ResetUI();
        dealer.ShowCardInHand(activePlayer, activePlayer.currentCardsInHand);
    }
}