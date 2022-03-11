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
        Draw = 0,
        O = -10,
        X = 10,
    }

    public enum Turn
    {
        O = -10,
        X = 10
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
        /// | 10 = X
        /// | -10 = O
        /// </summary>
        public static int turn = 10;

        /// <summary>
        /// Keeps track of how many moves have been taken in the current game.
        /// </summary>
        public static int moveCount = 0;

        /// <summary>
        /// The symbol the computer controls.
        /// </summary>
        public static Symbol? AIPlayer = null;

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
            if ((AIPlayer == null) || ((Symbol)turn != AIPlayer))
            {
                Button btn = sender as Button;
                Square clickedSquare = Square.GetByName(squares, btn.Name);

                if (GameManager.IsMoveValid(clickedSquare, btn))
                {
                    GameManager.AddSymbol(clickedSquare, btn);
                    GameManager.AdvanceTurn();


                    // Update clickedSquare so it matches current symbol value
                    clickedSquare = Square.GetByName(squares, btn.Name);

                    // Check if a winner can be found
                    Winner? winner = FindWinner(clickedSquare);

                    // Announce winner if found
                    if (winner != null)
                    {
                        MainWindow.AnnounceWinner(winner);
                        MainWindow.DisableBoard();
                    }
                }
            }
        }

        /// <summary>
        /// Set a nullable symbol to a value specified by a provided string.
        /// Used to set the AI player to the correct symbol using the string
        /// value from the combobox cbAISymbol.
        /// </summary>
        /// <param name="s">The symbol. May be X, O or Blank, otherwise returns null.</param>
        /// <returns>A nullable symbol of the specified value.</returns>
        public static Symbol? SetSymbol(string s)
        {
            if (!string.IsNullOrEmpty(s))
            {
                switch (s)
                {
                    case "X":
                        return Symbol.X;
                    case "O":
                        return Symbol.O;
                    case "Blank":
                        return Symbol.Blank;
                    default:
                        return null;
                }
            }

            return null;
        }

        /// <summary>
        /// Give the AI a symbol to play
        /// </summary>
        /// <param name="s">The symbol to play.</param>
        public static void SetAIPlayer(Symbol? s)
        {
            AIPlayer = s;
        }

        /// <summary>
        /// The AI takes one turn.
        /// </summary>
        public static void AITurn()
        {
            if (moveCount < 9)
            {
                // Find the best move
                Move move = Minimax.FindBestMove(board);
                
                // Simulate clicking on the board
                Square clickedSquare = board.Squares[move.Row, move.Col];
                Button btn = clickedSquare.Button;

                // Add symbol and advance turn if move is valid
                if (GameManager.IsMoveValid(clickedSquare, btn))
                {
                    GameManager.AddSymbol(clickedSquare, btn);
                    GameManager.AdvanceTurn();


                    // Update clickedSquare variable so it matches current symbol value
                    clickedSquare = Square.GetByName(squares, btn.Name);

                    // Check if a winner can be found
                    Winner? winner = FindWinner(clickedSquare);

                    // Announce winner if found
                    if (winner != null)
                    {
                        MainWindow.AnnounceWinner(winner);
                        MainWindow.DisableBoard();
                    }
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
        public static bool IsMoveValid(Square square, Button btn)
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
        public static void AddSymbol(Square s, Button btn)
        {
            s.Symbol = (Symbol)turn; // Sets the symbol on the square object

            s.Button.Content = (Turn)turn; // Adds proper symbol to button
        }

        /// <summary>
        /// Check for win conditions
        /// </summary>
        /// <param name="square">The square the latest move happened on</param>
        /// <returns></returns>
        public static Winner? FindWinner(Square square)
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
        /// Evaluate a hypothetical board for the minimax algorithm.
        /// Instead of picking a winner, this function assigns a numerical value to the gamestates.
        /// </summary>
        /// <param name="board">The board to check</param>
        /// <param name="square">The square the latest move happened on</param>
        /// <param name="moveCount">How many moves have been performed so far.</param>
        /// <returns></returns>
        public static int Evaluate(Board board)
        {

            // Check rows 

            for (int row = 0; row < board.Size; row++)
            {
                if ((board.Squares[row,0].Symbol == board.Squares[row,1].Symbol &&
                    board.Squares[row,1].Symbol == board.Squares[row,2].Symbol) &&
                    board.Squares[row,0].Symbol != Symbol.Blank)
                {
                    if (board.Squares[row,0].Symbol == Symbol.X)
                    {
                        return (int)Symbol.X;
                    }
                    else if (board.Squares[row,0].Symbol == Symbol.O)
                    {
                        return (int)Symbol.O;
                    }
                }
            }
            
            // Check columns
                
            for (int col = 0; col < board.Size; col++)
            {
                if ((board.Squares[0,col].Symbol == board.Squares[1,col].Symbol &&
                    board.Squares[1,col].Symbol == board.Squares[2,col].Symbol) &&
                    board.Squares[0,col].Symbol != Symbol.Blank)
                {
                    if (board.Squares[0, col].Symbol == Symbol.X)
                    {
                        return (int)Symbol.X;
                    }
                    else if (board.Squares[col, 0].Symbol == Symbol.O)
                    {
                        return (int)Symbol.O;
                    }
                }
            }

            // Check main diagonal

            if ((board.Squares[0,0].Symbol == board.Squares[1,1].Symbol &&
                board.Squares[1,1].Symbol == board.Squares[2,2].Symbol) &&
                board.Squares[0,0].Symbol != Symbol.Blank)
            {
                if (board.Squares[0,0].Symbol == Symbol.X)
                {
                    return (int)Symbol.X;
                }
                else if (board.Squares[0,0].Symbol == Symbol.O)
                {
                    return (int)Symbol.O;
                }
            }

            // Check anti-diagonal
            
            if ((board.Squares[0, 2].Symbol == board.Squares[1, 1].Symbol &&
                board.Squares[1, 1].Symbol == board.Squares[2, 0].Symbol) &&
                board.Squares[0,2].Symbol != Symbol.Blank)
            {
                if (board.Squares[0,2].Symbol == Symbol.X)
                {
                    return (int)Symbol.X;
                }
                else if (board.Squares[0,2].Symbol == Symbol.O)
                {
                    return (int)Symbol.O;
                }
            }

            return 0; // No winners
        }

        /// <summary>
        /// Allows the minimax function to learn its opponents symbol.
        /// </summary>
        /// <param name="s">The symbol of the minimax player</param>
        /// <returns></returns>
        public static Symbol GetOpponent(Symbol s)
        {
            if (s == Symbol.O)
            {
                return Symbol.X;
            }
            else
            {
                return Symbol.O;
            }
        }

        /// <summary>
        /// Advance the turn to the next player.
        /// </summary>
        private static void AdvanceTurn()
        {
            if (turn == 10)
            {
                turn = -10;
            }
            else
            {
                turn = 10;
            }

            moveCount++;
        }

        /// <summary>
        /// Reset all the parameters required to start the game from scratch.
        /// </summary>
        public static void ResetGame()
        {
            squares = new List<Square>();
            turn = 10;
            moveCount = 0;
        }
    }
}
