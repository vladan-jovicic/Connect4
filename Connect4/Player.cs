using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect4
{
    class Player
    {
        private PlayerType type;
        private RandomPlayer randomLogic;
        private MinimaxLogic minimaxLogic;
        private ABminimaxLogic abMinimaxLogic;
        private CircleType myCircleType;
        private int numOfMoves;
        public Player(PlayerType t, CircleType ct)
        {
            type = t;
            myCircleType = ct;
            if(type == PlayerType.RANDOM)
            {
                randomLogic = new RandomPlayer();
            }
            else if(type == PlayerType.MINIMAX)
            {
                minimaxLogic = new MinimaxLogic(myCircleType);
            }
            else if(type == PlayerType.ABMINIMAX)
            {
                abMinimaxLogic = new ABminimaxLogic(ct);
            }
            numOfMoves = 0;
        }

        public CircleType[][] nextMove(CircleType[][] state)
        {
            return state;
        }

        public PlayerType getPlayerType()
        {
            return type;
        }

        public int nextMove(CircleType[][] currentState, List<Circle>[] circles) //jos nisam odredio parametre
        {
            if(type == PlayerType.RANDOM)
            {
                return randomLogic.nextMove();
            }
            else if(type == PlayerType.MINIMAX)
            {
                return minimaxLogic.nextMove(currentState, circles, 1, 0);
            }
            else if(type == PlayerType.ABMINIMAX)
            {
                //why do I have 2 ????
                return abMinimaxLogic.nextMove(currentState, circles, 1, -1000, 1000, 0);
            }
            else
            {
                //THIS SHOULD NOT HAPPEN
                return 0;
            }
        }
        public int GetNumberOfMoves()
        {
            return numOfMoves;
        }
        public void IncreaseMoves()
        {
            numOfMoves++;
        }
    }
}
