using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect4
{
    enum CircleType { NONE, RED, YELLOW };
    enum PlayerType { HUMAN, RANDOM, MINIMAX, ABMINIMAX };
    class GameController
    {
        private const int HEIGHT = 6;
        private const int WIDTH = 7;
        private CircleType[][] state;
        private List<Circle>[] circles; // krugovi sa dna ka vrhu
        private int playerOnMove;
        private Player player1, player2;
        private bool gameEnded = false;
        private int playerWon;
        private long lastTime = 0, currentTime, mDur;
        public GameController(int firstMove, PlayerType p1, PlayerType p2)
        {
            state = new CircleType[HEIGHT][];
            for (int i = 0; i < HEIGHT; i++)
            {
                state[i] = new CircleType[WIDTH];
            }
            for(int i = 0; i < HEIGHT; i++)
            {
                for(int j = 0; j < WIDTH; j++)
                {
                    state[i][j] = CircleType.NONE;
                }
            }
            playerOnMove = firstMove;
            player1 = new Player(p1, CircleType.RED);
            player2 = new Player(p2, CircleType.YELLOW);
            circles = new List<Circle>[WIDTH];
            for(int i=0; i<WIDTH; i++)
            {
                circles[i] = new List<Circle>();
            }
            TimeSpan t = (DateTime.UtcNow - new DateTime(1970, 1, 1));
            lastTime = (long)t.TotalSeconds;

        }

        public void drawCircles(int column)
        {
            int a = 0;
            for (int i=0;i<circles[column].Count;i++)
            {
                circles[column][i].draw();
            }
        }

        public void nextMove(System.Windows.Forms.Panel [] panels)
        {
            
            if (isThereHuman())
                return;
            
            int p;
            if(playerOnMove == 1)
            {
                p = player1.nextMove(state, circles);

            }
            else
            {
                p = player2.nextMove(state, circles);
            }
            if (!isMoveRegular(p))
                return;
            addCircle(p, panels);
            if(playerOnMove == 1)
            {
                playerOnMove = 2;
                player1.IncreaseMoves();
            }
            else
            {
                playerOnMove = 1;
                player2.IncreaseMoves();
            }
            TimeSpan t = (DateTime.UtcNow - new DateTime(1970, 1, 1));
            currentTime = (long)t.TotalSeconds;
            mDur = currentTime - lastTime;
            lastTime = currentTime;

        }

        public void setNextMove(int column, System.Windows.Forms.Panel[] panels)
        {
            //check if the move is regular;
            if (!isMoveRegular(column))
                return;
            addCircle(column, panels);
            if(playerOnMove == 1)
            {
                playerOnMove = 2;
                player1.IncreaseMoves();
            }
            else
            {
                playerOnMove = 1;
                player2.IncreaseMoves();
            }
            TimeSpan t = (DateTime.UtcNow - new DateTime(1970, 1, 1));
            currentTime = (long)t.TotalSeconds;
            mDur = currentTime - lastTime;
            lastTime = currentTime;
            //playerOnMove = (playerOnMove == 1) ? 2 : 1;
        }

        private void addCircle(int column, System.Windows.Forms.Panel[] panels)
        {
            state[HEIGHT - 1 - circles[column].Count][column] = ((playerOnMove == 1) ? CircleType.RED : CircleType.YELLOW); //out of range exception // this will not happen
            circles[column].Add(new Circle((playerOnMove == 1) ? CircleType.RED : CircleType.YELLOW, 0, 565 - (circles[column].Count + 1) * 90, 90, 90, panels[column]));
        }

        public bool isThereHuman()
        {
            return ( (player1.getPlayerType() == PlayerType.HUMAN && playerOnMove == 1) || (playerOnMove == 2 && player2.getPlayerType() == PlayerType.HUMAN)) ? true : false;
        }

        public bool areBothHumans()
        {
            if (player1.getPlayerType() == PlayerType.HUMAN && player2.getPlayerType() == PlayerType.HUMAN)
                return true;
            return false;
        }

        public int getPlayerOnMove()
        {
            return playerOnMove;
        }

        public long getMoveDuration()
        {
            return mDur;
        }

        private bool isMoveRegular(int column)
        {
            if (circles[column].Count < 6)
                return true;
            else
                return false;
        }

        public void checkEndOfGame()
        {
            //provjeri uspravno
            int a = 5;
            for(int i=0; i<HEIGHT; i++)
            {
                for(int j=0; j<WIDTH; j++)
                {
                    CircleType ct = state[i][j];
                    if (ct == CircleType.NONE)
                        continue;
                    int cnt = 1;
                    for(int k=i+1, c=0; k<HEIGHT && c < 3; k++, c++)
                    {
                        if(state[k][j] == ct)
                        {
                            cnt++;
                        }
                    }
                    if(cnt == 4)
                    {
                        gameEnded = true;
                        playerWon = ((ct == CircleType.RED) ? 1 : 2);
                        break;
                    }
                }
                if (gameEnded)
                    break;
            }

            //provjeri horizontalno
            if (!gameEnded)
            {
                for (int i = 0; i < HEIGHT; i++)
                {
                    for (int j = 0; j < WIDTH; j++)
                    {
                        CircleType ct = state[i][j];
                        if (ct == CircleType.NONE)
                            continue;
                        int cnt = 1;
                        for (int k = j + 1, c = 0; k < WIDTH && c < 3; k++, c++)
                        {
                            if (state[i][k] == ct)
                            {
                                cnt++;
                            }
                        }
                        if (cnt == 4)
                        {
                            gameEnded = true;
                            playerWon = ((ct == CircleType.RED) ? 1 : 2);
                            break;
                        }
                    }
                    if (gameEnded)
                        break;
                }
            }
            //ukoso je najteze
            if (!gameEnded)
            {
                for (int i = 0; i < HEIGHT; i++)
                {
                    for (int j = 0; j < WIDTH; j++)
                    {
                        //levo dole
                        CircleType ct = state[i][j];
                        if (ct == CircleType.NONE)
                            continue;
                        int cnt = 1;
                        for (int k = i + 1, l = j - 1, c = 0; k < HEIGHT && l >= 0 && c < 3; k++, l--, c++)
                        {
                            if (state[k][l] == ct)
                                cnt++;
                        }
                        if (cnt == 4)
                        {
                            gameEnded = true;
                            playerWon = ((ct == CircleType.RED) ? 1 : 2);
                            break;
                        }

                        cnt = 1;
                        for(int k = i+1, l = j+1, c = 0; k < HEIGHT && l < WIDTH && c < 3; k++, l++, c++)
                        {
                            if (state[k][l] == ct)
                                cnt++;
                        }
                        if(cnt == 4)
                        {
                            gameEnded = true;
                            playerWon = ((ct == CircleType.RED) ? 1 : 2);
                            break;
                        }
                    }
                    if (gameEnded)
                        break;
                }
            }
            //provjeri jos da nije tabla ispunjena;
            if (!gameEnded)
            {
                bool filled = true;
                for (int i = 0; i < HEIGHT; i++)
                {
                    for (int j = 0; j < WIDTH; j++)
                    {
                        if (state[i][j] == CircleType.NONE)
                        {
                            filled = false;
                            break;
                        }
                    }
                }
                if (filled == true)
                {
                    gameEnded = true;
                    playerWon = 0;
                }
            }
        }

        public void Reset()
        {
            for(int i= 0; i <6; i++)
            {
                circles[i] = new List<Circle>();
                drawCircles(i);
            }

        }
        public bool isGameEnded()
        {
            return gameEnded;
        }

        public int whichPlayerWon()
        {
            return playerWon;
        }

        public Player getPlayer(int id)
        {
            return ((id == 1) ? player1 : player2);
        } 


    }
}
