using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace CaroGame
{
    /// <summary>
    /// Interaction logic for GamePlay.xaml
    /// </summary>
    public partial class GamePlay : Window
    {
        public GamePlay()
        {
            InitializeComponent();
        }
        //Biến kiểm tra xem đi hết bàn cờ chưa
        int SumCount = 0;

        int[,] a;
        Button[,] Button;
        int Cols = 10;
        int Rows = 10;
        //private object uiCanVas;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dt.Interval = TimeSpan.FromSeconds(1);
            dt.Tick += dtTicker;

            int btnWidth = 40;
            int btnHeigh = 40;

            a = new int[Cols, Rows];
            Button = new Button[Cols, Rows];

            //đặt các btn vào canvas
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    Button[i, j] = new Button();
                    //Button[i, j].Height = btnHeigh;
                    //Button[i, j].Width = btnWidth;
                    Button[i, j].Background = Brushes.White;
                    Button[i, j].FontSize = 20;

                    Button[i, j].Tag = new Tuple<int, int>(i, j);
                    Button[i, j].Click += BtnClick;

                    //đưa buttton lên giao diện
                    //Canvas.Children.Add(Button[i, j]);
                    Grid.SetColumn(Button[i, j], j);
                    Grid.SetRow(Button[i, j], i);
                    //Canvas.SetValue(Grid.RowProperty, j);
                    Canvas.Children.Add(Button[i, j]);
                    //Canvas.SetTop(Button[i, j], i * btnHeigh);
                }
            }

        }


        bool Xturn = true;

        private void BtnClick(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var tuple = btn.Tag as Tuple<int, int>;

            int i = tuple.Item1;
            int j = tuple.Item2;

            //MessageBox.Show($"{i};{j}");
            if (a[i, j] == 0)
            {
                SumCount++;
                if (Xturn)
                {
                    btn.Content = "X";
                    btn.Foreground = Brushes.Red;
                    a[i, j] = 1;
                }
                else
                {
                    btn.Content = "O";
                    btn.Foreground = Brushes.Green;
                    a[i, j] = 2;
                }
                Xturn = !Xturn;

                //kiểm tra thắng thua
                var kt = CheckWin(a, i, j);

                if (kt == 1)
                {
                    MessageBox.Show("X Won!");
                    dt.Stop();
                    Reset();
                }
                if (kt == 2)
                {
                    MessageBox.Show("O Won!");
                    dt.Stop();
                    Reset();
                }

                if (SumCount == Cols * Rows)
                {
                    MessageBox.Show("Hòa nhau!");
                    dt.Stop();
                    Reset();
                }

                countTime = 10;
                SetTurn();
            }


        }

        public void SetTurn()
        {
            if (Xturn)
            {
                turn.Content = "X";
                turn.Foreground = Brushes.Red;
            }
            else
            {
                turn.Content = "O";
                turn.Foreground = Brushes.Green;
            }
        }

        /// <summary>
        /// Hàm kiểm tra Win
        /// </summary>
        /// <param name="a">mảng lưu bên dưới</param>
        /// <param name="i">vị trí dòng hiện tại</param>
        /// <param name="j">vị trí cột hiện tại</param>
        /// <returns></returns>
        private int CheckWin(int[,] a, int i, int j)
        {
            const int conditionWin = 5;
            int count;
            int di, dj;
            //---------------loang theo chiều ngan----------------
            count = 1;
            //loang bên trái
            di = 0;
            dj = -1;
            count += Loang(di, dj, i, j);
            //loang bên phải
            di = 0;
            dj = 1;
            count += Loang(di, dj, i, j);

            if (count >= conditionWin)
            {
                return a[i, j];
            }

            //---------------loang theo chiều dọc----------------
            count = 1;
            //loang bên trên
            di = -1;
            dj = 0;
            count += Loang(di, dj, i, j);
            //loang bên dưới
            di = 1;
            dj = 0;
            count += Loang(di, dj, i, j);

            if (count >= conditionWin)
            {
                return a[i, j];
            }


            //---------------loang theo đường chéo chính----------------
            count = 1;
            //loang bên trên
            di = -1;
            dj = -1;
            count += Loang(di, dj, i, j);
            //loang bên dưới
            di = 1;
            dj = 1;
            count += Loang(di, dj, i, j);

            if (count >= conditionWin)
            {
                return a[i, j];
            }

            //---------------loang theo đường chéo phụ----------------
            count = 1;
            //loang bên trên
            di = -1;
            dj = 1;
            count += Loang(di, dj, i, j);
            //loang bên dưới
            di = 1;
            dj = -1;
            count += Loang(di, dj, i, j);

            if (count >= conditionWin)
            {
                return a[i, j];
            }


            return 0;
        }


        /// <summary>
        /// Hàm loang
        /// </summary>
        /// <param name="di"></param>
        /// <param name="dj"></param>
        /// <param name="i">vị trí i bắt đầu</param>
        /// <param name="j">vị trí j bắt đầu</param>
        /// <returns></returns>
        int Loang(int di, int dj, int i, int j)
        {
            int count = 0;
            int StartI = i;
            int StartJ = j;

            while (true)
            {
                j += dj;
                i += di;
                if (i > 9 || i < 0 || j > 9 || j < 0)
                {
                    break;
                }
                //nếu khác giá trị với start thì không cộng nửa
                if (a[StartI, StartJ] != a[i, j])
                {
                    break;
                }
                else
                    count++;
            }
            return count;
        }

        void Reset()
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    a[i, j] = 0;
                    Button[i, j].Content = "";
                }
            }
            Xturn = true;
            SetTurn();
            countTime = 10;
            time.Text = "10";
            SumCount = 0;
            time.Foreground = Brushes.White;
            dt.Stop();
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            Reset();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            var screen = new FileName();
            dt.Stop();
            btn_starttimeOut.Content = "Contunute";
            StreamWriter Writer = null;
            if (screen.ShowDialog() == true)
            {

                if (!screen.filename.ToString().Equals(""))
                {
                    Writer = new StreamWriter(screen.filename);
                    //lưu lại lượt đi hiện tại
                    //nếu Xturn = true lưu X không thi lưu O
                    Writer.WriteLine(Xturn ? "X" : "0");

                    //lưu SumCount
                    Writer.WriteLine($"{SumCount}");

                    //lưu lại thời gian
                    Writer.WriteLine($"{time.Text.ToString()}");

                    //lưu ma trận biểu diễn
                    for (int i = 0; i < Rows; i++)
                    {
                        for (int j = 0; j < Cols; j++)
                        {
                            Writer.Write($"{a[i, j]} ");

                            if (j == Cols - 1)
                            {
                                Writer.Write(" ");
                            }
                        }
                        Writer.WriteLine("");
                    }

                    Writer.Close();
                    MessageBox.Show("Lưu thành công!");
                }
            }


        }

        private void Load_Click(object sender, RoutedEventArgs e)
        {
            var srceen = new OpenFileDialog();

            if (srceen.ShowDialog() == true)
            {
                var FileName = srceen.FileName;

                var Reader = new StreamReader(FileName);

                //đọc dòng đầu (lượt đi hiện tại)
                var firstline = Reader.ReadLine();
                Xturn = firstline == "X";
                SetTurn();

                //đọc dòng kế tiếp SumCount biến kiểm tra hòa
                SumCount = int.Parse(Reader.ReadLine());

                //đọc lên thời gian
                countTime = int.Parse(Reader.ReadLine());
                time.Text = countTime.ToString();
                btn_starttimeOut.Content = "Continute";

                for (int i = 0; i < Rows; i++)
                {
                    var token = Reader.ReadLine().Split(new string[] { " " }, StringSplitOptions.None);
                    for (int j = 0; j < Cols; j++)
                    {
                        a[i, j] = int.Parse(token[j]);

                        if (a[i, j] == 1)
                        {
                            Button[i, j].Content = "X";
                            Button[i, j].Foreground = Brushes.Red;
                        }
                        if (a[i, j] == 2)
                        {
                            Button[i, j].Content = "O";
                            Button[i, j].Foreground = Brushes.Green;
                        }
                        if (a[i, j] == 0)
                        {
                            Button[i, j].Content = "";
                        }
                    }
                }
            }
        }

        DispatcherTimer dt = new DispatcherTimer();
        int countTime = 10;
        private void dtTicker(object sender, EventArgs e)
        {

            countTime--;
            if (countTime <= 5)
            {
                time.Foreground = Brushes.White;
            }
            else
            {
                time.Foreground = Brushes.White;
            }
            time.Text = (countTime).ToString();
            if (countTime == 0)
            {
                if (Xturn)
                {
                    MessageBox.Show("O Won!");
                    Reset();
                }
                else
                {
                    MessageBox.Show("X Won!");
                    Reset();
                }
                dt.Stop();
            }
        }

        private void TimeOut_Click(object sender, RoutedEventArgs e)
        {
            btn_starttimeOut.Content = "Start With Time Out";
            dt.Start();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            Close();
            mainWindow.Show();
        }
    }
}
