using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack
{
    class CardDeck
    {   
        /// <summary>
        /// The ShoeSize amount is based on an 8 deck shoe.
        /// </summary>
        private int ShoeSize = 416;

        // Contains a full deck of cards to create the shoe deck from.        
        private List<Card> FullDeck = new List<Card>(52);

        /// <summary>
        /// Queue that contains the cards for game play.
        /// </summary>
        public Queue<Card> ShoeDeck = new Queue<Card>();

        public CardDeck()
        {
            CreateDeck();
            ShuffleDeck();
        }

        /// <summary>
        ///     Creates full deck of 52 cards
        /// </summary>
        public void CreateDeck()
        {
            //The outer loop is used to assign the card suit by casting the loop ints(0-3) to the enum value holding the name of the suit.
            for (int i = 0; i <= 3; i++)
            {
                //The inner loop creates the 13 card of each suit.
                for (int j = 0; j < 13; j++)
                {
                    Card card = new Card();
                    card.cardSuit = (CardSuit)i;
                    card.cardType = (CardType)j;
                    card.CardImage = Image.FromFile(filename: @"Images\" + Convert.ToString((CardSuit)i) + Convert.ToString((CardType)j) + ".png");

                    if (j == 0)
                    {
                        card.CardValue = 11;
                        FullDeck.Add(card);
                    }
                    else if (j >= 10)
                    {
                        card.CardValue = 10;
                        FullDeck.Add(card);
                    }
                    else
                    {
                        card.CardValue = j + 1;
                        FullDeck.Add(card);
                    }
                }
            }

        }

        /// <summary>
        ///     Creates a shuffled 'shoe' of 8 full decks for Black Jack play
        /// </summary>
        public void ShuffleDeck()
        {
            List<Card> temp = new List<Card>();

            // Populates the temp list with 416 cards (8 full decks in order)
            for (int i = 0; i < 8; i++)
            {
                foreach (Card card in FullDeck)
                {
                    temp.Add(card);
                }
            }

            Random random = new Random(DateTime.Now.Millisecond);

            // Used to temporarily hold the randomized 416 cards
            List<Card> temp2 = new List<Card>();

            int counter = temp.Count;

            for (int i = 0; i < ShoeSize; i++)
            {
                int randNum = random.Next(0, counter);
                temp2.Add(temp.ElementAt(randNum));
                temp.Remove(temp.ElementAt(randNum));
                counter--;
            }

            // At this point temp is empty, temp2 holds a randomized deck of 416 cards
            // temp2 gets shuffled 7 times
            for (int i = 0; i < 7; i++)
            {
                int counter2 = temp2.Count;
                for (int j = 0; j < ShoeSize; j++)
                {
                    int randNum2 = random.Next(0, counter2);
                    temp.Add(temp2.ElementAt(randNum2));
                    temp2.Remove(temp2.ElementAt(randNum2));
                    counter2--;
                }

                for (int k = 0; k < ShoeSize; k++)
                {
                    temp2.Add(temp.ElementAt(k));
                }

                temp.Clear();
            }

            foreach (Card card in temp2)
            {
                ShoeDeck.Enqueue(card);
            }

            temp2.Clear();
        }
    }

}
