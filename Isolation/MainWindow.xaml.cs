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
using Minimax;

namespace Isolation
{
    public partial class MainWindow : Window
    {
        private bool whiteTurn = true;
        private int[] whitePawnPosition = new int[2] { 6, 3 };
        private int[] blackPawnPosition = new int[2] { 0, 3 };
        private char[,] gameBoard = new char[7, 7] {{'e','e','e','b','e','e','e'}, // e <-- empty
                                                    {'e','e','e','e','e','e','e'}, // b <-- black pawn
                                                    {'e','e','e','e','e','e','e'}, // w <-- white pawn
                                                    {'e','e','e','e','e','e','e'}, // d <-- blocked
                                                    {'e','e','e','e','e','e','e'},
                                                    {'e','e','e','e','e','e','e'},
                                                    {'e','e','e','w','e','e','e'},
                                                    };
        private int turnsCount = 0;
        private bool playerOpponent = true;
        private bool gameWasWon = false;

        public MainWindow()
        {
            InitializeComponent();
            List<Button> buttons = new List<Button>() { btn00, btn01, btn02, btn03, btn04, btn05, btn06,
                                                        btn10, btn11, btn12, btn13, btn14, btn15, btn16,
                                                        btn20, btn21, btn22, btn23, btn24, btn25, btn26,
                                                        btn30, btn31, btn32, btn33, btn34, btn35, btn36,
                                                        btn40, btn41, btn42, btn43, btn44, btn45, btn46,
                                                        btn50, btn51, btn52, btn53, btn54, btn55, btn56,
                                                        btn60, btn61, btn62, btn63, btn64, btn65, btn66};
            foreach (var button in buttons)
            {
                button.PreviewMouseDown += GameOnClick;
                button.MouseEnter += GameMouseOver;
                button.MouseLeave += GameMouseLeave;
            }
            Pvp.Click += ChooseOpponent;
            Pvs.Click += ChooseOpponent;
            SetProperBackground();
            ResetGameButton.Click += ResetGame;
        }

        private void ChooseOpponent(object sender, RoutedEventArgs e)
        {
            var cb = (CheckBox)sender;
            if (cb.Name == "Pvp")
            {
                Pvs.IsChecked = false;
                Pvp.IsChecked = true;
                if (turnsCount > 0)
                {
                    MessageBox.Show("Change will take effect in new game.");
                    playerOpponent = true;
                }
                else
                {
                    playerOpponent = true;
                    ResetGameButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                }
            }
            else
            {
                Pvp.IsChecked = false;
                Pvs.IsChecked = true;
                if (turnsCount > 0)
                {
                    MessageBox.Show("Change will take effect in new game.");
                    playerOpponent = false;
                }
                else
                {
                    playerOpponent = false;
                    ResetGameButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                }
            }
        }
        private void ResetGame(object sender, RoutedEventArgs e)
        {
            gameBoard = new char[7, 7] {{'e','e','e','b','e','e','e'},
                                        {'e','e','e','e','e','e','e'},
                                        {'e','e','e','e','e','e','e'},
                                        {'e','e','e','e','e','e','e'},
                                        {'e','e','e','e','e','e','e'},
                                        {'e','e','e','e','e','e','e'},
                                        {'e','e','e','w','e','e','e'},
                                        };
            whiteTurn = true;
            whitePawnPosition[0] = 6;
            whitePawnPosition[1] = 3;
            blackPawnPosition[0] = 0;
            blackPawnPosition[1] = 3;
            SetProperBackground();
            turnsCount = 0;
            ErrorBox.Text = "";
        }
        private void GameOnClick(object sender, MouseButtonEventArgs e)
        {
            var btn = (Button)sender;
            string btnName = btn.Name;
            int i, j = 0;
            int.TryParse(btnName[btnName.Length - 2].ToString(), out i);
            int.TryParse(btnName[btnName.Length - 1].ToString(), out j);

            if (e.ChangedButton == MouseButton.Left)
            {
                switch (gameBoard[i, j])
                {
                    case 'e':
                        {
                            int[] MoveCords = new int[2] { i, j };
                            if (MoveIsValid(MoveCords))
                            {
                                if (whiteTurn == true)
                                {
                                    gameBoard[i, j] = 'w';
                                    gameBoard[whitePawnPosition[0], whitePawnPosition[1]] = 'e';
                                    whitePawnPosition[0] = i;
                                    whitePawnPosition[1] = j;
                                    whiteTurn = false;
                                    ErrorBox.Text = "";
                                    SetProperBackground();
                                    if (gameWasWon)
                                    {
                                        gameWasWon = false;
                                        return;
                                    }
                                    turnsCount++;
                                }
                                else
                                {
                                    gameBoard[i, j] = 'b';
                                    gameBoard[blackPawnPosition[0], blackPawnPosition[1]] = 'e';
                                    blackPawnPosition[0] = i;
                                    blackPawnPosition[1] = j;
                                    whiteTurn = true;
                                    ErrorBox.Text = "";
                                    SetProperBackground();
                                    if (gameWasWon)
                                    {
                                        gameWasWon = false;
                                        return;
                                    }
                                    turnsCount++;
                                }
                            }
                            else
                            {
                                ErrorBox.Text = "Nieprawidłowy ruch!";
                            }
                            break;
                        }
                    default:
                        {
                            ErrorBox.Text = "Nieprawidłowy ruch!";
                            break;
                        }
                }

            }
            else if (e.ChangedButton == MouseButton.Right)
            {
                switch (gameBoard[i, j])
                {
                    case 'e':
                        {
                            if (whiteTurn == true)
                            {
                                gameBoard[i, j] = 'd';
                                whiteTurn = false;
                                ErrorBox.Text = "";
                                SetProperBackground();
                                if (gameWasWon)
                                {
                                    gameWasWon = false;
                                    return;
                                }
                                turnsCount++;
                            }
                            else
                            {
                                gameBoard[i, j] = 'd';
                                whiteTurn = true;
                                ErrorBox.Text = "";
                                SetProperBackground();
                                if (gameWasWon)
                                {
                                    gameWasWon = false;
                                    return;
                                }
                                turnsCount++;
                            }
                            break;
                        }
                    default:
                        {
                            ErrorBox.Text = "Ruch niemożliwy!";
                            break;
                        }
                }
            }
            if (playerOpponent == false && ErrorBox.Text == "")
            {
                ErrorBox.Text = "";
                whiteTurn = true;
                SiMove();
                SetProperBackground();
                if (gameWasWon)
                {
                    gameWasWon = false;
                    return;
                }
                turnsCount++;
            }
        }
        private void GameMouseOver(object sender, RoutedEventArgs e)
        {
            var btn = (Button)sender;
            string btnName = btn.Name;
            int i, j = 0;
            int.TryParse(btnName[btnName.Length - 2].ToString(), out i);
            int.TryParse(btnName[btnName.Length - 1].ToString(), out j);
            ImageBrush tempImg = new ImageBrush();

            if (gameBoard[i, j] == 'e')
            {
                tempImg.ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/Assets/Active_mouseover.png"));
                btn.Background = tempImg;
            }
            else if (gameBoard[i, j] == 'b')
            {
                tempImg.ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/Assets/Active_mouseover_black_pawn.png"));
                btn.Background = tempImg;
            }
            else if (gameBoard[i, j] == 'w')
            {
                tempImg.ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/Assets/Active_mouseover_white_pawn.png"));
                btn.Background = tempImg;
            }
            else
            {
                tempImg.ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/Assets/Inactive_mouseover.png"));
                btn.Background = tempImg;
            }
        }
        private void GameMouseLeave(object sender, RoutedEventArgs e)
        {
            var btn = (Button)sender;
            string btnName = btn.Name;
            int i, j = 0;
            int.TryParse(btnName[btnName.Length - 2].ToString(), out i);
            int.TryParse(btnName[btnName.Length - 1].ToString(), out j);
            ImageBrush tempImg = new ImageBrush();

            if (gameBoard[i, j] == 'e')
            {
                tempImg.ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/Assets/Active_none.png"));
                btn.Background = tempImg;
            }
            else if (gameBoard[i, j] == 'b')
            {
                tempImg.ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/Assets/Active_none_black_pawn.png"));
                btn.Background = tempImg;
            }
            else if (gameBoard[i, j] == 'w')
            {
                tempImg.ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/Assets/Active_none_white_pawn.png"));
                btn.Background = tempImg;
            }
            else
            {
                tempImg.ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/Assets/Inactive_none.png"));
                btn.Background = tempImg;
            }
        }
        private void SetProperBackground()
        {
            if (GameWon())
            {
                MessageBoxResult result;
                if (whiteTurn)
                {
                    result = MessageBox.Show("Black won!\nClicking \"OK\" will start a new game.\nClicking \"Cancel\" will close the game.", "Isolation Game", MessageBoxButton.OKCancel);
                    Turn.Text = "Black won!";
                }
                else
                {
                    result = MessageBox.Show("White won!\nClicking \"OK\" will start a new game.\nClicking \"Cancel\" will close the game.", "Isolation Game", MessageBoxButton.OKCancel);
                    Turn.Text = "White won!";
                }
                switch (result)
                {
                    case MessageBoxResult.OK:
                        {
                            whiteTurn = true;
                            ErrorBox.Text = "";
                            ResetGameButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                            gameWasWon = true;
                            break;
                        }
                    case MessageBoxResult.Cancel:
                        {
                            Environment.Exit(0);
                            break;
                        }
                }
                return;
            }
            else
            {
                if (whiteTurn == true) Turn.Text = "Turn: White";
                else Turn.Text = "Turn: Black";
                List<Button> buttons = new List<Button>() { btn00, btn01, btn02, btn03, btn04, btn05, btn06,
                                                        btn10, btn11, btn12, btn13, btn14, btn15, btn16,
                                                        btn20, btn21, btn22, btn23, btn24, btn25, btn26,
                                                        btn30, btn31, btn32, btn33, btn34, btn35, btn36,
                                                        btn40, btn41, btn42, btn43, btn44, btn45, btn46,
                                                        btn50, btn51, btn52, btn53, btn54, btn55, btn56,
                                                        btn60, btn61, btn62, btn63, btn64, btn65, btn66};
                foreach (var button in buttons)
                {
                    string btnName = button.Name;
                    int i, j = 0;
                    int.TryParse(btnName[btnName.Length - 2].ToString(), out i);
                    int.TryParse(btnName[btnName.Length - 1].ToString(), out j);
                    ImageBrush tempImg = new ImageBrush();
                    if (gameBoard[i, j] == 'e')
                    {
                        tempImg.ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/Assets/Active_none.png"));
                        button.Background = tempImg;
                    }
                    else if (gameBoard[i, j] == 'b')
                    {
                        tempImg.ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/Assets/Active_none_black_pawn.png"));
                        button.Background = tempImg;
                    }
                    else if (gameBoard[i, j] == 'w')
                    {
                        tempImg.ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/Assets/Active_none_white_pawn.png"));
                        button.Background = tempImg;
                    }
                    else
                    {
                        tempImg.ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/Assets/Inactive_none.png"));
                        button.Background = tempImg;
                    }
                }
            }           
        }
        private bool MoveIsValid(int[] newCords)
        {
            int[] PawnPosition;
            if (whiteTurn == true)
            {
                PawnPosition = whitePawnPosition;
            }
            else
            {
                PawnPosition = blackPawnPosition;
            }

            if (PawnPosition[0] == newCords[0] && PawnPosition[1] + 1 == newCords[1])
            {
                return true;
            }
            else if (PawnPosition[0] == newCords[0] && PawnPosition[1] - 1 == newCords[1])
            {
                return true;
            }
            else if (PawnPosition[0] + 1 == newCords[0] && PawnPosition[1] - 1 == newCords[1])
            {
                return true;
            }
            else if (PawnPosition[0] - 1 == newCords[0] && PawnPosition[1] - 1 == newCords[1])
            {
                return true;
            }
            else if (PawnPosition[0] - 1 == newCords[0] && PawnPosition[1] == newCords[1])
            {
                return true;
            }
            else if (PawnPosition[0] + 1 == newCords[0] && PawnPosition[1] == newCords[1])
            {
                return true;
            }
            else if (PawnPosition[0] - 1 == newCords[0] && PawnPosition[1] + 1 == newCords[1])
            {
                return true;
            }
            else if (PawnPosition[0] + 1 == newCords[0] && PawnPosition[1] + 1 == newCords[1])
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private bool GameWon()
            {
            int[] PawnPos = new int[2];
            char enemy;
            int tempCounter = 0;
            if (whiteTurn == true)
            {
                PawnPos = whitePawnPosition;
                enemy = 'b';
            }
            else
            {
                PawnPos = blackPawnPosition;
                enemy = 'w';
            }
            //[0] - X
            //[1] - Y
            if (PawnPos[1] == 6 || gameBoard[PawnPos[0], PawnPos[1] + 1] == 'd' || gameBoard[PawnPos[0], PawnPos[1] + 1] == enemy) tempCounter++;//right
            if (PawnPos[1] == 6 || PawnPos[0] == 0 || gameBoard[PawnPos[0] - 1, PawnPos[1] + 1] == 'd' || gameBoard[PawnPos[0] - 1, PawnPos[1] + 1] == enemy) tempCounter++;//up-right
            if (PawnPos[1] == 6 || PawnPos[0] == 6 || gameBoard[PawnPos[0] + 1, PawnPos[1] + 1] == 'd' || gameBoard[PawnPos[0] + 1, PawnPos[1] + 1] == enemy) tempCounter++;//down-right
            if (PawnPos[0] == 0 || gameBoard[PawnPos[0] - 1, PawnPos[1]] == 'd' || gameBoard[PawnPos[0] - 1, PawnPos[1]] == enemy) tempCounter++;//up
            if (PawnPos[1] == 0 || PawnPos[0] == 0 || gameBoard[PawnPos[0] - 1, PawnPos[1] - 1] == 'd' || gameBoard[PawnPos[0] - 1, PawnPos[1] - 1] == enemy) tempCounter++;//up-left
            if (PawnPos[1] == 0 || gameBoard[PawnPos[0], PawnPos[1] - 1] == 'd' || gameBoard[PawnPos[0], PawnPos[1] - 1] == enemy) tempCounter++;//left
            if (PawnPos[1] == 0 || PawnPos[0] == 6 || gameBoard[PawnPos[0] + 1, PawnPos[1] - 1] == 'd' || gameBoard[PawnPos[0] + 1, PawnPos[1] - 1] == enemy) tempCounter++;//down-left
            if (PawnPos[0] == 6 || gameBoard[PawnPos[0] + 1, PawnPos[1]] == 'd' || gameBoard[PawnPos[0] + 1, PawnPos[1]] == enemy) tempCounter++;//down

            if (tempCounter == 8) return true;
            else return false;
            }
        private void SiMove()
        {
            var gameTree = new GameTree(gameBoard);
            var node = gameTree.CreateTree(playerOpponent, 4, null);

            var miniMax = new Minimax.Minimax();
            node.Value = miniMax.Compute(node, 3, int.MinValue, int.MaxValue, playerOpponent);

            int value = 0;

            value = node.Children.Min(c => c.Value);

            node.Children.Reverse();
            var nextNode = node.Children.OrderBy(c => Guid.NewGuid()).FirstOrDefault(c => c.Value == value);

            gameBoard = nextNode.GameBoardState;

            if (nextNode.ActionType == ActionType.Move)
            {
                var gameTreeTemp = new GameTree(gameBoard);
                var blackPawnCords = gameTreeTemp.GetCoords('b');
                int[] temp = new int[2] {blackPawnCords.FirstOrDefault().Y, blackPawnCords.FirstOrDefault().X };
                blackPawnPosition = temp;
            }
        }
    }
}
