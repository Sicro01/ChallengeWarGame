using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChallengeGameOfWar_Si.Classes;

namespace ChallengeGameOfWar_Si.Classes
{
    public class Game
    {
        public List<Round> Rounds { get; set; }
        
        public Game()
        {
            Rounds = new List<Round>();
        }
        
        public Game Play(List<Player> players)
        {
            //Set up a new Game and the first Round with the Players passed to this class
            Game game = new Game();
            Round round = new Round();
            Round.CumulativeRoundNumber = 0;
            
            round.Players = players;

            //Play Rounds until either one of the Player's has no more cards OR either one has too few cards to play a War Round OR they've played 20 War Rounds
            while (round.Players[0].PlayerHand.Count > 0 && round.Players[1].PlayerHand.Count > 0 && Round.NumberOfWarRounds < 10)
            {
                //Play a Round and add it to the Game so we can print out the results after the Game has finished
                game.Rounds.Add(round.playARound(round));
                //if (round.RoundNumber > 999)
                //{
                //    break;
                //}
                //Create a new Round - but keep the Players and their Hand's
                Round newRound = new Round() { Players = round.Players };
                round = newRound;
            }

            //Game over
            return game;
        }
        
    }
   
}