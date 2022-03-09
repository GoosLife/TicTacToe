using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    internal class Board
    {
        public int Size { get; set; }
        public Square[,] Squares { get; set; }

        public Board(int size, List<Square> squares)
        {
            Size = size;
            Squares = new Square[Size, Size];

            foreach (Square square in squares)
            {
                Squares[square.Row, square.Column] = square;
            }
        }
    }
}
