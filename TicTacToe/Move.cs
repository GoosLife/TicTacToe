using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    internal class Move
    {
        public int Row { get; set; }
        public int Col { get; set; }

        /// <summary>
        /// Creates a new move with the default coordinates (-1,-1). The coordinates must then be manually set.
        /// </summary>
        public Move()
        {
            Row = -1;
            Col = -1;
        }
    }
}
