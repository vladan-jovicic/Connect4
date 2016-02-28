using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect4
{
    class RandomPlayer
    {
        Random rd;
        public RandomPlayer()
        {
            rd = new Random();
        }

        public int nextMove()
        {
            int p = rd.Next(0, 7);
            return p;
        }
    }
}
