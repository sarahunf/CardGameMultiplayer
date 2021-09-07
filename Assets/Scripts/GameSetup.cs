using System;
using System.Collections.Generic;
using CardControllers;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;


public class GameSetup : MonoBehaviour
{
    public static GameSetup Instance { get; private set; }
    public Card[] cards;

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
}