using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect4
{
    class MinimaxLogic
    {
        private const int HEIGHT = 6;
        private const int WIDTH = 7;
        private int maxDepth = 6;
        private CircleType mytype;
        public MinimaxLogic(CircleType ct)
        {
            mytype = ct;
        }


        private CircleType OpponentType()
        {
            return ((mytype == CircleType.RED) ? CircleType.YELLOW : CircleType.RED);
        }
        public int nextMove(CircleType[][] currentState, List<Circle>[] circles, int minmax, int currentDepth)
        {
            if (currentDepth > maxDepth)
            {
                int scr = hFunction(mytype, currentState) * 10;
                int scr1 = hFunction(OpponentType(), currentState) *10;
                if (Math.Abs(scr) == Math.Abs(scr1))
                {
                    if (minmax == 1)
                        return scr;
                    else
                        return scr1;
                }
                else if (Math.Abs(scr) > Math.Abs(scr1))
                    return scr;
                else
                    return scr1;
            }
            int score = UtilityFunction(currentState);
            if (score != -100)
                return score;
            if (minmax == 1)
            {
                //maximiziraj
                int bestscore = -2000;
                int bestmove = -1;
                for (int i = 0; i < 7; i++)
                {
                    if (isMoveRegular(i, circles))
                    {
                        addCircle(i, ref currentState, ref circles, mytype);
                        int p = nextMove(currentState, circles, 2, currentDepth + 1);
                        if (p >= bestscore)
                        {
                            bestscore = p;
                            bestmove = i;
                        }
                        removeCircle(i, ref currentState, ref circles);
                    }
                }
                if (currentDepth == 0)
                    return bestmove;
                else
                    return bestscore;
            }
            else
            {
                //minimiziraj
                int bestscore = 2000;
                int bestmove = -1;
                for (int i = 0; i < 7; i++)
                {
                    if (isMoveRegular(i, circles))
                    {
                        addCircle(i, ref currentState, ref circles, OpponentType());
                        int p = nextMove(currentState, circles, 1, currentDepth + 1);
                        if (p <= bestscore)
                        {
                            bestscore = p;
                            bestmove = i;
                        }
                        removeCircle(i, ref currentState, ref circles);
                    }
                }
                if (currentDepth == 0)
                    return bestmove;
                else
                    return bestscore;
            }
            //return 0;
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

        private int UtilityFunction(CircleType[][] currentState)
        {
            int p = checkEnd(currentState); // OfGame(currentState);
            if (p == 1)
            {
                if (mytype == CircleType.RED)
                    return 1000;
                else
                    return -1000;
            }
            else if (p == 2)
            {
                if (mytype == CircleType.YELLOW)
                    return 1000;
                else
                    return -1000;
            }
            else
                return -100;
        }
        //izboljsaj funkciju
        public int hFunction(CircleType ct, CircleType[][] currentState)
        {
            int maximum = 0;
            for (int i = 0; i < HEIGHT; i++)
            {
                for (int j = 0; j < WIDTH; j++)
                {
                    if (currentState[i][j] == CircleType.NONE)
                        continue;
                    //desno
                    int cnt = 0;
                    for (int k = j; k < WIDTH; k++)
                    {
                        if (currentState[i][k] == ct)
                            cnt++;
                        else
                            break;
                    }
                    maximum = Math.Max(maximum, cnt);
                    cnt = 0;
                    for (int k = i; k < HEIGHT; k++) // dole
                    {
                        if (currentState[k][j] == ct)
                            cnt++;
                        else
                            break;
                    }
                    maximum = Math.Max(maximum, cnt);
                    cnt = 0;
                    for (int k = i, l = j; k < HEIGHT && l < WIDTH; k++, l++) //koso desno
                    {
                        if (currentState[k][l] == ct)
                            cnt++;
                        else
                            break;
                    }
                    maximum = Math.Max(maximum, cnt);
                    cnt = 0;
                    for (int k = i, l = j; k < HEIGHT && l >= 0; k++, l--)
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
            int scr = hFunction(CircleType.RED, state) * 10; // hFunction(myType, state);
            int scr1 = hFunction(CircleType.YELLOW, state) * 10; // hFunction(OpponentType(), state);
            if (scr >= 40)
                return 1;
            else if (scr1 >= 40)
                return 2;
            else return 0;
        }
    }
}
