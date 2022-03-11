using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace TicTacToe
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public static List<Button> buttons = new List<Button>();
        public static Symbol? AIPlayer;
        DispatcherTimer AITimer = new DispatcherTimer();


        public MainWindow()
        {
            InitializeComponent();

            // Get all buttons that make up the gameboard
            foreach (Button btn in GameGrid.Children.OfType<Button>())
            {
                buttons.Add(btn);
            }
            
            // Initiate a new game
            GameManager.InitGame(buttons);

            // Set up AI timer to check if it's the AI's turn
            AITimer.Tick += new EventHandler(dispatcherTimer_Tick);
            AITimer.Interval = new TimeSpan(0, 0, 1); // Sets off once a second
        }

        /// <summary>
        /// Activates all the actions that make up one turn of the game.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Turn(object sender, RoutedEventArgs e)
        {
            GameManager.Turn(sender, e);
        }

        /// <summary>
        /// Creates a messagebox to announce who won the game,
        /// or that it is a draw.
        /// </summary>
        /// <param name="w"></param>
        public static void AnnounceWinner(Winner? w)
        {
            if ((int)w != 0)
            {
                MessageBox.Show("The winner is " + w.ToString());
            }
            else
            {
                MessageBox.Show("The game is a draw.");
            }
        }

        /// <summary>
        /// Disables all buttons that make up the gameboard
        /// once a game has been completed.
        /// </summary>
        public static void DisableBoard()
        {
            foreach (Button b in buttons)
            {
                b.IsEnabled = false;
            }
        }

        /// <summary>
        /// Re-enables all the buttons that make up the gameboard.
        /// </summary>
        private void EnableBoard()
        {
            foreach (Button b in buttons)
            {
                b.IsEnabled = true;
            }
        }

        /// <summary>
        /// Resets the window, the game and then initializes a new game.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRestart_Click(object sender, RoutedEventArgs e)
        {
            ResetWindow();
            GameManager.ResetGame();
            GameManager.InitGame(buttons);
            EnableBoard();
        }

        /// <summary>
        /// Resets all the buttons and the objects associated with the current game
        /// </summary>
        private void ResetWindow()
        {
            foreach (Button btn in GameGrid.Children.OfType<Button>())
            {
                btn.Content = null;
            }
        }

        private void btnAiTurn_Click(object sender, RoutedEventArgs e)
        {
            GameManager.AITurn();
        }

        private void cbAISymbol_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AIPlayer = GameManager.SetSymbol(cbAISymbol.SelectedValue.ToString());
            
            GameManager.SetAIPlayer(AIPlayer);

            if (AIPlayer != null)
            {
                AITimer.Start();
            }
            else
            {
                AITimer.Stop();
            }
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (GameManager.turn == (int)GameManager.AIPlayer)
            {
                GameManager.AITurn();
            }
        }
    }
}
