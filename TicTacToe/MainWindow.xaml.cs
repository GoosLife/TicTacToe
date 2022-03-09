using System;
using System.Collections.Generic;
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

namespace TicTacToe
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        static List<Button> buttons = new List<Button>();

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
            if ((int)w > -1)
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
    }
}
