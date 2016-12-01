using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChallengeGameOfWar_Si.Classes;

namespace ChallengeGameOfWar_Si
{
    
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void okButton_Click(object sender, EventArgs e)
        {
            playTheGame("Simon", "Jill");
        }

        private void playTheGame(string player1Name, string player2Name)
        {

            PrepareTheCards prepareTheCards = new PrepareTheCards();

            List<Player> players = prepareTheCards.PrepareDeckForGame(player1Name, player2Name);
            Game game = new Game();
            game.Players = players;

            game = game.Play(game);

        }
    }
}