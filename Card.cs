using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack
{   
    
    class Card
    {
        public CardSuit cardSuit;
        public CardType cardType;
        public int CardValue;
        public Image CardImage;
    }

    enum CardSuit
    {
        Club,
        Diamond,
        Heart,
        Spade
    }

    enum CardType
    {
        Ace,
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Ten,
        Jack,
        Queen,
        King
    }
}


