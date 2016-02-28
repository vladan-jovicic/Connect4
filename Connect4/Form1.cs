using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Connect4
{
    public partial class Form1 : Form
    {
        GameController gameController;
        private long lastDateTime;
        Timer GameTimer;
        private DateTime time;
        private bool winnerWrote = false;
        public Form1()
        {
            InitializeComponent();
            //this.Paint += new PaintEventHandler(Form1_Paint);
            
            //GameTimer.Start();
            //gameController = new GameController(1, C);
        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {

            if (gameController == null)
                return;
            for (int i = 0; i < 7; i++)
            {
                gameController.drawCircles(i);
            }
            Invalidate();
            movesP1.Text = "Number of moves of Player 1: " + (gameController.getPlayer(1).GetNumberOfMoves().ToString());
            movesP2.Text = "Number of moves of Player 2: " + (gameController.getPlayer(2).GetNumberOfMoves().ToString());
            label1.Text = "Status: " + ((gameController.getPlayerOnMove() == 1) ? "Player1" : "Player2") + " on move";
            //TimeSpan t = (DateTime.UtcNow - new DateTime(1970, 1, 1));
            //long currentTime = (long)t.TotalSeconds;
            //long currentTime = time.Hour * 3600 + time.Minute * 60 + time.Second;
            totalNumOfMov.Text = "Total num of moves: " + (gameController.getPlayer(1).GetNumberOfMoves() + gameController.getPlayer(2).GetNumberOfMoves()).ToString();
            //long tmp = currentTime - lastDateTime;
            if (gameController.getPlayerOnMove() == 2)
            {
                player1Time.Text = "Player1 time: " + (gameController.getMoveDuration()).ToString();
            }
            else
            {
                player2Time.Text = "Player2 time: " + (gameController.getMoveDuration()).ToString();
            }
            StopGameLoop();
            if (gameController == null)
                return;
            if (gameController.isThereHuman()) //if there is human and is on move
                return;
            if (!gameController.areBothHumans())
            {
                Panel[] pnls = { panel2, panel3, panel4, panel5, panel6, panel7, panel8 };
                gameController.nextMove(pnls);
            }
            
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {
            if (gameController == null)
                return;
            gameController.drawCircles(2);
        }

        private void panel_hover(object sender, EventArgs e)
        {
            if (gameController == null || !gameController.isThereHuman())
                return;
            Panel p = sender as Panel;
            p.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
        }

        private void panel_leave(object sender, EventArgs e)
        {
            if (gameController == null || !gameController.isThereHuman())
                return;
            Panel p = sender as Panel;
            p.BorderStyle = BorderStyle.FixedSingle;
        }

        private void panel_click(object sender, MouseEventArgs e)
        {
            if (gameController == null || !gameController.isThereHuman())
                return;
            Panel p = sender as Panel;
            Panel[] pnls = { panel2, panel3, panel4, panel5, panel6, panel7, panel8 };
            for (int i=0; i<7; i++)
            {
                if (p.Equals(pnls[i]))
                {
                    gameController.setNextMove(i, pnls);
                    break;
                }
            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            if (gameController == null)
                return;
            gameController.drawCircles(0);
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {
            if (gameController == null)
                return;
            gameController.drawCircles(1);
        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {
            if (gameController == null)
                return;
            gameController.drawCircles(3);
        }

        private void panel6_Paint(object sender, PaintEventArgs e)
        {
            if (gameController == null)
                return;
            gameController.drawCircles(4);
        }

        private void panel7_Paint(object sender, PaintEventArgs e)
        {
            if (gameController == null)
                return;
            gameController.drawCircles(5);
        }

        private void panel8_Paint(object sender, PaintEventArgs e)
        {
            if (gameController == null)
                return;
            gameController.drawCircles(6);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            NewGame();

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void NewGame()
        {
            RadioButton[] rbuttons = { radioButton1, radioButton2, radioButton3, radioButton4, radioButton8, radioButton7, radioButton6, radioButton5 };
            PlayerType[] types = { PlayerType.HUMAN, PlayerType.RANDOM, PlayerType.MINIMAX, PlayerType.ABMINIMAX };
            PlayerType p1 = PlayerType.RANDOM, p2 = PlayerType.RANDOM;
            for (int i = 0; i < 4; i++)
            {
                if (rbuttons[i].Checked)
                    p1 = types[i];
                if (rbuttons[i + 4].Checked)
                    p2 = types[i];
            }
            gameController = new GameController(1, p1, p2);
            winnerWrote = false;
            Panel[] pnls = { panel2, panel3, panel4, panel5, panel6, panel7, panel8 };
            for (int i = 0; i < 7; i++)
            {
                pnls[i].Invalidate();
            }
            player1Time.Text = "Player1 time: 0";
            player2Time.Text = "Player2 time: 0";
            time = new DateTime();
            TimeSpan t = (DateTime.UtcNow - new DateTime(1970, 1, 1));
            lastDateTime = (long)t.TotalSeconds;
            GameTimer = new Timer();
            GameTimer.Interval = 10;
            GameTimer.Tick += new EventHandler(this.GameTimer_Tick);
            GameTimer.Start();
        }

        private void StopGameLoop()
        {
            if (gameController == null)
                return;
            gameController.checkEndOfGame();
            if (gameController.isGameEnded())
            {
                if (winnerWrote)
                    return;
                if (gameController.whichPlayerWon() == 0)
                {
                    //MessageBox.Show("The game finished tie!", "Game Over", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    label1.Text = "Status: The game finished tie!";
                    winnerWrote = true;
                }
                else
                {
                    //MessageBox.Show("Player" + gameController.whichPlayerWon().ToString() + " won!", "Game Over", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    label1.Text = "Status: Player" + gameController.whichPlayerWon().ToString() + " won!"; 
                    winnerWrote = true;
                }
                //gameController.Reset();
                gameController = null;
                //GameTimer.Stop();
                GameTimer.Interval = 1000000000;
                GameTimer.Dispose();
                GameTimer.Enabled = false;
                GameTimer = null;
                return;
            }
        }
    }
}
