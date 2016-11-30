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
            playTheGame();
        }

        private void playTheGame()
        {

            //resultLabel.Text;
            //1 - Deal cards randomly (4 suits, 13 cards each - remove each card as it's dealt)
            //2 - Turn over card and compare
            //3 - If not equal then the winner takes two cards compared
            //4 - If equal than Play War until there is a winner - increase War Count++
            //5 - Continue playing
            //6 - if all cards won or # War rounds >= 20 then end and declare winner

            //Class - Dealer - Pack of Cards

            //Class - Player - Name, Set of Cards
            //Class - NormalRound, 2 cards
            //Class - WarRound - 8 cards

            DeckOfCards deckOfCards = new DeckOfCards();
            deckOfCards = deckOfCards.PrepareDeckForGame();

            deckOfCards = dealARound(deckOfCards);

            //foreach (var card in deckOfCards.Player1Hand)
            //{
            //    resultLabel.Text += $"1@ {card.CardValue} - {card.CardName} <br> </br> ";
            //}

           
            //foreach (var card in deckOfCards.Player2Hand)
            //{
            //    resultLabel.Text += $"2@ {card.CardValue} - {card.CardName} <br> </br> ";
            //}
        }
        private DeckOfCards dealARound(DeckOfCards deckOfCards)
        {
            int index = 0;
            index++;
            NormalRound normalRound = new NormalRound();
            yield return deckOfCards.Player1Hand[index];
        }

    }
}