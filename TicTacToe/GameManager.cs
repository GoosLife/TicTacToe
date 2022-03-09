using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace TicTacToe
{
    public enum Winner
    {
        Draw = -1,
        O = 0,
        X = 1,
    }

    public enum Turn
    {
        O = 0,
        X = 1
    }

    /// <summary>
    /// Handles game logic
    /// </summary>
    internal class GameManager
    {
        /// <summary>
        /// The board associated with the current game
        /// </summary>
        public static Board board;

        /// <summary>
        /// The squares associated with the current game.
        /// </summary>
        public static List<Square> squares = new List<Square>();

        /// <summary>
        /// Keeps track of whose turn it currently is.
        /// | 1 = X
        /// | 0 = O
        /// </summary>
        public static int turn = 1;

        /// <summary>
        /// Keeps track of how many moves have been taken in the current game.
        /// </summary>
        public static int moveCount = 0;

        /// <summary>
        /// Initialize a new instance of the game.
        /// </summary>
        /// <param name="buttons">A list of the buttons comprising the gameboard.</param>
        public static void InitGame(List<Button> buttons)
        {
            foreach (Button btn in buttons)
            {
                Square s = new Square(btn);

                s.Row = Grid.GetRow(btn);
                s.Column = Grid.GetColumn(btn);

                squares.Add(s);
            }

            board = new Board(3, squares);
        }

        /// <summary>
        /// All the actions that take place within 1 game turn.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void Turn(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            Square clickedSquare = Square.GetByName(squares, btn.Name);

            if (GameManager.IsMoveValid(sender, e, clickedSquare, btn))
            {
                GameManager.AddSymbol(sender, e, clickedSquare, btn);
                GameManager.AdvanceTurn();


                // Update clickedSquare so it matches current symbol value
                clickedSquare = Square.GetByName(squares, btn.Name);

                // Check if a winner can be found
                Winner? winner = FindWinner(clickedSquare, moveCount);

                // Announce winner if found
                if (winner != null)
                {
                    MainWindow.AnnounceWinner(winner);
                    MainWindow.DisableBoard();
                }
            }
        }

        /// <summary>
        /// Checks if a proposed move is valid or not. A player can propose a move by clicking a button on the board.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <param name="square">The square of the proposed move.</param>
        /// <param name="btn">The button associated with the square.</param>
        /// <returns></returns>
        public static bool IsMoveValid(object sender, RoutedEventArgs e, Square square, Button btn)
        {
            if (btn.Content == null)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Adds the current symbol (X or O) to the square in question,
        /// then draws that symbol to the button associated with the square.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <param name="s"></param>
        /// <param name="btn"></param>
        public static void AddSymbol(object sender, RoutedEventArgs e, Square s, Button btn)
        {
            s.Symbol = (Symbol)turn; // Sets the symbol on the square object

            s.Button.Content = (Turn)turn; // Adds proper symbol to button
        }

        /// <summary>
        /// Check for win conditions
        /// </summary>
        /// <param name="board">The gameboard to check</param>
        /// <param name="square">The square the latest move happened on</param>
        /// <returns></returns>
        public static Winner? FindWinner(Square square, int moveCount)
        {
            int row = square.Row;
            int col = square.Column;

            // Check columns
            for (int i = 0; i <= board.Size; i++)
            {
                if (board.Squares[row,i].Symbol != square.Symbol)
                {
                    break;
                }

                if (i == board.Size - 1)
                    return (Winner)square.Symbol;
            }

            // Check rows
            for (int i = 0; i < board.Size; i++)
            {
                if (board.Squares[i,col].Symbol != square.Symbol)
                {
                    break;
                }

                if (i == board.Size - 1)
                    return (Winner)square.Symbol;
            }

            // Check main diagonal
            if (row == col)
            {
                for (int i = 0; i < board.Size; i++)
                {
                    if (board.Squares[i,i].Symbol != square.Symbol)
                    {
                        break;
                    }

                    if (i == board.Size - 1)
                        return (Winner)square.Symbol;
                }
            }

            // Check antidiagonal
            if (row + col == board.Size - 1)
            {
                for (int i = 0; i < board.Size; i++)
                {
                    if (board.Squares[i, ((board.Size - 1) - i)].Symbol != square.Symbol)
                    {
                        break;
                    }

                    if (i == board.Size - 1)
                        return (Winner)square.Symbol;
                }
            }

            // Check draw
            if (moveCount == (Math.Pow(board.Size,2)))
            {
                return Winner.Draw;
            }

            return null;
        }

        /// <summary>
        /// Advance the turn to the next player.
        /// </summary>
        private static void AdvanceTurn()
        {
            if (turn == 1)
            {
                turn = 0;
            }
            else
            {
                turn = 1;
            }

            moveCount++;
        }

        /// <summary>
        /// Reset all the parameters required to start the game from scratch.
        /// </summary>
        public static void ResetGame()
        {
            squares = new List<Square>();
            turn = 1;
            moveCount = 0;
        }
    }
}
