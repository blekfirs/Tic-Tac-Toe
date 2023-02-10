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

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        private Button[] buttons = new Button[9];
        private string[] positions = new string[9];
        private string[] currentPlayer = { "X", "O" };
        private int state = 0;
        private int moveCount = 0;
        public MainWindow()
        {
            InitializeComponent();

            for (int i = 0; i < 9; i++)
                buttons[i] = this.FindName("Button" + (i + 1)) as Button;

            RestartButton.Visibility = Visibility.Hidden; 
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            int buttonIndex = int.Parse(button.Name.Substring(6)) - 1;

            if (positions[buttonIndex] != null)
                return;

            button.Content = currentPlayer[state];
            positions[buttonIndex] = currentPlayer[state];
            button.IsEnabled = false;

            if (CheckForWin(currentPlayer[state]))
                return;

            if (++moveCount == 9)
            {
                ResultTextBlock.Text = "It's a draw!";
                RestartButton.Visibility = Visibility.Visible;
                DisableButtons();
                return;
            }

            shiftPlayer();
            RobotMove();
            ++moveCount;
        }

        private void shiftPlayer() => state = (state + 1) % 2;

        private void RobotMove()
        {
            List<int> availableMoves = Enumerable.Range(0, 9).Where(i => positions[i] == null).ToList();
            int randomIndex = new Random().Next(availableMoves.Count);
            int move = availableMoves[randomIndex];

            buttons[move].Content = currentPlayer[state];
            positions[move] = currentPlayer[state];
            buttons[move].IsEnabled = false;

            CheckForWin(currentPlayer[state]);
            shiftPlayer();
        }

        private bool CheckForWin(string player)
        {
            if (positions[0] == player && positions[1] == player && positions[2] == player ||
                positions[3] == player && positions[4] == player && positions[5] == player ||
                positions[6] == player && positions[7] == player && positions[8] == player ||
                positions[0] == player && positions[3] == player && positions[6] == player ||
                positions[1] == player && positions[4] == player && positions[7] == player ||
                positions[2] == player && positions[5] == player && positions[8] == player ||
                positions[0] == player && positions[4] == player && positions[8] == player ||
                positions[2] == player && positions[4] == player && positions[6] == player)
            {
                RestartButton.Visibility = Visibility.Visible;

                ResultTextBlock.Text = player + " wins!";
                DisableButtons();

                return true;
            }

            return false;
        }

        private void DisableButtons()
        {
            foreach (Button button in buttons)
                button.IsEnabled = false;
        }

        private void SwitchPlayers()
        {
            var temp = currentPlayer[0];
            currentPlayer[0] = currentPlayer[1];
            currentPlayer[1] = temp;
        }

        private void RestartButton_Click(object sender, RoutedEventArgs e)
        {
            ResultTextBlock.Text = "";
            moveCount = 0;

            SwitchPlayers();

            Array.Clear(positions, 0, positions.Length);

            foreach (Button button in buttons)
            {
                button.Content = "";
                button.IsEnabled = true;
            }

            RestartButton.Visibility = Visibility.Hidden;

            if (currentPlayer[0] == "O")
            {
                state = 1;
                RobotMove();
            }
            else
                state = 0;
        }
    }
}
