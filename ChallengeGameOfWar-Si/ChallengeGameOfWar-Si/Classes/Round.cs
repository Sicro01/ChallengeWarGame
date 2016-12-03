using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChallengeGameOfWar_Si.Classes;

namespace ChallengeGameOfWar_Si.Classes
{
    public class Round
    {
        public string TypeOfRound { get; set; } = "Normal Round";
        public int RoundNumber { get; set; }
        public static int CumulativeRoundNumber { get; set; } = 0;
        public static int NumberOfWarRounds { get; set; }
        public List<Player> Players { get; set; }
        public List<Card> PlayerFaceCards { get; set; }
        public List<int> PlayerEndOfRoundCardCount { get; set; }
        public bool Player0IsTheWinner { get; set; } = false;
        public bool Player1IsTheWinner { get; set; } = false;
        public bool ItsADraw { get; set; } = false;
        public bool ItsWar { get; set; } = false;

        public Round()
        {
            Players = new List<Player>();
            PlayerFaceCards = new List<Card>();
            PlayerEndOfRoundCardCount = new List<int>();
        }


        public Round playARound(Round round)
        {
            //Deal next Face Cards and Compare them
            round = getAndCompareNextCards(round);

            //Winner/Loser OR Possibly War
            if (round.Player0IsTheWinner)
            {
                //Player 0 Wins - Add both facecards into Player 0's Hand
                return endOfRoundUpdates(round, round.Players[0]);
            }
            //else if (round.Player1IsTheWinner)
            else
            {
                //Player 1 Wins - Add both facecards into Player 1's Hand
                return endOfRoundUpdates(round, round.Players[1]);
            }
            //else if (round.ItsADraw)
            //{
            //    //See if we can play War - so long as each player each at least 4 Cards and we've played less than 20 rounds of War already
            //    //If we've played 20 already then the winner is the one with the most Cards
            //    return round;
            //    //playAWarRound(round);
            //}
            
            return round;
        }

        private Round getAndCompareNextCards(Round round)
        {
            //Take a card from each Player's Hand - setting up the Round's Face Cards
            round = dealACard(round, round.Players[0]);
            round = dealACard(round, round.Players[1]);

            //Compare Face Cards - setting bools used to determine path
            round.Player0IsTheWinner = (round.PlayerFaceCards[0].Rank > round.PlayerFaceCards[1].Rank) ? true : false;
            round.Player1IsTheWinner = (round.PlayerFaceCards[1].Rank > round.PlayerFaceCards[0].Rank) ? true : false;
            round.ItsADraw = (!Player0IsTheWinner && !Player1IsTheWinner) ? true : false;

            return round;
        }

        private Round dealACard (Round round, Player player)
        {
            //Take the next card from the Player's Hand (index 0) and store it in the Round's Face Card - then remove the Card from the Player's Hand
            round.PlayerFaceCards.Add(player.PlayerHand[0]);
            player.PlayerHand.RemoveAt(0);
            //Check if they can still play War
            player.InsufficientCardsToPlayWar = (player.PlayerHand.Count < 4) ? true : false;

            return round;
        }
        private Player addCardsToPlayerHand (Player player, List<Card> list)
        {
            foreach (Card card in list)
            {
                player.PlayerHand.Add(card);
            }
            //Check if they can still play War
            player.InsufficientCardsToPlayWar = (player.PlayerHand.Count < 4) ? true : false;
            return player;
        }
        private Round endOfRoundUpdates(Round round, Player winningPlayer)
        {
            //Increase overall number of Rounds in this Game
            CumulativeRoundNumber++;
            winningPlayer = addCardsToPlayerHand(winningPlayer, round.PlayerFaceCards);
            round.PlayerEndOfRoundCardCount.Add(round.Players[0].PlayerHand.Count);
            round.PlayerEndOfRoundCardCount.Add(round.Players[1].PlayerHand.Count);
            round.RoundNumber = CumulativeRoundNumber;
            return round;
        }
    }
}