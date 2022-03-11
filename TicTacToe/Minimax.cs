using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace TicTacToe
{
    internal class Minimax
    {
        static List<Snapshot> Sequence = new List<Snapshot>();

        /// <summary>
        /// Perform minimax algorithm on the current gameboard
        /// </summary>
        /// <param name="board">The initial board position.</param>
        /// <param name="depth">The search depth.</param>
        /// <param name="isMax">Whether to search for the current turn player or his opponent.</param>
        /// <returns></returns>
        static int PerformMinimax(Board board, int depth, bool isMax)
        {
            int score = GameManager.Evaluate(board);

            // X has won
            if (score == 10)
            {
                return score - depth;
            }
            else if (score == -10)
            {
                return score + depth;
            }

            // If no moves are left, return draw
            if (!IsMovesLeft(board))
            {
                return 0;
            }

            if (isMax)
            {
                int best = -1000;

                for (int row = 0; row < board.Size; row++)
                {
                    for (int col = 0; col < board.Size; col++)
                    {
                        if (board.Squares[row, col].Symbol == Symbol.Blank)
                        {
                            board.Squares[row, col].Symbol = Symbol.X;

                            best = Math.Max(best, PerformMinimax(board, depth + 1, !isMax));

                            board.Squares[row, col].Symbol = Symbol.Blank;
                        }
                    }
                }

                return best;
            }

            else
            {
                int best = 1000;

                for (int row = 0; row < board.Size; row++)
                {
                    for (int col = 0; col  < board.Size; col++)
                    {
                        if (board.Squares[row,col].Symbol == Symbol.Blank)
                        {
                            board.Squares[row, col].Symbol = Symbol.O;

                            best = Math.Min(best, PerformMinimax(board, depth + 1, !isMax));

                            board.Squares[row, col].Symbol = Symbol.Blank;
                        }
                    }
                }

                return best;
            }
        }

        public static Move FindBestMove(Board board)
        {
            int bestVal = -1000;
            bool isMax = false;

            if (GameManager.turn == -10)
            {
                bestVal = 1000;
                isMax = true;
            }
            

            Move bestMove = new Move();
            bestMove.Row = -1;
            bestMove.Col = -1;

            for (int row = 0; row < board.Size; row++)
            {
                for (int col = 0; col < board.Size; col++)
                {
                    if (board.Squares[row,col].Symbol == Symbol.Blank)
                    {
                        board.Squares[row, col].Symbol = (Symbol)GameManager.turn;

                        int moveVal = PerformMinimax(board, 0, isMax);

                        // Undo move 
                        board.Squares[row, col].Symbol = Symbol.Blank;

                        if (GameManager.turn == 10)
                        {
                            if (moveVal > bestVal)
                            {
                                bestMove.Row = row;
                                bestMove.Col = col;
                                bestVal = moveVal;

                            }
                        }
                        else
                        {
                            if (moveVal < bestVal)
                            {
                                bestMove.Row = row;
                                bestMove.Col = col;
                                bestVal = moveVal;
                            }
                        }
                    }
                }
            }

            return bestMove;
        }

        public static bool IsMovesLeft(Board board)
        {
            for (int row = 0; row < board.Size; row++)
            {
                for (int col = 0; col < board.Size; col++)
                {
                    if (board.Squares[row,col].Symbol == Symbol.Blank)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
