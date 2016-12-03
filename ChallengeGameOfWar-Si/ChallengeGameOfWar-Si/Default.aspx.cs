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
            //Set up Players and their Hands
            PrepareTheCards prepareTheCards = new PrepareTheCards();
            List<Player> players = prepareTheCards.PrepareDeckForGame(player1Name, player2Name);
            resultLabel.Text = "";

            //printTheHands(players);

            //Play the game until either one player has all the cards or they've played 20 War rounds
            Game game = new Game();
            game = game.Play(players);

            printTheGame(game);
        }

        private void printTheGame(Game game)
        {
            

            foreach (Round round in game.Rounds)
            {
                Player winner = (round.Player0IsTheWinner) ? round.Players[0] : round.Players[1];
                Player loser = (round.Player0IsTheWinner) ? round.Players[1] : round.Players[0];
                int winnerListIndex = (round.Player0IsTheWinner) ? 0 : 1;
                int loserListIndex = (round.Player0IsTheWinner) ? 1 : 0;
                string draw = (round.Player0IsTheWinner || round.Player1IsTheWinner) ? "Not a Draw" : "Draw";

                resultLabel.Text +=
                    $"Round Number/Type: {round.RoundNumber} / {round.TypeOfRound}:  {draw}        " +
                    $"Winner: {winner.PlayerName} played  a {round.PlayerFaceCards[winnerListIndex].Name} of {round.PlayerFaceCards[winnerListIndex].Suit} " +
                    $"beating {loser.PlayerName} who played {round.PlayerFaceCards[loserListIndex].Name} of {round.PlayerFaceCards[loserListIndex].Suit}" +
                    "<br> </br>";

                resultLabel.Text +=
                    $"{winner.PlayerName} has {round.PlayerEndOfRoundCardCount[winnerListIndex]} and {loser.PlayerName} has {round.PlayerEndOfRoundCardCount[loserListIndex]} " +
                    $"- totalling {round.PlayerEndOfRoundCardCount[winnerListIndex] + round.PlayerEndOfRoundCardCount[loserListIndex]} cards." +
                    "<br> </br>" +
                    "<br> </br>";
            }
        }
        private void printTheHands(List<Player> players)
        {
            foreach (Player player  in players)
            {
                foreach (Card card in player.PlayerHand)
                {
                    resultLabel.Text += $"Player Name: {player.PlayerName} : Card: {card.Name} of {card.Suit} Rank of {card.Rank} <br></br>";
                }
            }
        }
    }
}