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
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool blockingMode = false;
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
            btn00.Click += GameOnClick;
            btn00.MouseEnter += GameMouseOver;
            btn00.MouseLeave += GameMouseLeave;
        }

        private void GameOnClick(object sender, RoutedEventArgs e)
        {
            var btn = (Button)sender;
            string btnName = btn.Name;
            int i, j = 0;
            int.TryParse(btnName[btnName.Length - 2].ToString(), out i);
            int.TryParse(btnName[btnName.Length - 1].ToString(), out j);
            if (gameBoard[i,j] == 'e' && blockingMode == false)
            {
                //sprawdzić czy ruch jest możliwy
                //ustawić dla obecnej pozycji pionka tło active_none
                //ustawić dla klikniętego pola tło active_none_white_pawn lub ..._black_pawn
                //zaktualizować gameBoard oraz whitePawnPosition lub blackPawnPosition
            }
            else if (gameBoard[i, j] == 'e' && blockingMode == true)
            {
                //ustawić dla wybranego pola tło inactive
                //zaktaulizować gameBoard
            }
            else if (gameBoard[i, j] == 'b' && blockingMode == true)
            {
                //ruch niemożliwy
            }
            else if (gameBoard[i, j] == 'w' && blockingMode == true)
            {
                //wyświetlić informację, że należy kliknąć w pole, do którego chcemy przemieścić pionka
            }
            else
            {
                //pozostałe przypadki?
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
    }
}
