using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace test
{
    public partial class MainWindow : Window
    {
        private readonly double[][,] option =
        {
            new [,]{ { 0, 0.2, 0.4, 0.6, 0.8, 1 }, { 3, 6, 3, 6, 4, 3 } },
            new[,]{ { 0, 0.2, 0.4, 0.6, 0.8, 1 }, { 5, 5, 4, 4, 6, 6 } },
            new[,]{ { 3, 3.2, 3.4, 3.6, 3.8, 4 }, { 2, 3, 3, 3, 2, 4 } },
            new[,]{ { 3, 3.2, 3.4, 3.6, 3.8, 4 }, { 6, 2, 6, 4, 3, 4 } },
            new[,]{ { 5, 5.2, 5.4, 5.6, 5.8, 6 }, { 2, 4, 4, 3, 3, 3 } },
            new[,]{ { 4, 4.2, 4.4, 4.6, 4.8, 5 }, { 4, 3, 6, 6, 4, 4 } },
            new[,]{ { 1, 1.2, 1.4, 1.6, 1.8, 2 }, { 2, 6, 4, 4, 2, 5 } },
            new[,]{ { 5, 5.2, 5.4, 5.6, 5.8, 6 }, { 3, 2, 5, 2, 2, 3 } },
            new[,]{ { 2, 2.2, 2.4, 2.6, 2.8, 3 }, { 4, 2, 4, 2, 5, 2 } },
            new[,]{ { 0, 0.2, 0.4, 0.6, 0.8, 1 }, { 6, 3, 2, 6, 2, 5 } }
        };
        bool del = false;
        
        public MainWindow()
        {
            InitializeComponent();

            foreach (UIElement el in GroupButton.Children)
            {
                if (el is Button)
                {
                    ((Button)el).Click += ButtonClick;
                }
            }

            Draw();
		}

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            string name = (sender as Button).Name;
        
            int i = Convert.ToInt32(name.Substring(name.Length -1)) - 1;
            SolveFunction(option[i]);
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        public void Draw()
        {
            for (int j = 0; j < 2; j++) 
            {
                for (int k = 0; k <= 12; k++)
                {
                    if (j == 0)
                    {
                        Line L = new Line()
                        {
                            X1 = k * 50,
                            Y1 = 0,
                            X2 = k * 50,
                            Y2 = canvas.Height,
                            Stroke = Brushes.Gray 
                        };
                        if (k % 2 == 0)
                            L.StrokeThickness = 3;
                        else
                            L.StrokeThickness = 1;
                        canvas.Children.Add(L); 
                    }
                   
                    for (int K = 0; K <= 12 ; K++) 
                    {
                        if (j == 1)
                        {
                            Line L = new Line
                            {
                                Y1 = K * 50,
                                X1 = 0,
                                Y2 = K * 50,
                                X2 = canvas.Width,
                                Stroke = Brushes.Gray, 
                                StrokeThickness = 3 
                            };
                            canvas.Children.Add(L); 
                        }
                    }
                }
            }
        }

        public void SolveFunction(double[,] function)
        {
            double S0 = 0, S1 = 0, S2 = 0, S3 = 0, S4 = 0, b0 = 0, b1 = 0, b2 = 0, a1, a0, c1, c2, c3, c4, c5, c6;
            for (int i = 0; i < 6; i++)
            {
                S0 += Math.Pow(function[0, i], 0); 
                S1 += Math.Pow(function[0, i], 1);
                S2 += Math.Pow(function[0, i], 2);
                b0 += function[1, i]; 
                b1 += function[1, i] * function[0, i];
                S3 += Math.Pow(function[0, i], 3);
                S4 += Math.Pow(function[0, i], 4);
                b2 += function[0, i] * function[0, i] * function[1, i]; 
            }
            
            a0 = ((b1 * S1) - (S2 * b0)) / ((S1 * S1) - (S2 * S0));
            a1 = (b0 - S0 * a0) / S1;
            c1 = S1 - S0 * S3 / S2;
            c2 = S2 - S1 * S3 / S2;
            c3 = b1 - b0 * S3 / S2;
            c4 = S2 - S0 * S4 / S2;
            c5 = S3 - S1 * S4 / S2;
            c6 = b2 - b0 * S4 / S2;


            DrawPoints(function);
            DrawLine(a0, a1, function);
            DrawParabola(b0, S0, S1, S2,c1, c2, c3, c4, c5, c6, function);
            OutValue(function);
        }

        public void DrawPoints(double[,] function)
        {
            for (int i = 0; i < 6; i++) 
            {
                int r = 5;  
                Ellipse E = new Ellipse
                {
                    Fill = Brushes.Red, 
                    Width = r * 2,
                    Height = r * 2,
                    Margin = new Thickness(i * 100 + 45, canvas.Height - function[1, i] * 50 - r, 0, 0) 
                };
                if (del == true)
                {
                    canvas.Children.Clear();
                    Draw();
                    del = false;
                }

                canvas.Children.Add(E);
            }
            del = true;
        }

        public void DrawLine(double a0, double a1, double[,] function)
        {
            List<Point> P = new List<Point>();
            double x = 50, y;
            while (x <= 550)
            {
                y = (a0 * 50) + (a1 * (function[0, 0] + ((x - 50) / 500)) * 50);
                P.Add(new Point(x, y));
                x += 0.01; 
            }
            Polyline PL = new Polyline
            {
                Stroke = Brushes.Blue,
                StrokeThickness = 2
            };
            PointCollection PC = new PointCollection();

            foreach (Point p in P)
            {
                PC.Add(new Point(p.X, canvas.Height - p.Y)); 
            }
            PL.Points = PC;

            LabelText(a0, a1);

            canvas.Children.Add(PL); 
        }

        public void LabelText(double a0, double a1)
        {
            l_tb.Text = (a0 != 0 && a1 != 0) ? $"y={a0:f3} + ({a1:f3})*x" : (a0 != 0 && a1 == 0) ? $"y={a0:f3}" : $"y={a1:f3}*x ";
        }

        public void DrawParabola(double b0, double S0, double S1, double S2, double c1, double c2, double c3, double c4,
                                 double c5, double c6, double[,] function)
        {
            double a0, a1, a2;
            a0 = (c6 * c2 - c5 * c3) / (c2 * c4 - c5 * c1);
            a1 = (c3 - c1 * a0) / (c2);
            a2 = (b0 - S1 * a1 - S0 * a0) / S2;
           
          

            List<Point> P1 = new List<Point>(); 
            double x1 = 50, y1;  
            while (x1 <= 550) 
            {
                double t = function[0, 0] + (x1 - 50) / 500;
                y1 = a0 * 50 + a1 * (t) * 50 + a2 * (t) * (t) * 50;
                P1.Add(new Point(x1, y1));
                x1 += 0.01;
            }
            
            Polyline PL1 = new Polyline();
            PointCollection PC1 = new PointCollection();
            foreach (Point p1 in P1)  // для каждой из
            {
                PC1.Add(new Point(p1.X, canvas.Height - p1.Y)); 
            }
            PL1.Points = PC1; 
            PL1.Stroke = Brushes.Red; 
            
            PL1.StrokeThickness = 2; 
            canvas.Children.Add(PL1);

            SquareText(a0, a1, a2);
            
        }

        public void SquareText(double a0, double a1, double a2)
        {
            if (a0 != 0 && a1 != 0 && a2 != 0)
            {
                p_tb.Text = $"y={a0:f3} + ({a1:f3})*x + ({a2:f3})*x^2)";
            }

            else if (a0 != 0 && a1 != 0 && a2 == 0)
            {
                p_tb.Text = $"y={a0:f3} + ({a1:f3}*x";
            }

            else if (a1 != 0 && a1 == 0 && a2 != 0)
            {
                p_tb.Text = $"y={a0:f3} + ({a2:f3})*x^2)";
            }
        }

        public void OutValue(double[,] function)
        {
            
            OutX1.Text = function[0, 0].ToString();
            OutX2.Text = function[0, 1].ToString();
            OutX3.Text = function[0, 2].ToString();
            OutX4.Text = function[0, 3].ToString();
            OutX5.Text = function[0, 4].ToString();
            OutX6.Text = function[0, 5].ToString();

            OutY1.Text = function[1, 0].ToString();
            OutY2.Text = function[1, 1].ToString();
            OutY3.Text = function[1, 2].ToString();
            OutY4.Text = function[1, 3].ToString();
            OutY5.Text = function[1, 4].ToString();
            OutY6.Text = function[1, 5].ToString();
        }

        public void InputValue()
        {

            try
            {
               
                double inpX1 = Convert.ToDouble(X1.Text.Replace(".", ","));
                double inpX2 = Convert.ToDouble(X2.Text.Replace(".", ","));
                double inpX3 = Convert.ToDouble(X3.Text.Replace(".", ","));
                double inpX4 = Convert.ToDouble(X4.Text.Replace(".", ","));
                double inpX5 = Convert.ToDouble(X5.Text.Replace(".", ","));
                double inpX6 = Convert.ToDouble(X6.Text.Replace(".", ","));
                double inpY1 = Convert.ToDouble(Y1.Text.Replace(".", ","));
                double inpY2 = Convert.ToDouble(Y2.Text.Replace(".", ","));
                double inpY3 = Convert.ToDouble(Y3.Text.Replace(".", ","));
                double inpY4 = Convert.ToDouble(Y4.Text.Replace(".", ","));
                double inpY5 = Convert.ToDouble(Y5.Text.Replace(".", ","));
                double inpY6 = Convert.ToDouble(Y6.Text.Replace(".", ","));

                double[,] inputArray = {
                    {inpX1, inpX2, inpX3, inpX4, inpX5, inpX6 },
                    {inpY1, inpY2, inpY3, inpY4, inpY5, inpY6}};
                ClearTextBox();
                SolveFunction(function: inputArray);
            }
            catch
            {
               MessageBox.Show("Ошибка заполнения таблицы");
            }
        }

        public void ClearTextBox()
        {
            TextBox[,] myarray = {
                {X1, X2, X3, X4, X5, X6 },
                {Y1, Y2, Y3, Y4, Y5, Y6,}
            };

            foreach (TextBox elem in myarray)
            {
                elem.Text = "";
            }
        }

        private void Calc_Button_Click(object sender, RoutedEventArgs e) => InputValue();

        private void Close(object sender, MouseButtonEventArgs e)
        {
            try 
            {
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            
        }

        private void Minimaze(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
            try
            {
                this.WindowState = WindowState.Minimized;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        
    }
}
