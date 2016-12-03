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
        public int IndexOfLoserUnableToPlayWar { get; set; }
        public bool PlayerHasInsufficnetCardsToPlayWar { get; set; } = false;

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
            else if (round.Player1IsTheWinner)
            {
                //Player 1 Wins - Add both facecards into Player 1's Hand
                return endOfRoundUpdates(round, round.Players[1]);
            }
            else if (round.ItsADraw)
            {
                //    //See if we can play War - so long as each player each at least 4 Cards and we've played less than 20 rounds of War already
                //    //If we've played 20 already then the winner is the one with the most Cards
                round = testIfWePlayWar(round);

                return playWar(round);
            }

                return round;
        }

        private Round getAndCompareNextCards(Round round)
        {
            //Take a card from each Player's Hand - setting up the Round's Face Cards
            round = dealACard(round, round.Players[0],1);
            round = dealACard(round, round.Players[1],1);

            //Compare Face Cards - setting bools used to determine path
            round.Player0IsTheWinner = (round.PlayerFaceCards[0].Rank > round.PlayerFaceCards[1].Rank) ? true : false;
            round.Player1IsTheWinner = (round.PlayerFaceCards[1].Rank > round.PlayerFaceCards[0].Rank) ? true : false;
            round.ItsADraw = (!Player0IsTheWinner && !Player1IsTheWinner) ? true : false;

            return round;
        }

        private Round dealACard (Round round, Player player, int numberOFCardsToDeal)
        {
            for (int i = 0; i < numberOFCardsToDeal; i++)
            {
                //Take the next card from the Player's Hand (index 0) and store it in the Round's Face Card - then remove the Card from the Player's Hand
                round.PlayerFaceCards.Add(player.PlayerHand[0]);
                player.PlayerHand.RemoveAt(0);
            }
            
            //Check if they can still play War
            player.SufficientCardsToPlayWar = (player.PlayerHand.Count > 3) ? true: false;

            return round;
        }
        private Player addCardsToWinningPlayerHand (Player winningPlayer, List<Card> list)
        {
            foreach (Card card in list)
            {
                winningPlayer.PlayerHand.Add(card);
            }
            //Check if they can still play War
            winningPlayer.SufficientCardsToPlayWar = (winningPlayer.PlayerHand.Count > 3) ? true : false;
            return winningPlayer;
        }
        private Round endOfRoundUpdates(Round round, Player winningPlayer)
        {
            //Increase overall number of Rounds in this Game
            CumulativeRoundNumber++;
            winningPlayer = addCardsToWinningPlayerHand(winningPlayer, round.PlayerFaceCards);
            round.PlayerEndOfRoundCardCount.Add(round.Players[0].PlayerHand.Count);
            round.PlayerEndOfRoundCardCount.Add(round.Players[1].PlayerHand.Count);
            round.RoundNumber = CumulativeRoundNumber;
            return round;
        }

        private Round playWar(Round round)
        {
            //Test conditions are viable to play War
            round = testIfWePlayWar(round);
            while (round.ItsWar)
            {
                //Store the Player Face Cards - we need to keep them before we overwrite the Round Face Cards with the next 'Get and Compare'
                List<Card> normalRoundPlayerFaceCards = round.PlayerFaceCards;

                //Deal next Face Cards and Compare them
                round = getAndCompareNextCards(round);

                if (round.Player0IsTheWinner)
                {
                    //Player 0 Wins - Add both normal round facecards into Player 0's Hand + War Round Face Cards + winning Player's war stack + losing Player's war stack

                    round.Players[0] = addCardsToWinningPlayerHand(round.Players[0], normalRoundPlayerFaceCards);
                    round.Players[0] = addCardsToWinningPlayerHand(round.Players[0], round.PlayerFaceCards);



                    //return endOfRoundUpdates(round, round.Players[0]);
                }
                else if (round.Player1IsTheWinner)
                {
                    //Player 1 Wins - Add both facecards into Player 1's Hand
                    return endOfRoundUpdates(round, round.Players[1]);
                }
                else if (round.ItsADraw)
                {

                    //Test conditions are viable to play War
                    round = testIfWePlayWar(round);

            }

            return round;
        }

        private Round testIfWePlayWar(Round round)
        {
            //See if we can play War - so long as each player each at least 4 Cards and we've played less than 20 rounds of War already
            //If we've played 20 already then the winner is the one with the most Cards
            if (!round.Players[0].SufficientCardsToPlayWar)
            {
                round.ItsWar = false;
                round.IndexOfLoserUnableToPlayWar = 0;
                round.PlayerHasInsufficnetCardsToPlayWar = true;
                return round;
            }
            else if (!round.Players[1].SufficientCardsToPlayWar)
            {
                round.ItsWar = false;
                round.IndexOfLoserUnableToPlayWar = 1;
                round.PlayerHasInsufficnetCardsToPlayWar = true;
                return round;
            }
            else if (Round.NumberOfWarRounds > 19)
            {
                round.ItsWar = false;
                return round;
            }
            else
            {
                Round.NumberOfWarRounds++;
                round.ItsWar = true;
                return round;
            }
        }
    }
}