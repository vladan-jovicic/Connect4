using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect4
{
    class ABminimaxLogic
    {
        private const int HEIGHT = 6;
        private const int WIDTH = 7;
        private const int maxDepth = 6;
        private CircleType myType;
        public ABminimaxLogic(CircleType ct)
        {
            myType = ct;
        }

        private CircleType OpponentType()
        {
            return ((myType == CircleType.RED) ? CircleType.YELLOW : CircleType.RED);
        }
        
        public int nextMove(CircleType [][] currentState, List<Circle>[] circles, int minimax, int alpha, int beta, int currentDepth)
         {
            //check for the utility function
            //probably, something is wrong here
            if(currentDepth > maxDepth)
            {
                int scr = hFunction(myType, currentState)*10;
                int scr1 = hFunction(OpponentType(), currentState)*10;
                if (Math.Abs(scr) == Math.Abs(scr1))
                {
                    if (minimax == 1)
                        return scr;
                    else
                        return scr1;
                }
                else if (Math.Abs(scr) > Math.Abs(scr1))
                    return scr;
                else
                    return scr1;
            }
            int score = UtilityFunction(ref currentState);
            //WARNING It should not happend that score == 100;
            if (score != -100)
                return score;
            if (minimax == 1) //if maximizing player
            {
                int v = -2000;
                int bestMove = -1;
                int[] bm = new int[7];
                for(int i=0; i<WIDTH; i++)
                {
                    if (isMoveRegular(i, circles))
                    {
                        addCircle(i, ref currentState, ref circles, myType);
                        int p = nextMove(currentState, circles, 2, alpha, beta, currentDepth + 1);
                        bm[i] = p;
                        if (p >= v)
                        {
                            v = p;
                            bestMove = i;
                        }
                        alpha = Math.Max(alpha, v);
                        removeCircle(i, ref currentState, ref circles);
                        if (beta < alpha)
                            break;
                    }
                }
                if (currentDepth == 0)
                    return bestMove;
                else
                    return v;
            }
            else //minimizing player
            {
                int v = 2000;
                int bestMove = -1;
                for(int i=0; i <7; i++)
                {
                    if(isMoveRegular(i, circles))
                    {
                        addCircle(i, ref currentState, ref circles, OpponentType());
                        int p = nextMove(currentState, circles, 1, alpha, beta, currentDepth + 1);
                        if(p <= v)
                        {
                            v = p;
                            bestMove = i;
                        }
                        removeCircle(i, ref currentState, ref circles);
                        beta = Math.Min(beta, v);
                        if (beta < alpha)
                            break;
                    }
                }
                if (currentDepth == 0)
                    return bestMove;
                else
                    return v;
            }

        }
        private void removeCircle(int column, ref CircleType[][] currentState, ref List<Circle>[] circles)
        {
            currentState[HEIGHT - 1 - circles[column].Count + 1][column] = CircleType.NONE;
            circles[column].RemoveAt(circles[column].Count - 1);
        }
        private void addCircle(int column, ref CircleType[][] currentState, ref List<Circle>[] circles, CircleType ct)
        {
            currentState[HEIGHT - 1 - circles[column].Count][column] = ct;//out of range exception // this will not happen
            circles[column].Add(new Circle(ct, -100, -100, 1, 1, null));
        }

        private bool isMoveRegular(int column, List<Circle>[] circles)
        {
            if (circles[column].Count < 6)
                return true;
            else
                return false;
        }

        private int UtilityFunction(ref CircleType[][] currentState)
        {
            int p = checkEnd(currentState); // OfGame(currentState);
            if (p == 1)
            {
                if (myType == CircleType.RED)
                    return 1000;
                else
                    return -1000;
            }
            else if (p == 2)
            {
                if (myType == CircleType.YELLOW)
                    return 1000;
                else
                    return -1000;
            }
            else
                return -100;
        }


        public int hFunction(CircleType ct, CircleType[][] currentState)
        {
            int maximum = 0;
            for(int i = 0; i < HEIGHT; i++)
            {
                for(int j=0; j<WIDTH;j ++)
                {
                    if (currentState[i][j] == CircleType.NONE)
                        continue;
                    //desno
                    int cnt = 0;
                    for(int k = j; k < WIDTH; k++)
                    {
                        if (currentState[i][k] == ct)
                            cnt++;
                        else
                            break;
                    }
                    maximum = Math.Max(maximum, cnt);
                    cnt = 0;
                    for(int k = i; k < HEIGHT; k++) // dole
                    {
                        if (currentState[k][j] == ct)
                            cnt++;
                        else
                            break;
                    }
                    maximum = Math.Max(maximum, cnt);
                    cnt = 0;
                    for(int k = i, l = j; k < HEIGHT && l < WIDTH; k++, l++) //koso desno
                    {
                        if (currentState[k][l] == ct)
                            cnt++;
                        else
                            break;
                    }
                    maximum = Math.Max(maximum, cnt);
                    cnt = 0;
                    for(int k = i, l = j; k < HEIGHT && l >= 0; k++, l--)
                    {
                        if (currentState[k][l] == ct)
                            cnt++;
                        else
                            break;
                    }
                    maximum = Math.Max(maximum, cnt);
                }
            }
            return maximum;
        }

        public int checkEnd(CircleType[][] state)
        {
            int scr = hFunction(CircleType.RED, state)*10; // hFunction(myType, state);
            int scr1 = hFunction(CircleType.YELLOW, state)*10; // hFunction(OpponentType(), state);
            if (scr >= 40)
                return 1;
            else if (scr1 >= 40)
                return 2;
            else return 0;
        }

    }
}
