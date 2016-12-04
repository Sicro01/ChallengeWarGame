using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChallengeGameOfWar_Si.Classes;

namespace ChallengeGameOfWar_Si.Classes
{
    public class Round
    {
        public string TypeOfRound { get; set; }
        public int RoundNumber { get; set; }
        public static int CumulativeRoundNumber { get; set; } = 0;
        public static int NumberOfWarRounds { get; set; }
        public int WarRoundNumber { get; set; } = 0;
        public List<Player> Players { get; set; }
        public List<Card> PlayerFaceCards { get; set; }
        public List<int> PlayerEndOfRoundCardCount { get; set; }
        public bool Player0IsTheWinner { get; set; } = false;
        public bool Player1IsTheWinner { get; set; } = false;
        public bool ItsADraw { get; set; } = false;
        public bool ItsWar { get; set; } = false;
        public int IndexOfLoserUnableToPlayWar { get; set; }
        public bool PlayerHasInsufficnetCardsToPlayWar { get; set; } = false;
        public List<Card> WarWinnersCards { get; set; }
        public bool ReachedMaxNumberOfWarRounds { get; set; } = false;
        public List<Card> PlayerFaceCardsTriggeringWar { get; set; }

        public Round()
        {
            Players = new List<Player>();
            PlayerFaceCards = new List<Card>();
            PlayerEndOfRoundCardCount = new List<int>();
            WarWinnersCards = new List<Card>();
            PlayerFaceCardsTriggeringWar = new List<Card>();
        }


    public Round playARound(Round round)
        {
            //Deal next Face Cards and Compare them
            round = getAndCompareNextCards(round);

            //Winner/Loser OR Possibly War
            if (round.Player0IsTheWinner)
            {
                //Player 0 Wins - Add both facecards into Player 0's Hand
                return endOfRoundUpdates(round, "Normal Round", round.Players[0], round.PlayerFaceCards);
            }
            else if (round.Player1IsTheWinner)
            {
                //Player 1 Wins - Add both facecards into Player 1's Hand
                return endOfRoundUpdates(round, "Normal Round", round.Players[1], round.PlayerFaceCards);
            }
            else if (round.ItsADraw)
            {
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
            round.ItsADraw = (round.PlayerFaceCards[0].Rank == round.PlayerFaceCards[1].Rank) ? true : false;

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
        private Player addCardsToWinningPlayerHand (Player winningPlayer, List<Card> cardsToBeAddedToWinningPlayersHand)
        {
            foreach (Card card in cardsToBeAddedToWinningPlayersHand)
            {
                winningPlayer.PlayerHand.Add(card);
            }
            //Check if they can still play War
            winningPlayer.SufficientCardsToPlayWar = (winningPlayer.PlayerHand.Count > 3) ? true : false;
            return winningPlayer;
        }
        private Round endOfRoundUpdates(Round round, string typeOfRound, Player winningPlayer, List<Card> cardsToBeAddedToWinningPlayersHand)
        {
            //Increase overall number of Rounds in this Game
            CumulativeRoundNumber++;
            round.TypeOfRound = typeOfRound;
            winningPlayer = addCardsToWinningPlayerHand(winningPlayer, cardsToBeAddedToWinningPlayersHand);
            round.PlayerEndOfRoundCardCount.Add(round.Players[0].PlayerHand.Count);
            round.PlayerEndOfRoundCardCount.Add(round.Players[1].PlayerHand.Count);
            round.RoundNumber = CumulativeRoundNumber;
            return round;
        }

        private Round playWar(Round round)
        {
            //Test conditions are viable to play War
            round = testIfWePlayWar(round);

            //Save the Face Cards which triggered the War
            PlayerFaceCardsTriggeringWar.AddRange(round.PlayerFaceCards);

            while (round.ItsWar)
            {    
                //The PlayerFaceCards List will already contain the PlayerFaceCards from the previous Round where they matched, triggering the War - we add the two triple card sets to this list
                //Get the War triple cards set for each Player - they get stores in the List PlayerFaceCards - then add them to the WarWinnersCards List - ready for the Winner
                round = round.dealACard(round, round.Players[0], 3);
                round = round.dealACard(round, round.Players[1], 3);
                WarWinnersCards.AddRange(round.PlayerFaceCards);
                //Wipe out all cards in PlayerFaceCards - ready to get the next one to compare for the War Round
                round.PlayerFaceCards.Clear();

                //Deal next Face Cards and Compare them
                round = getAndCompareNextCards(round);
                WarWinnersCards.AddRange(round.PlayerFaceCards);

                if (round.Player0IsTheWinner)
                {
                    //Player 0 Wins 
                    return endOfRoundUpdates(round, "War Round", round.Players[0], WarWinnersCards);
                }
                else if (round.Player1IsTheWinner)
                {
                    //Player 1 Wins
                    return endOfRoundUpdates(round, "War Round", round.Players[1], WarWinnersCards);
                }
                else if (round.ItsADraw)
                {
                    //Test conditions are viable to play War
                    round = testIfWePlayWar(round);
                    round.PlayerFaceCards.Clear();
                    //round.ItsADraw = false;
                }
                else
                {
                    return endOfRoundUpdates(round, "Help", round.Players[1], WarWinnersCards);
                }
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
                round.ReachedMaxNumberOfWarRounds = true;
                return round;
            }
            else
            {
                Round.NumberOfWarRounds++;
                round.WarRoundNumber = Round.NumberOfWarRounds;
                round.ItsWar = true;
                return round;
            }
        }
    }
}