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
using static System.Net.Mime.MediaTypeNames;

namespace WpfApp1
{

    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static int countNumbers = 4;
        public int CountColumn = countNumbers;
        public int CountRow = countNumbers;
        List<List<TextBlock>> quads = new List<List<TextBlock>>();
        public TextBlock selected_quad;
        int[,] answeredField;

        public MainWindow()
        {
            InitializeComponent();



            answeredField = generateLevel(CountColumn, CountRow);
            buttons.RowDefinitions.Add(new RowDefinition());
            for (int i = 0; i < countNumbers; i++)
            {
                playfield.RowDefinitions.Add(new RowDefinition());
                playfield.ColumnDefinitions.Add(new ColumnDefinition());
                Button button = new Button();
                button.Content = (i + 1).ToString();
                button.Click += Button_Click;


                buttons.ColumnDefinitions.Add(new ColumnDefinition());
                buttons.Children.Add(button);
                Grid.SetColumn(button, i);
                Grid.SetRow(button, 0);

            }
            

            for (int i = 0; i < CountColumn; i++)
            {
                quads.Add(new List<TextBlock>());
                for (int j = 0; j < CountRow; j++)
                {
                    Border brd = new Border();
                    brd.BorderBrush = new SolidColorBrush(Colors.Black);
                    brd.BorderThickness = new Thickness(1);
                    Brush b = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                    brd.Background = b;

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
        private void checkSolution()
        {
            int finCount = CountColumn * CountRow;
            int count = 0;
            for (int i = 0; i < answeredField.GetLength(0); i++)
            {
                for (int j = 0; j < answeredField.GetLength(1); j++)
                {
                    if(answeredField[i, j] == int.Parse(quads[i][j].Text))
                    {
                        count++;
                    }
                }
            }
            if (finCount == count)
            {
                MessageBox.Show("Done");
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
                selected_quad.Text = "\n" + btn.Content.ToString();
                checkSolution();
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


                    int rand = getRandom(random, 1, CountColumn + 1, numbers.Concat(numbersRow).ToList());

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
        public List<List<int>> getSolution(List<List<TextBlock>> quads)
        {
            List<List<int>> solution = new List<List<int>>();
            solution.Add(new List<int>());
            solution.Add(new List<int>());
            solution.Add(new List<int>());
            solution.Add(new List<int>());
            for (int i = 0; i < CountColumn; i++)
            {
                int[] numbs = new int[CountColumn];
                for (int j = 0; j < CountRow; j++)
                {
                    numbs[j] = int.Parse(quads[i][j].Text);
                }
                solution[0].Add(getHighSolution(numbs));
                solution[2].Add(getHighSolution(numbs.Reverse().ToArray()));

            }

            for (int i = 0; i < CountColumn; i++)
            {
                solution.Add(new List<int>());
                int[] numbs = new int[CountColumn];
                for (int j = 0; j < CountRow; j++)
                {
                    numbs[j] = int.Parse(quads[j][i].Text);
                }
                solution[1].Add(getHighSolution(numbs));
                solution[3].Add(getHighSolution(numbs.Reverse().ToArray()));
            }

            return solution;
        }

        public int getHighSolution(int[] lineTowers)
        {
            int count = 1;
            int max = lineTowers[0];
            for (int i = 1; i < lineTowers.Length; i++)
            {
                if (max < lineTowers[i])
                {
                    count++;
                    max = lineTowers[i];
                }
            }
            return count;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string result = "";
            foreach (List<int> listInt in getSolution(quads))
            {
                foreach (int item in listInt)
                {
                    result += item.ToString() + " ";
                }
                result += "\n";
            }
            MessageBox.Show(result);
        }
    }
}
