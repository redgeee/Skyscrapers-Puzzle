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
using System.Windows.Media.Effects;
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
        public int cumnumbers { set { countNumbers = value; CountColumn = value; CountRow = value; } }
        public MainWindow(int difficult)
        {
                cumnumbers = difficult;
            
            InitializeComponent();


            answeredField = generateLevel(CountColumn, CountRow);
            buttons.RowDefinitions.Add(new RowDefinition());
            Button button_clear = new Button();
            button_clear.Content = "0";
            button_clear.Click += Button_Clear_Click;
            buttons.ColumnDefinitions.Add(new ColumnDefinition());
            buttons.Children.Add(button_clear);
            Grid.SetColumn(button_clear, 0);
            Grid.SetRow(button_clear, 0);

            for (int i = 0; i < countNumbers; i++)
            {
                playfield.RowDefinitions.Add(new RowDefinition());
                playfield.ColumnDefinitions.Add(new ColumnDefinition());
                Button button = new Button();
                button.Content = (i + 1).ToString();
                button.Click += Button_Click;


                buttons.ColumnDefinitions.Add(new ColumnDefinition());
                buttons.Children.Add(button);
                Grid.SetColumn(button, i + 1);
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
                    playfield.Children.Add(tb);

                    Grid.SetColumn(tb, i);
                    Grid.SetRow(tb, j);

                    tb.TextAlignment = TextAlignment.Center;
                    tb.FontSize = 18;

                    tb.MouseUp += quad_MouseUp;

                    quads[i].Add(tb);
                }
            }

            List<List<int>> solv = getSolution(answeredField);
            DropShadowEffect shadow = new DropShadowEffect();
            Color clr = (Color)ColorConverter.ConvertFromString("#FF3333");
            shadow.Color = clr;
            shadow.Direction = 320;
            shadow.ShadowDepth = 5;
            Color color = (Color)ColorConverter.ConvertFromString("#3333FF");
            SolidColorBrush br = new SolidColorBrush();
            br.Color = color;

            up_prompt.RowDefinitions.Add(new RowDefinition());
            for (int i = 0; i < countNumbers; i++)
            {
                up_prompt.ColumnDefinitions.Add(new ColumnDefinition());
                TextBlock tb = new TextBlock();
                
                //tb.Foreground = br;
                tb.Text = solv[0][i].ToString();
                tb.FontSize = 30;
                tb.TextAlignment = TextAlignment.Center;
                tb.Effect = shadow;
                up_prompt.Children.Add(tb);
                Grid.SetColumn(tb, i);
                Grid.SetRow(tb, 0);
            }
            down_prompt.RowDefinitions.Add(new RowDefinition());
            for (int i = 0; i < countNumbers; i++)
            {
                down_prompt.ColumnDefinitions.Add(new ColumnDefinition());
                TextBlock tb = new TextBlock();
                tb.Text = solv[2][i].ToString();
                tb.FontSize = 30;
                tb.TextAlignment = TextAlignment.Center;
                tb.Effect = shadow;
                down_prompt.Children.Add(tb);
                Grid.SetColumn(tb, i);
                Grid.SetRow(tb, 0);
            }
            left_prompt.ColumnDefinitions.Add(new ColumnDefinition());
            for (int i = 0; i < countNumbers; i++)
            {
                
                left_prompt.RowDefinitions.Add(new RowDefinition());
                TextBlock tb = new TextBlock();
                tb.Text = (countNumbers > 4 ? "" : "\n") + solv[1][i].ToString();
                tb.FontSize = 30;
                tb.TextAlignment = TextAlignment.Center;
                tb.Effect = shadow;
                left_prompt.Children.Add(tb);
                Grid.SetColumn(tb, 0);
                Grid.SetRow(tb, i);
            }
            right_prompt.ColumnDefinitions.Add(new ColumnDefinition());
            for (int i = 0; i < countNumbers; i++)
            {
                right_prompt.RowDefinitions.Add(new RowDefinition());
                TextBlock tb = new TextBlock();
                tb.Text = (countNumbers > 4 ? "" : "\n") + solv[3][i].ToString();
                tb.FontSize = 30;
                tb.TextAlignment = TextAlignment.Center;
                tb.Effect = shadow;
                right_prompt.Children.Add(tb);
                Grid.SetColumn(tb, 0);
                Grid.SetRow(tb, i);
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
                    string sr = quads[i][j].Text;
                    if (answeredField[i, j] == (sr == "" ? -1 : int.Parse(sr)))
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
                selected_quad.Text = "\n" + "\n" + btn.Content.ToString();
                checkSolution();
            }

        }

        private void Button_Clear_Click(object sender, RoutedEventArgs e)
        {
            if (selected_quad != null)
            {
               
                selected_quad.Text = null;
                
            }
        }

        private int[,] generateLevel(int countColumn, int countRow)
        {
            while (true)
            {
                try
                {

                    int[,] matrix = new int[countColumn, countRow];

                    Random rnd = new Random();

                    for (int i = 0; i < matrix.GetLength(0); i++)
                    {
                        List<int> rowValues = new List<int>();
                        for (int k = 0; k < CountColumn; k++)
                        {
                            rowValues.Add(k + 1);
                        }
                        for (int j = 0; j < matrix.GetLength(1); j++)
                        {

                            List<int> columnValues = new List<int>();
                            for (int k = 0; k < matrix.GetLength(0); k++)
                            {
                                columnValues.Add(matrix[k, j]);
                            }

                            List<int> resultValues = rowValues.ToList();
                            foreach (int item in columnValues)
                            {
                                resultValues.Remove(item);
                            }

                            int newValue = resultValues[rnd.Next(resultValues.Count)];
                            matrix[i, j] = newValue;
                            rowValues.Remove(newValue);

                        }
                    }

                    return matrix;

                }
                catch (Exception)
                {
                }
            }
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
        public List<List<int>> getSolution(int[,] quads)
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
                    numbs[j] = int.Parse(quads[i,j].ToString());
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
                    numbs[j] = int.Parse(quads[j, i].ToString());
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

        private void back_btn_Click(object sender, RoutedEventArgs e)
        {
            Window1 win2 = new Window1();
            this.Close();
            win2.Show();
        }
    }
}
