using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect4
{
    class Design
    {
        Tuple<int, int>[] startingPosition;
        public Design()
        {
            startingPosition = new Tuple<int, int>[7];
            startingPosition[0] = Tuple.Create<int, int>(0, 478);
            startingPosition[1] = Tuple.Create<int, int>(0, 478);
        }
    }
}
