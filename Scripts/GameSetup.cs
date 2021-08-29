using System;
using System.Collections.Generic;
using CardControllers;
using UnityEngine;


public class GameSetup : MonoBehaviour
{
    public static GameSetup Instance { get; private set; }
    [SerializeField] internal int playersInGame = 2;
    public Card[] cards;
    private List<Player> Players;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);
        
        CardsDeck.SetCurrentDeckInGame(cards);
    }

    private void Start()
    {
        var players = FindObjectsOfType<Player>();
        foreach (var player in players)
        {
            Players.Add(player);
        }
    }
}