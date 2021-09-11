using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Turn : MonoBehaviour
{
    public static Turn Instance { get; private set; }

    public UnityEvent CallNextPlayer;
    private bool lastPlayer;

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
        CallNextPlayer.AddListener(NextPlayerInTurn);
    }

    private void NextPlayerInTurn()
    {
        var p = 0;
        
        for (var i = 0; i < GameManager.Instance.players.Count; i++)
        {
            var playerPlaying = GameManager.Instance.players[i];

            if (playerPlaying == GameManager.Instance.activePlayer)
            {
                playerPlaying.gameObject.SetActive(false);
                p = i;
                break;
            }
        }
        
        if (p == GameManager.Instance.players.Count - 1)
        {
            p = 0;
            lastPlayer = true;
            Debug.Log("back to first player");
        }
        else
            p += 1;

        var nextPlayer = GameManager.Instance.players[p];
        nextPlayer.gameObject.SetActive(true);
        GameManager.Instance.activePlayer = nextPlayer;

        if (lastPlayer)
        {
            GameManager.Instance.CallEndTurn.Invoke();
            lastPlayer = false;
            Debug.Log("exchanging cards");
        }

        GameManager.Instance.ShowNextPlayerHand();
    }
}