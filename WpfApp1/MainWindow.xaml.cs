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
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int CountColumn = 4;
        public int CountRow = 4;
        List<List<TextBlock>> quads = new List<List<TextBlock>>();
        public TextBlock selected_quad;

        public MainWindow()
        {
            InitializeComponent();

            int[,] answeredField = generateLevel(CountColumn, CountRow);


            for (int i = 0; i < CountColumn; i++)
            {
                quads.Add(new List<TextBlock>());
                for (int j = 0; j < CountRow; j++)
                {
                    Border brd = new Border();
                    brd.BorderBrush = new SolidColorBrush(Colors.Black);
                    brd.BorderThickness = new Thickness(1);

                    playfield.Children.Add(brd);
                    Grid.SetColumn(brd, i);
                    Grid.SetRow(brd, j);

                    TextBlock tb = new TextBlock();
                    tb.Text = answeredField[i, j].ToString();
                    playfield.Children.Add(tb);

                    Grid.SetColumn(tb, i);
                    Grid.SetRow(tb, j);

                    tb.TextAlignment = TextAlignment.Center;
                    tb.FontSize = 18;

                    tb.MouseUp += quad_MouseUp;

                    quads[i].Add(tb);
                }
            }

        }
        private void quad_MouseUp(object sender, MouseButtonEventArgs e)
        {
            quads_clear();
            TextBlock t = sender as TextBlock;
            Brush b = new SolidColorBrush(Color.FromRgb(253, 255, 150));
            t.Background = b;
            selected_quad = t;
        }
        private void quads_clear()
        {
            foreach (var row in quads)
            {
                foreach (var item in row)
                {
                    item.Background = null;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (selected_quad != null)
            {
                Button btn = sender as Button;
                selected_quad.Text = "\r\n" + btn.Content.ToString();
            }

        }

        private int[,] generateLevel(int countColumn, int countRow)
        {
            List<int> numbers = new List<int>();
            List<int> numbersRow = new List<int>();

            Random random = new Random();
            int[,] ansField = new int[countColumn, countRow];

            for (int i = 0; i < countColumn; i++)
            {
                numbers.Clear();

                for (int j = 0; j < countRow; j++)
                {
                    numbersRow.Clear();
                    for (int k = 0; k < countColumn; k++)
                    {
                        numbersRow.Add(ansField[k, j]);
                    }


                    int rand = getRandom(random, 1, 5, numbers.Concat(numbersRow).ToList());

                    ansField[i, j] = rand;
                    numbers.Add(rand);

                }
            }
            
            return ansField;

        }
        
        public int getRandom(Random rand, int min, int max, List<int> exclude)
        {
            int result = 0;
            do
            {
                result = rand.Next(min, max);
            }
            while (exclude.Contains(result));

            return result;
        }
    }
}
