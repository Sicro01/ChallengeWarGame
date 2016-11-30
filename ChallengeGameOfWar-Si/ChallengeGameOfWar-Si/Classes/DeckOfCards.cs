using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System;


namespace ChallengeGameOfWar_Si.Classes
{
    public class DeckOfCards
    {
        public List<Card> OrderedDeckOfCards { get; private set; }
        public List<Card> ShuffledDeckOfCards { get; set; }
        public List<Card> Player1Hand { get; private set; }
        public List<Card> Player2Hand { get; private set; }

        public static Random random = new Random();
        public DeckOfCards()
        {
            OrderedDeckOfCards = new List<Card>();
            ShuffledDeckOfCards = new List<Card>();
            Player1Hand = new List<Card>();
            Player2Hand = new List<Card>();
        }
       

        public DeckOfCards PrepareDeckForGame()
        {
            List<string> suits = new List<string> { "Diamonds", "Hearts", "Clubs", "Spades" };
            string[] cardFaces = {"Two", "Three", "Four", "Five", "Six",
                "Seven", "Eight", "Nine", "Ten", "Jack", "Queen", "King", "Ace" };
            int[] cardFaceValues = { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 };

            DeckOfCards deckOfCards = new DeckOfCards();
            deckOfCards = createOrderedPackOfCards(suits, cardFaces,cardFaceValues);
            deckOfCards = shuffleTheDeck(deckOfCards);
            deckOfCards = dealPlayerHands(deckOfCards);
            return deckOfCards;
        }

        private DeckOfCards createOrderedPackOfCards(List<string> suits, string[] cardFaces, int[] cardFaceValues)
        {
            DeckOfCards deckOfCards = new DeckOfCards();
           
            foreach (var suit in suits)
            {
                for (int i = 0; i < 13; i++)
                {
                    deckOfCards.OrderedDeckOfCards.Add
                        (new Card { Suit = suit, CardName = cardFaces[i], CardValue = cardFaceValues[i] });
                }
            };
            return deckOfCards;
        }
        private DeckOfCards shuffleTheDeck(DeckOfCards deckOfCards)
        {
            deckOfCards.ShuffledDeckOfCards = deckOfCards.OrderedDeckOfCards.OrderBy(p => random.Next()).ToList();
            return deckOfCards;
        }
        //public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source, Random rng)
        //{
        //    T[] elements = source.ToArray();
        //    // Note i > 0 to avoid final pointless iteration
        //    for (int i = elements.Length - 1; i > 0; i--)
        //    {
        //        // Swap element "i" with a random earlier element it (or itself)
        //        int swapIndex = rng.Next(i + 1);
        //        yield return elements[swapIndex];
        //        elements[swapIndex] = elements[i];
        //        // we don't actually perform the swap, we can forget about the
        //        // swapped element because we already returned it.
        //    }

        //    // there is one item remaining that was not returned - we return it now
        //    yield return elements[0];
        //}

        private DeckOfCards dealPlayerHands(DeckOfCards deckOfCards)
        {
            int cardIndex = 0;
            for (int i = 1; i < 27; i++)
            {
                deckOfCards.Player1Hand.Add(deckOfCards.ShuffledDeckOfCards[cardIndex]);
                cardIndex++;
                deckOfCards.Player2Hand.Add(deckOfCards.ShuffledDeckOfCards[cardIndex]);
                cardIndex++;
            }
            return deckOfCards;
        }

        public string printCardDeck()
        {
            List<string> suits = new List<string> { "Diamond", "Hearts", "Clubs", "Spades" };
            string[] cardFaces = {"Two", "Three", "Four", "Five", "Six",
                "Seven", "Eight", "Nine", "Ten", "Jack", "Queen", "King", "Ace" };
            int[] cardFaceValues = { 2, 3, 4, 5, 6, 7, 8, 9, 10, 10, 10, 10, 11 };
            string s = "";
            foreach (var suit in suits)
            {
                for (int i = 0; i < 13; i++)
                {
                    s += $"Card: {cardFaces[i]} of {suit} which has a value of {cardFaceValues[i]}.<br> </br>";
                }
            }
            return s;
        }
    }
}