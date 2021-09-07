using System;
using System.Collections.Generic;
using System.Linq;
using CardControllers;
using UnityEngine;

public class Rules : MonoBehaviour
{
    //set lists for every type of card here and score accordingly
    //use rules to set rules and scoring for player
    private void Start()
    {
        GameManager.Instance.compareButton.onClick.AddListener(CalculateRound);

        foreach (var player in GameManager.Instance.players)
        {
            player.useChopstick.onClick.AddListener(UseChopsticks);
        }
    }

    public void CalculateRound()
    {
        var round = GameManager.Instance.round++;

        CompareSashimi(GameManager.Instance.players);
        CompareTempura(GameManager.Instance.players);
        CompareMakiRoll(GameManager.Instance.players);
        CompareDumpling(GameManager.Instance.players);
        MultiplyWasabi(GameManager.Instance.players);

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

            if (tempuraCount % 2 == 0)
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

        var maxMakiValue = 0;
        foreach (var item in playerScore)
        {
            if (item.Value > maxMakiValue)
            {
                maxMakiValue = item.Value;
                playWinner.Add(item.Key);
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
                if (playLooser.Count != 0)
                {
                    var first = playLooser.Keys.FirstOrDefault();

                    if (first is { }) first.score += maxiRoll.minValue;
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
                    1 => 5,
                    2 => 3,
                    3 => 6,
                    4 => 10,
                    5 => 15,
                    _ => 15
                };
            }
        }
    }

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

                if (wasabiList.Count > 0)
                {
                    if (player.CanUseWasabi())
                        player.score += cardUsed.maxValue * GameManager.Instance.wasabi.maxValue;

                    player.cardsUsedInTurn.Remove(wasabiList.FirstOrDefault());
                }
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

            if (chopstickList.Count > 0)
            {
                if (player.CanUseChopstick())
                    player.cardsToPlayCount = 2;
                
                player.cardsUsedInTurn.Remove(chopstickList.FirstOrDefault());
            }
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
                if (playLooser.Count != 0)
                {
                    var first = playLooser.Keys.FirstOrDefault();

                    if (first is { }) first.score += pudding.minValue;
                }
            }
        }
    }
}