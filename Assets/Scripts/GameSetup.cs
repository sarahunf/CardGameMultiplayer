using System;
using System.Collections.Generic;
using CardControllers;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;


public class GameSetup : MonoBehaviour
{
    public static GameSetup Instance { get; private set; }
    internal int PlayersInGame;
    public Card[] cards;
    public List<Player> players;

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
        var playersInScene = FindObjectsOfType<Player>();
        foreach (var player in playersInScene)
        {
            players.Add(player);
        }
        PlayersInGame = players.Count;
    }
}