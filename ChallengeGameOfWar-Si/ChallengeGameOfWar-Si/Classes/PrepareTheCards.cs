using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChallengeGameOfWar_Si.Classes;


namespace ChallengeGameOfWar_Si.Classes
{
    public class PrepareTheCards
    {
        public List<Card> OrderedDeckOfCards { get; private set; }
        public List<Card> ShuffledDeckOfCards { get; set; }

        public static Random random = new Random();
        public PrepareTheCards()
        {
            OrderedDeckOfCards = new List<Card>();
            ShuffledDeckOfCards = new List<Card>();
        }

        public List<Player> PrepareDeckForGame(string player1Name, string player2Name)
        {
            List<string> suits = new List<string> { "♥", "♦", "♠", "♣" };
            string[] cardName = {"Two", "Three", "Four", "Five", "Six",
                "Seven", "Eight", "Nine", "Ten", "Jack", "Queen", "King", "Ace" };
            int[] cardRank = { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 };

            PrepareTheCards prepareTheCards = new PrepareTheCards();
            prepareTheCards = createOrderedPackOfCards(suits, cardName, cardRank);
            prepareTheCards = shuffleTheDeck(prepareTheCards);
            List<Player> players = dealPlayerHands(prepareTheCards, player1Name, player2Name);

            return players;
        }

        private PrepareTheCards createOrderedPackOfCards(List<string> suits, string[] cardName, int[] cardRank)
        {
            //Create a deck of cards
            PrepareTheCards prepareTheCards = new PrepareTheCards();
           
            foreach (var suit in suits)
            {
                for (int i = 0; i < 13; i++)
                {
                    prepareTheCards.OrderedDeckOfCards.Add
                        (new Card { Suit = suit, Name = cardName[i], Rank = cardRank[i] });
                }
            };
            return prepareTheCards;
        }

        private PrepareTheCards shuffleTheDeck(PrepareTheCards prepareTheCards)
        {
            //Shuffle the cards (better routines exist for this)
            prepareTheCards.ShuffledDeckOfCards = prepareTheCards.OrderedDeckOfCards.OrderBy(p => random.Next()).ToList();
            return prepareTheCards;
        }

        private List<Player> dealPlayerHands(PrepareTheCards prepareTheCards, string player1Name, string player2Name)
        {
            //Create and return 2 players and their hands
            List<Player> players = new List<Player>()
            {
                new Player { PlayerName=player1Name },
                new Player { PlayerName=player2Name }
            };
            
            for (int i = 0; i < 26; i++)
            {
                players[0].PlayerHand.Add(prepareTheCards.ShuffledDeckOfCards[i]);
                players[1].PlayerHand.Add(prepareTheCards.ShuffledDeckOfCards[i+26]);
            }
            return players;
        }
    }
}