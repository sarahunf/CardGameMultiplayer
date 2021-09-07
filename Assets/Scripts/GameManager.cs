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
    [SerializeField] internal Button useChopsticks;
    public List<Player> players;
    internal int PlayersInGame;
    internal int round;
    internal int lastRound;
    private int handsChange;
    [SerializeField] internal Card chopsticks;
    [SerializeField] internal Card wasabi;

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
        var playersInScene = FindObjectsOfType<Player>();
        foreach (var player in playersInScene)
        {
            players.Add(player);
        }

        PlayersInGame = players.Count;
        finishTurnButton.onClick.AddListener(EndTurn);
    }

    public void UpdatePlayersHand(Player player, Card card)
    {
        player.UpdateCards(card);
    }

    private void EndTurn()
    {
        var reseter = this.GetComponent<ResetCards>();
        var dealer = this.GetComponent<DealCards>();
        reseter.ResetUI();
        
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
        
        List<List<Card>> handsList = players.Select(player => player.currentCardsInHand).ToList();
        
        foreach (var player in players)
        {
            player.currentCardsInHand = handsList.Last();
            handsList.RemoveAt(handsList.Count - 1);
        }

        foreach (var player in players)
        {
            player.cardsToPlayCount = 1;
            dealer.ShowCardInHand(player,player.currentCardsInHand);
        }
        
        //if last card on hand call deal cards
    }
}