using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BlackJack
{
    class Player
    {        
        public int PlayerTotal = 0;
        public string playerName = string.Empty;
       
        public List<Card> DealerHand = new List<Card>();
        public List<Card> PlayerMainHand = new List<Card>();
        public List<Card> PlayerSplitHand = new List<Card>();
                

        /// <summary>
        /// Sums the card value in the players hand.
        /// </summary>
        /// <param name="cards"></param>
        /// <returns>The sum of the card values.</returns>
        public int CountHand(List<Card> cards)
        {
            int total = 0;
            
            foreach (Card card in cards)
            {
                total += card.CardValue;
            }

            foreach (Card card in cards)
            {
                if (total <= 21)
                    break;
                else if (card.CardValue == 11)
                {
                    total -= 10;
                }
            }
            return total;
        }
        

        /// <summary>
        /// Displays each players hand.
        /// </summary>
        /// <param name="panel"></param>
        /// <param name="playerList"></param>
        /// <param name="playerName"></param>
        /// <param name="dealerShowAll"></param>
        public void DisplayPlayerHand(Panel panel, List<Card> playerList, string playerName, bool dealerShowAll = false)
        {
            int hPos = 0;
            int vPos = 0;
            bool firstCard = true;

            //Clear card images
            panel.Controls.Clear();

            foreach (Card card in playerList)
            {
                PictureBox pb = new PictureBox();
                pb.Size = new Size(50, 70);
                pb.Location = new Point(hPos, vPos);
                pb.SizeMode = PictureBoxSizeMode.Zoom;

                if (playerName == "Dealer" && dealerShowAll == false && firstCard == true)
                {
                    pb.Image = Image.FromFile(filename: @"Images\CardBack_Blue.png");
                }
                else
                {
                    pb.Image = Image.FromFile(filename: @"Images\" + Convert.ToString(card.cardSuit) + Convert.ToString((CardType)card.cardType) + ".png");
                }

                panel.Controls.Add(pb);
                hPos += 50;
                firstCard = false;
            }
        }
       

        /// <summary>
        /// Split the player hand if the cards delt match.
        /// </summary>
        /// <param name="panel"></param>
        /// <param name="label"></param>
        /// <param name="splitHand"></param>
        public void Split(Panel panel, Label label, bool splitHand = false)
        {
            //Set splitHand to true.
            splitHand = true;

            //Set the 2nd players card panel to the default backcolor and set its visible property to true.
            //Set the 2nd players total label default forecolor, and set the its visible property to true.
            panel.BackColor = Control.DefaultBackColor;
            label.ForeColor = Color.Black;
            panel.Visible = true;
            label.Visible = true;

            //Split the players hand.
            PlayerSplitHand.Add(PlayerMainHand.ElementAt(1));
            PlayerMainHand.Remove(PlayerMainHand.ElementAt(1));
            
        }
        
    }

    class Dealer : Player
    {
        public Dealer()
        {
            this.playerName = "Dealer";
        }

        /// <summary>
        /// Deals a single card.
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        public Card DealCard(Queue<Card> card)
        {
            return card.Dequeue();
        }

    }

}
