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
        

        public Game playARound(Game game)
        {
            Round round = new Round();

            return round;
        }

    }
}