using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using CardControllers;
using UnityEngine;
using UnityEngine.Events;
using Debug = UnityEngine.Debug;

public class Rules : MonoBehaviour
{
    //set lists for every type of card here and score accordingly
    //use rules to set rules and scoring for player

    //RULES ARE NOT WORKING PROPERLY. DEBUG IT.
    private void Start()
    {
        GameManager.Instance.compareButton.onClick.AddListener(CalculateRound);

        foreach (var player in GameManager.Instance.players)
        {
            player.btUseChopstick.onClick.AddListener(UseChopsticks);
        }
    }

    public void CalculateRound()
    {
        var round = GameManager.Instance.round++;

        CompareSashimi(GameManager.Instance.players);
        DebugPlayer("CompareSashimi");
        CompareTempura(GameManager.Instance.players);
        DebugPlayer("CompareTempura");
        CompareMakiRoll(GameManager.Instance.players);
        DebugPlayer("CompareMakiRoll");
        CompareDumpling(GameManager.Instance.players);
        DebugPlayer("CompareDumpling");
        CompareNigiri(GameManager.Instance.players);
        DebugPlayer("CompareNigiri");
        //MultiplyWasabi(GameManager.Instance.players);

        if (round == 3)
        {
            ComparePudding(GameManager.Instance.players);
            //CountScore();
        }
    }

    private void CompareSashimi(List<Player> players)
    {
        if (players == null) return;

        foreach (var player in players)
        {
            var sashimiCount = player.cardsUsedInTurn.Count(card => card.name == "Sashimi");

            if (sashimiCount % 3 == 0)
            {
                switch (sashimiCount / 3)
                {
                    case 1:
                        player.score += 10;
                        break;
                    case 2:
                        player.score += 20;
                        break;
                    case 3:
                        player.score += 30;
                        break;
                    case 4:
                        player.score += 40;
                        break;
                }
            }
        }
    }

    private void CompareTempura(List<Player> players)
    {
        if (players == null) return;

        foreach (var player in players)
        {
            var tempuraCount = player.cardsUsedInTurn.Count(card => card.name == "Tempura");

            if (tempuraCount % 2 == 0 && tempuraCount > 0)
            {
                switch (tempuraCount / 2)
                {
                    case 1:
                        player.score += 5;
                        break;
                    case 2:
                        player.score += 10;
                        break;
                    case 3:
                        player.score += 15;
                        break;
                    case 4:
                        player.score += 20;
                        break;
                    case 5:
                        player.score += 25;
                        break;
                }
            }
        }
    }

    private void CompareMakiRoll(List<Player> players)
    {
        var maxiRoll = ScriptableObject.CreateInstance<Card>();

        var playerScore = new Dictionary<Player, int>();
        var playWinner = new List<Player>();
        var playLooser = new Dictionary<Player, int>();

        if (players == null) return;

        foreach (var player in players)
        {
            var makiCards = player.cardsUsedInTurn.Where(card => card.name == "Maki Rolls").ToList();
            {
                var currentMakiCount = makiCards.Sum(makiCard => makiCard.quantity);
                playerScore.Add(player, currentMakiCount);
            }
        }

        //changed to for loop to acces last index, but it must compare to all indexes that came before.
        //maybe a int list would be better so it can compare my value to a list in a foreach loop
        var maxMakiValue = 0;
        for (int i = 0; i < playerScore.Count; i++)
        {
            var item = playerScore.ElementAt(i);
            
            if (item.Value > maxMakiValue)
            {
                maxMakiValue = item.Value;
                playWinner.Add(item.Key);
                if (i <= 0) continue;
                var itemLast = playerScore.ElementAt(i - 1);
                if (itemLast.Value >= item.Value || itemLast.Key == null) continue;
                playWinner.Remove(itemLast.Key);
                playLooser.Add(itemLast.Key, itemLast.Value);
            }
            else
                 playLooser.Add(item.Key, item.Value);
        }
        
        var secondMakiValue = 0;
        if (playWinner.Count > 1)
        {
            var score = Convert.ToInt32(Math.Floor((double) (maxiRoll.maxValue / playWinner.Count)));
            foreach (var player in playWinner)
            {
                player.score += score;
            }
        }
        else if (playWinner.Count == 1)
        {
            playWinner[0].score += maxiRoll.maxValue;

            foreach (var item in playerScore.Where(item => item.Value >= secondMakiValue))
            {
                secondMakiValue = item.Value;
                playWinner.Add(item.Key);
            }

            if (playLooser.Count > 1)
            {
                var score = Convert.ToInt32(Math.Floor((double) (maxiRoll.minValue / playWinner.Count)));
                foreach (var kvp in playLooser)
                {
                    kvp.Key.score += score;
                }
            }
            else
            {
                if (playLooser.Count == 0) return;
                var first = playLooser.Keys.FirstOrDefault();

                if (first is { }) first.score += maxiRoll.minValue;
            }
        }
    }

    //take wasabi in account!!!!!
    //maybe if used with wasabi card gets an bool and will be ignored here.
    private void CompareNigiri(List<Player> players)
    {
        if (players == null) return;
        foreach (var player in players)
        {
            var value = 0;

            var eggNigiriCount = player.cardsUsedInTurn.Count(card => card.name == "Egg Nigiri");

            if (eggNigiriCount > 0)
            {
                value = 1;
                for (var i = 0; i < eggNigiriCount; i++)
                {
                    player.score += value;
                }
            }

            var salmonNigiriCount = player.cardsUsedInTurn.Count(card => card.name == "Salmon Nigiri");

            if (salmonNigiriCount > 0)
            {
                value = 2;
                for (var i = 0; i < salmonNigiriCount; i++)
                {
                    player.score += value;
                }
            }

            var squidNigiriCount = player.cardsUsedInTurn.Count(card => card.name == "Squid Nigiri");

            if (squidNigiriCount <= 0) continue;
            {
                value = 3;
                for (var i = 0; i < squidNigiriCount; i++)
                {
                    player.score += value;
                }
            }
        }
    }

    private void CompareDumpling(List<Player> players)
    {
        if (players == null) return;
        foreach (var player in players)
        {
            var dumplings = player.cardsUsedInTurn.Count(card => card.name == "Dumpling");

            if (dumplings > 0)
            {
                player.score += dumplings switch
                {
                    1 => 1,
                    2 => 3,
                    3 => 6,
                    4 => 10,
                    5 => 15,
                    _ => 15
                };
            }
        }
    }

    //change wasabi logic to bool on card not last card used
    //or call this when its used, not at the end fo the turn
    private void MultiplyWasabi(List<Player> players)
    {
        if (players == null) return;
        var wasabiList = new List<Card>();

        foreach (var player in players)
        {
            var cardUsed = player.lastUsedCard;

            if (cardUsed != null && cardUsed.name.Contains("Nigiri"))
            {
                wasabiList.AddRange(player.cardsUsedInTurn.Where(card => card.name == "Wasabi"));

                if (wasabiList.Count <= 0) continue;
                if (player.CanUseWasabi())
                    player.score += cardUsed.maxValue * GameManager.Instance.wasabi.maxValue;

                player.cardsUsedInTurn.Remove(wasabiList.FirstOrDefault());
            }
        }
    }

    private void UseChopsticks()
    {
        var players = GameManager.Instance.players;
        if (players == null) return;
        var chopstickList = new List<Card>();

        foreach (var player in players)
        {
            chopstickList.AddRange(player.cardsUsedInTurn.Where(card => card.name == "Chopsticks"));

            if (chopstickList.Count <= 0) continue;
            if (player.CanUseChopstick())
                player.cardsToPlayCount = 2;

            player.cardsUsedInTurn.Remove(chopstickList.FirstOrDefault());
        }
    }

    private void ComparePudding(List<Player> players)
    {
        if (players == null) return;

        var pudding = ScriptableObject.CreateInstance<Card>();
        var playerScore = new Dictionary<Player, int>();
        var playWinner = new List<Player>();
        var playLooser = new Dictionary<Player, int>();

        foreach (var player in players)
        {
            var puddingCards = player.cardsUsedInGame.Where(card => card.name == "Pudding").ToList();
            {
                var count = puddingCards.Sum(cards => cards.quantity);
                playerScore.Add(player, count);
            }
        }

        var maxPuddingValue = 0;
        foreach (var item in playerScore)
        {
            if (item.Value > maxPuddingValue)
            {
                maxPuddingValue = item.Value;
                playWinner.Add(item.Key);
            }
            else
                playLooser.Add(item.Key, item.Value);
        }

        var secondPuddingValue = 0;
        if (playWinner.Count > 1)
        {
            var score = Convert.ToInt32(Math.Floor((double) (pudding.maxValue / playWinner.Count)));
            foreach (var player in playWinner)
            {
                player.score += score;
            }
        }
        else if (playWinner.Count == 1)
        {
            playWinner[0].score += pudding.maxValue;

            foreach (var item in playerScore.Where(item => item.Value >= secondPuddingValue))
            {
                secondPuddingValue = item.Value;
                playWinner.Add(item.Key);
            }

            if (playLooser.Count > 1)
            {
                var score = Convert.ToInt32(Math.Floor((double) (pudding.minValue / playWinner.Count)));
                foreach (var kvp in playLooser)
                {
                    kvp.Key.score += score;
                }
            }
            else
            {
                if (playLooser.Count == 0) return;
                var first = playLooser.Keys.FirstOrDefault();

                if (first is { }) first.score += pudding.minValue;
            }
        }
    }

    private void DebugPlayer(string callingMethod)
    {
        foreach (var player in GameManager.Instance.players)
        {
            Debug.LogError("Player: " + player + " score: " + player.score + " calling method: " + callingMethod);
        }

    }
}