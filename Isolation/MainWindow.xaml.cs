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
            SetProperBackground();
            ResetGameButton.Click += ResetGame;
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
                switch (gameBoard[i,j])
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
                                }
                                else
                                {
                                    gameBoard[i, j] = 'b';
                                    gameBoard[blackPawnPosition[0], blackPawnPosition[1]] = 'e';
                                    blackPawnPosition[0] = i;
                                    blackPawnPosition[1] = j;
                                    whiteTurn = true;
                                    ErrorBox.Text = "";
                                }
                                SetProperBackground();
                                break;
                            }
                            else
                            {
                                ErrorBox.Text = "Nieprawidłowy ruch!";
                                break;
                            } 
                        }
                    default:
                        {
                            ErrorBox.Text = "Ruch niemożliwy!";
                            break;
                        }
                }

            }
            else
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
                            }
                            else
                            {
                                gameBoard[i, j] = 'd';
                                whiteTurn = true;
                                ErrorBox.Text = "";
                            }
                            SetProperBackground();
                            break;
                        }
                    default:
                        {
                            ErrorBox.Text = "Ruch niemożliwy!";
                            break;
                        }
                }
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

            if (gameBoard[i,j] == 'e')
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
    }
}
