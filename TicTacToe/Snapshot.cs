using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
 
    /// <summary>
    /// I used this to draw various information from all stages of the minimax algorithm,
    /// for debugging purposes.
    /// I could see exactly what moves the AI was trying, and what value it was assigning them.
    /// </summary>
    internal class Snapshot
    {
        public int Size { get; set; }
        public int[,] Symbol { get; set; }

        public int Score { get; set; }
        public int Depth { get; set; }

        public Snapshot(Board b, int score, int depth)
        {
            Size = b.Size;
            Symbol = new int[Size,Size];

            Score = score;
            Depth = depth;

            foreach (Square s in b.Squares)
            {
                Symbol[s.Row, s.Column] = (int)s.Symbol;
            }
        }

        // Draw the board
        public override string ToString()
        {
            string result = "";

            for (int row = 0; row < Size; row++)
            {
                result += "\n------\n";

                for (int col = 0; col < Size; col++)
                {
                    if (Symbol[row,col] == 10)
                    {
                        result += "X|";
                    }
                    else if (Symbol[row,col] == -10)
                    {
                        result += "O|";
                    }
                    else
                    {
                        result += "-|";
                    }
                }
            }

            result += "\n\nScore: " + Score + " | Depth: " + Depth;

            return result;
        }
    }
}
