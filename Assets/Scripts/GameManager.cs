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

    public UnityEvent callEndTurn;

    public Transform playersParent;
    public List<Player> players;
    [SerializeField] internal int playersInGame;

    [SerializeField] internal int round;
    internal int lastRound;
    private int handsChange;

    [SerializeField] internal Card chopsticks;
    [SerializeField] internal Card wasabi;
    internal Player activePlayer;

    internal ResetCards reseter;
    internal DealCards dealer;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        reseter = GetComponent<ResetCards>();
        dealer = GetComponent<DealCards>();
        var playersInScene = playersParent.GetComponentsInChildren<Player>(true);
        foreach (var player in playersInScene)
        {
            players.Add(player);
        }

        playersInGame = players.Count;
        activePlayer = playersInScene[0];
        callEndTurn.AddListener(EndTurn);
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
            var rules = GetComponent<Rules>();
            rules.CalculateRound();

            //reseting players hand
            foreach (var player in players)
            {
                player.ResetAfterTurn();
            }

            return;
        }

        ExchangePlayersHand();
    }

    //on the end of the turn card exchange goes one step further (player 1 hand goes to player 3, for ex)
    // if more than 2 players. Problem is on Last()?
    private void ExchangePlayersHand()
    {
        var handsList = players.Select(player => player.currentCardsInHand).ToList();
        
        foreach (var player in players)
        {
            player.currentCardsInHand = handsList.Last();
            handsList.RemoveAt(handsList.Count - 1);
            player.cardsToPlayCount = 1;
        }
    }

    internal void ShowNextPlayerHand()
    {
        reseter.ResetUI();
        dealer.DisplayCards(activePlayer, activePlayer.currentCardsInHand);
    }
}