using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BlackJack
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }               

        CardDeck cd = new CardDeck();
        Player p = new Player();
        Dealer d = new Dealer();

        // Toggle for switch statement. 1 = Player is playing. 2 = Dealer is playing.
        int playerPlaying = 1;

        bool doubleDown = false;

        bool initialDeal = true;

        bool splitHand = false;
        bool playingSplitHand = false;

        private void buttonDeal_Click(object sender, EventArgs e)
        {
            // Set up to begin game play.
            clearAllPlayerHands();
            ClearWinLossImages();
            buttonDeal.Enabled = false;
            enablePlayersButtons();
            panelPlayerHand.BackColor = Color.Transparent;
            panelSplitHand.BackColor = Color.Transparent;


            // Deal 2 cards each for the player and dealer.
            for (int i = 1; i < 3; i++)
            {
                p.PlayerMainHand.Add(d.DealCard(cd.ShoeDeck));
                d.DealerHand.Add(d.DealCard(cd.ShoeDeck));
            }

            // Move control to GamePlayLogic to start playing the game.
            GamePlayLogic();
            
        }

        private void buttonHit_Click(object sender, EventArgs e)
        {
            if (playingSplitHand == false)
            {
                p.PlayerMainHand.Add(d.DealCard(cd.ShoeDeck));
            }
            else
            {
                p.PlayerSplitHand.Add(d.DealCard(cd.ShoeDeck));
            }
            buttonSplit.Enabled = false;
            GamePlayLogic();
        }

        private void buttonStay_Click(object sender, EventArgs e)
        {
            if (splitHand == false || playingSplitHand == true)
            {
                playerPlaying = 2;
            }
            else if (splitHand == true)
            {
                panelSplitHand.BackColor = Color.Lime;
                panelPlayerHand.BackColor = Color.Transparent;
                playingSplitHand = true;
            }
            buttonSplit.Enabled = false;
            GamePlayLogic();

        }

        private void buttonDoubleDown_Click(object sender, EventArgs e)
        {
            p.PlayerMainHand.Add(d.DealCard(cd.ShoeDeck));
            doubleDown = true;
            GamePlayLogic();
        }

        private void buttonSplit_Click(object sender, EventArgs e)
        {
            buttonSplit.Enabled = false;
            buttonDoubleDown.Enabled = false;
            splitHand = true;
            initialDeal = false;
            panelPlayerHand.BackColor = Color.Lime;

            // Split the hand and deal each hand a second card.
            p.PlayerSplitHand.Add(p.PlayerMainHand[1]);
            p.PlayerMainHand.RemoveAt(1);
            p.PlayerMainHand.Add(d.DealCard(cd.ShoeDeck));
            p.PlayerSplitHand.Add(d.DealCard(cd.ShoeDeck));

            // Display both the players hands and update the labels for each hand.
            p.DisplayPlayerHand(panelPlayerHand, p.PlayerMainHand, p.playerName);
            if (p.PlayerMainHand[0].CardValue + p.PlayerMainHand[1].CardValue == 21)
            {
                labelPlayerMainHand.Text = "Player has: BlackJack";
                playingSplitHand = true;
                panelPlayerHand.BackColor = Color.Transparent;
                panelSplitHand.BackColor = Color.Lime;
            }
            else
            {
                labelPlayerMainHand.Text = "Player has: " + p.CountHand(p.PlayerMainHand).ToString();
            }

            p.DisplayPlayerHand(panelSplitHand, p.PlayerSplitHand, p.playerName);
            labelPlayerSplitHand.Visible = true;

            if (p.PlayerSplitHand[0].CardValue + p.PlayerSplitHand[1].CardValue == 21)
            {
                labelPlayerSplitHand.Text = "Player has: BlackJack";
                playerPlaying = 2;
                GamePlayLogic();
            }
            else
            {
                labelPlayerSplitHand.Text = "Player has: " + p.CountHand(p.PlayerSplitHand).ToString();
            }

            
        }

        private void GamePlayLogic()
        {
            switch (playerPlaying) 
            {
                // Player is playing.
                case 1:
                    // player can split hand if the first two cards are of the same type and its the initial deal.
                    if (p.PlayerMainHand[0].cardType == p.PlayerMainHand[1].cardType && initialDeal == true)
                    {
                        buttonSplit.Enabled = true;
                    }

                    initialDeal = false;

                    if (playingSplitHand == false)
                    {
                        // Only show one of the dealers cards and the value of that card until its the dealers turn to play.                        
                        d.DisplayPlayerHand(panelDealerHand, d.DealerHand, d.playerName);
                        labelDealersHand.Text = "Dealer shows: " + d.DealerHand[1].CardValue.ToString();

                        // Player has blackjack or player has 21 or player busts. Pass the play to the dealer by setting playerPlaying to 2.
                        if ((p.PlayerMainHand[0].CardValue + p.PlayerMainHand[1].CardValue == 21) || p.CountHand(p.PlayerMainHand) >= 21)
                        {
                            if (splitHand == true)
                            {
                                playingSplitHand = true;
                                panelPlayerHand.BackColor = Color.Transparent;
                                panelSplitHand.BackColor = Color.Lime;
                            }
                            else
                            {
                                playerPlaying = 2;
                            }
                            
                        }

                        //Show the players hand and update the players label to reflect the total of the hand.
                        p.DisplayPlayerHand(panelPlayerHand, p.PlayerMainHand, p.playerName);

                        if (p.PlayerMainHand[0].CardValue + p.PlayerMainHand[1].CardValue == 21)
                        {
                            labelPlayerMainHand.Text = "Player has: BlackJack";
                        }
                        else if (p.CountHand(p.PlayerMainHand) <= 21)
                        {
                            labelPlayerMainHand.Text = "Player has: " + p.CountHand(p.PlayerMainHand).ToString();
                        }
                        else if (p.CountHand(p.PlayerMainHand) > 21)
                        {
                            labelPlayerMainHand.Text = "Player busts with " + p.CountHand(p.PlayerMainHand).ToString();
                        }


                        if (doubleDown == true)
                        {
                            playerPlaying = 2;
                        }

                    }
                    else if (playingSplitHand == true)
                    {
                        
                        // Player has blackjack or player has 21 or player busts. Pass the play to the dealer by setting playerPlaying to 2.
                        if ((p.PlayerSplitHand[0].CardValue + p.PlayerSplitHand[1].CardValue == 21) || p.CountHand(p.PlayerSplitHand) >= 21)
                        {
                           playerPlaying = 2;                          

                        }

                        //Show the players hand and update the players label to reflect the total of the hand.
                        p.DisplayPlayerHand(panelSplitHand, p.PlayerSplitHand, p.playerName);

                        if (p.PlayerSplitHand[0].CardValue + p.PlayerSplitHand[1].CardValue == 21)
                        {
                            labelPlayerSplitHand.Text = "Player has: BlackJack";
                        }
                        else if (p.CountHand(p.PlayerSplitHand) <= 21)
                        {
                            labelPlayerSplitHand.Text = "Player has: " + p.CountHand(p.PlayerSplitHand).ToString();
                        }
                        else if (p.CountHand(p.PlayerSplitHand) > 21)
                        {
                            labelPlayerSplitHand.Text = "Player busts with " + p.CountHand(p.PlayerSplitHand).ToString();
                        }
                    }
                    

                    // Pass the play to the dealer if the player is finished.
                    if (playerPlaying == 2)
                    {                       
                        doubleDown = false;
                        GamePlayLogic();
                    }

                    break;

                // Dealer is playing
                case 2:
                    disablePlayerButtons();
                    panelSplitHand.BackColor = Color.Transparent;

                    // Player does not have blackjack. Dealer plays the hand.
                    if (!(p.PlayerMainHand[0].CardValue + p.PlayerMainHand[1].CardValue == 21))
                    {
                        while (d.CountHand(d.DealerHand) < 17)
                        {
                            d.DealerHand.Add(d.DealCard(cd.ShoeDeck));
                            d.DisplayPlayerHand(panelDealerHand, d.DealerHand, d.playerName, true);                           
                        }
                        
                    }

                    // Show the dealers full hand and update the dealers label to reflect the total of the hand.
                    d.DisplayPlayerHand(panelDealerHand, d.DealerHand, d.playerName, true);

                    if (d.DealerHand[0].CardValue + d.DealerHand[1].CardValue == 21)
                    {
                        labelDealersHand.Text = "Dealer has: BlackJack";
                    }
                    else
                    {
                        labelDealersHand.Text = "Dealer has: " + d.CountHand(d.DealerHand).ToString();
                    }
                    checkWin();
                    buttonDeal.Enabled = true;
                    playerPlaying = 1;
                    
                    break;
                default:
                    break;
            }

            // Shoe deck is getting low. Repopulate queue.
            if (cd.ShoeDeck.Count <= 25)
            {
                cd.ShuffleDeck();
            }
        }  
        
        private void checkWin()
        {
            
            if ((p.PlayerMainHand[0].CardValue + p.PlayerMainHand[1].CardValue == 21 && d.DealerHand[0].CardValue + d.DealerHand[1].CardValue == 21) ||
                    (p.CountHand(p.PlayerMainHand) == d.CountHand(d.DealerHand)) || 
                    (p.CountHand(p.PlayerMainHand) > 21 && d.CountHand(d.DealerHand) > 21))
            {
                // Push
                labelMainHandPush.Text = "Push!!!";
            }
            else if (p.PlayerMainHand[0].CardValue + p.PlayerMainHand[1].CardValue == 21)
            {
                // Player wins with blackjack
                pictureBoxMainHand.Image = Image.FromFile(@"Images\Correct.png");
                pictureBoxDealerHand.Image = Image.FromFile(@"Images\InCorrect.png");
            }
            else if (d.DealerHand[0].CardValue + d.DealerHand[1].CardValue == 21)
            {
                // Dealer wins with blackjack
                pictureBoxMainHand.Image = Image.FromFile(@"Images\InCorrect.png");
                pictureBoxDealerHand.Image = Image.FromFile(@"Images\Correct.png");
            }
            else if ((p.CountHand(p.PlayerMainHand) <= 21 && d.CountHand(d.DealerHand) <= 21) &&
                    (p.CountHand(p.PlayerMainHand) > d.CountHand(d.DealerHand)) || 
                    (p.CountHand(p.PlayerMainHand) <= 21 && d.CountHand(d.DealerHand) > 21))
            {
                // Player wins
                pictureBoxMainHand.Image = Image.FromFile(@"Images\Correct.png");
                pictureBoxDealerHand.Image = Image.FromFile(@"Images\InCorrect.png");
            }
            else if ((p.CountHand(p.PlayerMainHand) <= 21 && d.CountHand(d.DealerHand) <= 21) &&
                    (p.CountHand(p.PlayerMainHand) < d.CountHand(d.DealerHand)) ||
                    (p.CountHand(p.PlayerMainHand) > 21 && d.CountHand(d.DealerHand) <= 21))
            {
                // Dealer wins
                pictureBoxMainHand.Image = Image.FromFile(@"Images\InCorrect.png");
                pictureBoxDealerHand.Image = Image.FromFile(@"Images\Correct.png");
            }

            if (splitHand == true)//*****************************************************************
            {
                if ((p.PlayerSplitHand[0].CardValue + p.PlayerSplitHand[1].CardValue == 21 && d.DealerHand[0].CardValue + d.DealerHand[1].CardValue == 21) ||
                    (p.CountHand(p.PlayerSplitHand) == d.CountHand(d.DealerHand)) || (p.CountHand(p.PlayerSplitHand) > 21 && d.CountHand(d.DealerHand) > 21))
                {
                    // Push
                    labelSplitHandPush.Text = "Push!!!";
                }
                else if (p.PlayerSplitHand[0].CardValue + p.PlayerSplitHand[1].CardValue == 21)
                {
                    // Player wins with blackjack
                    pictureBoxSplitHand.Image = Image.FromFile(@"Images\Correct.png");
                    pictureBoxDealerSplit.Image = Image.FromFile(@"Images\InCorrect.png");
                }
                else if (d.DealerHand[0].CardValue + d.DealerHand[1].CardValue == 21)
                {
                    // Dealer wins with blackjack
                    pictureBoxSplitHand.Image = Image.FromFile(@"Images\InCorrect.png");
                    pictureBoxDealerSplit.Image = Image.FromFile(@"Images\Correct.png");
                }
                else if ((p.CountHand(p.PlayerSplitHand) <= 21 && d.CountHand(d.DealerHand) <= 21) &&
                        (p.CountHand(p.PlayerSplitHand) > d.CountHand(d.DealerHand)) ||
                        (p.CountHand(p.PlayerSplitHand) <= 21 && d.CountHand(d.DealerHand) > 21))
                {
                    // Player wins
                    pictureBoxSplitHand.Image = Image.FromFile(@"Images\Correct.png");
                    pictureBoxDealerSplit.Image = Image.FromFile(@"Images\InCorrect.png");
                }
                else if ((p.CountHand(p.PlayerSplitHand) <= 21 && d.CountHand(d.DealerHand) <= 21) &&
                        (p.CountHand(p.PlayerSplitHand) < d.CountHand(d.DealerHand)) ||
                        (p.CountHand(p.PlayerSplitHand) > 21 && d.CountHand(d.DealerHand) <= 21))
                {
                    // Dealer wins
                    pictureBoxSplitHand.Image = Image.FromFile(@"Images\InCorrect.png");
                    pictureBoxDealerSplit.Image = Image.FromFile(@"Images\Correct.png");
                }
            }
            int x = 0;
        }

        private void ClearWinLossImages()
        {
            pictureBoxMainHand.Image = null;
            pictureBoxSplitHand.Image = null;
            pictureBoxDealerSplit.Image = null; 
            pictureBoxDealerHand.Image = null;
        }
        
        private void enablePlayersButtons()
        {
            buttonHit.Enabled = true;
            buttonStay.Enabled = true;            
            buttonDoubleDown.Enabled = true;
        }

        private void disablePlayerButtons()
        {
            buttonHit.Enabled = false;
            buttonStay.Enabled = false;
            buttonDoubleDown.Enabled = false;
        }

        private void clearAllPlayerHands()
        {
            // Dealer Hand
            panelDealerHand.Controls.Clear();
            d.DealerHand.Clear();
            labelDealersHand.Text = "0";

            // Player Hand
            panelPlayerHand.Controls.Clear();
            p.PlayerMainHand.Clear();
            labelPlayerMainHand.Text = "0";

            // Player Split Hand
            panelSplitHand.Controls.Clear();
            p.PlayerSplitHand.Clear();          
            labelPlayerSplitHand.Text = "0";
            labelPlayerSplitHand.Visible = false;

            // Set the state of game variables back to default.
            playerPlaying = 1;
            doubleDown = false;
            initialDeal = true;
            playingSplitHand = false;
            splitHand = false;
            labelMainHandPush.Text = string.Empty;
            labelSplitHandPush.Text = string.Empty;
            
        }

        private void buttonReadMe_Click(object sender, EventArgs e)
        {

        }
    }
}
