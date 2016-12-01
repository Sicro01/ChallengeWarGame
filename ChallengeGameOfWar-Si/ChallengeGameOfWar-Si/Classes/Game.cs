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
        public List<Player> Players { get; set; }
        
        public Game Play(Game game)
        {
            Round round = new Round();
            game = round.playARound(game);

            game.Rounds.Add(round);

            return game;
        }
        
    }
   
}