using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChallengeGameOfWar_Si.Classes
{
    public class Player
    {
        public string PlayerName { get; set; }
        public List<Card> PlayerHand { get; set; }
        public List<Card> PlayerWarCards { get; set; }
        public bool SufficientCardsToPlayWar { get; set; } = true;

        public Player()
        {
            PlayerHand = new List<Card>();
        }
    }
}