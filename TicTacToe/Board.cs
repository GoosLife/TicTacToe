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

        /// <summary>
        /// Prints a human-friendly, easily readable board.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string result = "";

            for (int row = 0; row < Size; row++)
            {
                result += ($"\n------\n");

                for (int col = 0; col < Size; col++)
                {
                    if (Squares[row, col].Symbol == Symbol.Blank)
                    {
                        result += $"-|";
                    }
                    else
                    {
                        result += Squares[row,col].Symbol + "|";
                    }
                }
            }

            return result;
        }
    }
}
