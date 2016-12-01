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

            resultLabel.Text = "";
            DeckOfCards deckOfCards = new DeckOfCards();
            deckOfCards = deckOfCards.PrepareDeckForGame();

            deckOfCards = dealARound(deckOfCards);

            if (deckOfCards.Player2Hand.Count == 0)
            {
                resultLabel.Text += $"Player 1 wins with a card count of {deckOfCards.Player1Hand.Count} : Player 2 had a card count of {deckOfCards.Player2Hand.Count}.";
            }
            else if (deckOfCards.Player1Hand.Count == 0)
            {
                resultLabel.Text += $"Player 2 wins with a card count of {deckOfCards.Player2Hand.Count} : Player 1 had a card count of {deckOfCards.Player1Hand.Count}.";
            }
            else
            {
                resultLabel.Text += $"Wars > 20 <br></br>";
                resultLabel.Text += $"Player 1 had card count of {deckOfCards.Player1Hand.Count} : Player 2 had a card count of {deckOfCards.Player2Hand.Count}.";
            }
        }
        private DeckOfCards dealARound(DeckOfCards deckOfCards)
        {
            int numberOfWarRounds = 0;
            int numberNormalRounds = 0;
            Card player1NormalRoundFaceCard;
            Card player2NormalRoundFaceCard;


            while ((deckOfCards.Player1Hand.Count > 0 && deckOfCards.Player2Hand.Count > 0) || numberOfWarRounds < 21)
            {
                player1NormalRoundFaceCard = deckOfCards.Player1Hand[0];
                player2NormalRoundFaceCard = deckOfCards.Player2Hand[0];
                numberNormalRounds++;

                if (player1NormalRoundFaceCard.CardValue > player2NormalRoundFaceCard.CardValue)
                {
                    //Player 1 Wins - 
                    //Remove and re-add Player1's facecard; then add Player2's facecard to Player1's hand
                    //Remove Player2's facecard from their hand
                    deckOfCards.Player1Hand.RemoveAt(0);
                    deckOfCards.Player2Hand.RemoveAt(0);
                    deckOfCards.Player1Hand.Add(player1NormalRoundFaceCard);
                    deckOfCards.Player1Hand.Add(player2NormalRoundFaceCard);
                    resultLabel.Text += $" Normal Round: {numberNormalRounds}: Player 1 Wins: {player1NormalRoundFaceCard.CardName} beats {player2NormalRoundFaceCard.CardName} " +
                        $"Player 1 # cards {deckOfCards.Player1Hand.Count} : Player 2 # cards {deckOfCards.Player2Hand.Count}<br> </br> ";
                }
                else if (player2NormalRoundFaceCard.CardValue > player1NormalRoundFaceCard.CardValue)
                {
                    //Player 2 Wins - remove and re-add Player2's facecard; then add Player1's facecard to Player2's hand
                    deckOfCards.Player1Hand.RemoveAt(0);
                    deckOfCards.Player2Hand.RemoveAt(0);
                    deckOfCards.Player2Hand.Add(player2NormalRoundFaceCard);
                    deckOfCards.Player2Hand.Add(player1NormalRoundFaceCard);
                    resultLabel.Text += $" Normal Round: {numberNormalRounds}: Player 2 Wins: {player2NormalRoundFaceCard.CardName} beats {player1NormalRoundFaceCard.CardName} " +
                        $"Player 1 # cards {deckOfCards.Player1Hand.Count} : Player 2 # cards {deckOfCards.Player2Hand.Count}<br> </br> ";
                }
                else
                {
                    //Remove the two Normal Round face cards from each Player's hand
                    deckOfCards.Player1Hand.RemoveAt(0);
                    deckOfCards.Player2Hand.RemoveAt(0);

                    //Only consider Entering into War if: (1) Each player has sufficient cards (>3) (2) # Of War Rounds < 21

                    bool player1HasEnoughWarCards = deckOfCards.Player1Hand.Count > 3 ? true : false;
                    bool player2HasEnoughWarCards = deckOfCards.Player2Hand.Count > 3 ? true : false;

                    if (numberOfWarRounds < 21 && player1HasEnoughWarCards && player2HasEnoughWarCards)
                    {
                        //Set up War Card variables
                        //Set up the Player's List to hold their War Cards and the War Face Cards
                        List<Card> player1WarCards = new List<Card>();
                        List<Card> player2WarCards = new List<Card>();
                        Card player1WarFaceCard;
                        Card player2WarFaceCard;
                        bool warWon = false;

                        while (numberOfWarRounds < 21 && player1HasEnoughWarCards && player2HasEnoughWarCards && !warWon)
                        {
                            numberOfWarRounds++;
                            resultLabel.Text += $"**************** War (Round {numberOfWarRounds}) ****************<br></br>" +
                                $"Player 1 Card {player1NormalRoundFaceCard.CardName} of {player1NormalRoundFaceCard.Suit} == " +
                                $"Player 2 Card {player2NormalRoundFaceCard.CardName} of {player2NormalRoundFaceCard.Suit} == <br></br>";

                            //Set up War face cards and remove them from the Player's hands
                            player1WarFaceCard = deckOfCards.Player1Hand[0];
                            deckOfCards.Player1Hand.RemoveAt(0);
                            player2WarFaceCard = deckOfCards.Player2Hand[0];
                            deckOfCards.Player2Hand.RemoveAt(0);

                            //Add the War Face cards to the Player's War Card Stack - as the winner will get the 3 opponent's face down AND the their face card
                            player1WarCards.Add(player1WarFaceCard);
                            player2WarCards.Add(player2WarFaceCard);

                            //Set up War cards sets of 3 for each round of War

                            for (int i = 0; i < 3; i++)
                            {
                                player1WarCards.Add(deckOfCards.Player1Hand[0]);
                                deckOfCards.Player1Hand.RemoveAt(0);
                                player2WarCards.Add(deckOfCards.Player2Hand[0]);
                                deckOfCards.Player2Hand.RemoveAt(0);
                            }

                            //resultLabel.Text = $"{player1WarCards[0].CardName} {player1WarCards[0].CardValue}";

                            if (player1WarFaceCard.CardValue > player2WarFaceCard.CardValue)
                            {
                                //Player 1 Wins the War
                                for (int i = 0; i < player1WarCards.Count; i++)
                                {
                                    //Add back in all of Player 1's cards
                                    deckOfCards.Player1Hand.Add(player1WarCards[i]);
                                    //Add in all of Player 2's cards
                                    deckOfCards.Player1Hand.Add(player2WarCards[i]);
                                }
                                //Add the last Normal Round Face Cards (where they matched) to the winning players Hand
                                //Set win Flag
                                resultLabel.Text += $"Player 1 Wins - {player1WarCards.Count} + {player1WarCards.Count} + 2 cards.<br> </br>";
                                deckOfCards.Player1Hand.Add(player1NormalRoundFaceCard);
                                deckOfCards.Player1Hand.Add(player2NormalRoundFaceCard);
                                resultLabel.Text += $"Player 1 # cards {deckOfCards.Player1Hand.Count} : Player 2 # cards {deckOfCards.Player2Hand.Count}<br> </br> ";
                                player1HasEnoughWarCards = deckOfCards.Player1Hand.Count > 3 ? true : false;
                                player2HasEnoughWarCards = deckOfCards.Player2Hand.Count > 3 ? true : false;
                                warWon = true;
                            }
                            else if (player2WarFaceCard.CardValue > player1WarFaceCard.CardValue)
                            {
                                //Player 2 Wins the War
                                for (int i = 0; i < player2WarCards.Count; i++)
                                {
                                    //Add back in all of Player 2's cards
                                    deckOfCards.Player2Hand.Add(player2WarCards[i]);
                                    //Add in all of Player 1's cards
                                    deckOfCards.Player2Hand.Add(player1WarCards[i]);
                                }
                                //Add the last Normal Round Face Cards (where they matched) to the winning players Hand
                                //Set win FlagdeckOfCards.Player2Hand.Add(player2NormalRoundFaceCard);
                                resultLabel.Text += $"Player 2 Wins - {player1WarCards.Count} + {player1WarCards.Count} + 2 cards.<br> </br>";
                                deckOfCards.Player2Hand.Add(player1NormalRoundFaceCard);
                                deckOfCards.Player2Hand.Add(player2NormalRoundFaceCard);
                                resultLabel.Text += $"Player 1 # cards {deckOfCards.Player1Hand.Count} : Player 2 # cards {deckOfCards.Player2Hand.Count}<br> </br> ";
                                player1HasEnoughWarCards = deckOfCards.Player1Hand.Count > 3 ? true : false;
                                player2HasEnoughWarCards = deckOfCards.Player2Hand.Count > 3 ? true : false;
                                warWon = true;
                            }
                        }
                    }
                    else
                    {
                        player1HasEnoughWarCards = deckOfCards.Player1Hand.Count > 3 ? true : false;
                        player2HasEnoughWarCards = deckOfCards.Player2Hand.Count > 3 ? true : false;
                        if (!player2HasEnoughWarCards)
                        {
                            //Player 1 Wins the War
                            for (int i = 0; i < player1WarCards.Count; i++)
                            {
                                //Add back in all of Player 1's cards
                                deckOfCards.Player1Hand.Add(player1WarCards[i]);
                                //Add in all of Player 2's cards
                                deckOfCards.Player1Hand.Add(player2WarCards[i]);
                            }
                            deckOfCards.Player1Hand.Add(player1NormalRoundFaceCard);
                            deckOfCards.Player1Hand.Add(player2NormalRoundFaceCard);
                            return deckOfCards;
                        }
                        else if (!player1HasEnoughWarCards)
                        {
                            for (int i = 0; i < player2WarCards.Count; i++)
                            {
                                //Add back in all of Player 2's cards
                                deckOfCards.Player2Hand.Add(player2WarCards[i]);
                                //Add in all of Player 1's cards
                                deckOfCards.Player2Hand.Add(player1WarCards[i]);
                            }
                            deckOfCards.Player1Hand.Add(player1NormalRoundFaceCard);
                            deckOfCards.Player1Hand.Add(player2NormalRoundFaceCard);
                            return deckOfCards;
                        }
                    }
                 }
                }
            }
            //If either player dropped out because they have less then 4 cards they lose
            //Add this in
            return deckOfCards;
        }
    }
}