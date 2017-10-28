using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _3Dfunctions
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public _3DPoint[] P;
        public Point Center;
        public MainWindow()
        {
            InitializeComponent();
            WindowState = WindowState.Maximized;
            Center.X = this.Width / 2;
            Center.Y = this.Height / 2;
        }
        public void InitializePoints(int size)
        {
            P = new _3DPoint[(2 * size) * (2 * size) * 5 * 5];
            double xCount = -size;
            double yCount = -size;

            _3DPoint.inMiddle = 60 * (int)Math.Pow(4 * (size + size), 2) / P.Length;
            int i;
            for (i = 0; i < P.Length && xCount < size; i += 1)
            {
                P[i] = new _3DPoint();
                P[i].x = (float)xCount;
                P[i].y = (float)yCount;
                P[i].z = equation(P[i].x, P[i].y);
                P[i].c = (Color)ColorConverter.ConvertFromString("White");
                yCount = Math.Round(yCount, 1);
                if (yCount == size)
                {
                    yCount = -size;
                    xCount += 0.2;
                }
                else
                {
                    yCount += 0.2;
                }
            }
        }
        public SolidColorBrush myBrush = new SolidColorBrush(Color.FromRgb(0, 0, 255));

        public void UpdateFunction()
        {
            deleteFunction();
            drawFunction(P);
        }
        public void deleteFunction()
        {
            drawingboard.Children.Clear();
        }

        public void moveFunction(_3DPoint[] P, float x, float y)
        {
            for (int i = 0; i < drawingboard.Children.Count; i++)
            {
                Canvas.SetTop(drawingboard.Children[i], Center.Y + P[i].y * 50 + y);
                Canvas.SetLeft(drawingboard.Children[i], Center.X + P[i].x * 50 + x);
                P[i].y += y;
                P[i].x += x;
            }
        }

        public void movePoint(int index, _3DPoint P, float x, float y)
        {
            Rectangle r = (Rectangle)drawingboard.Children[index];
            Canvas.SetTop(r, Center.Y + P.y * spacing + y);
            Canvas.SetLeft(r, Center.X + P.x * spacing + x);
            //P.x += x;
            //P.y += y;
        }
        public double[][] createRotatedMatrix(double radians, string type)
        {
            double[][] R = new double[3][];
            R[0] = new double[3];
            R[1] = new double[3];
            R[2] = new double[3];
            switch (type)
            {
                case "xaxis":
                    R[0][0] = 1; R[0][1] = 0; R[0][2] = 0;
                    R[1][0] = 0; R[1][1] = Math.Cos(radians); R[1][2] = -Math.Sin(radians);
                    R[2][0] = 0; R[2][1] = Math.Sin(radians); R[2][2] = Math.Cos(radians);
                    break;
                case "yaxis":
                    R[0][0] = Math.Cos(radians); R[0][1] = 0; R[0][2] = Math.Sin(radians);
                    R[1][0] = 0; R[1][1] = 1; R[1][2] = 0;
                    R[2][0] = -Math.Sin(radians); R[2][1] = 0; R[2][2] = Math.Cos(radians);
                    break;

                case "zaxis":
                    R[0][0] = Math.Cos(radians); R[0][1] = -Math.Sin(radians); R[0][2] = 0;
                    R[1][0] = Math.Sin(radians); R[1][1] = Math.Cos(radians); R[1][2] = 0;
                    R[2][0] = 0; R[2][1] = 0; R[2][2] = 1;
                    break;
            }
            return R;
        }

        public _3DPoint[] multiplyMartix(_3DPoint[] P, double[][] R)
        {
            _3DPoint[] newMatrix = new _3DPoint[P.Length];
            for (int i = 0; i < newMatrix.Length; i++)
            {
                if (P[i] != null)
                {
                    newMatrix[i] = new _3DPoint();
                    newMatrix[i].x = (float)(P[i].x * R[0][0] + P[i].y * R[0][1] + P[i].z * R[0][2]); //new x value
                    newMatrix[i].y = (float)(P[i].x * R[1][0] + P[i].y * R[1][1] + P[i].z * R[1][2]); //new y value
                    newMatrix[i].z = (float)(P[i].x * R[2][0] + P[i].y * R[2][1] + P[i].z * R[2][2]);
                    newMatrix[i].c = P[i].c;
                    newMatrix[i].proximity = P[i].proximity;
                }
            }
            return newMatrix;
        }
        public int spacing = 50;
        public void drawFunction(_3DPoint[] P)
        {
            foreach (_3DPoint Point in P)
            {

                Rectangle r = new Rectangle();
                r.Height = 2;
                r.Width = 2;
                r.Fill = myBrush;
                drawingboard.Children.Add(r);
                if (!(Center.Y + Point.y * 50 < 0 || Center.Y + Point.y * 50 > this.Height || Center.X + Point.x * 50 < 0 || Center.X + Point.x * 50 > this.Width))
                {
                    Canvas.SetTop(r, Center.Y + Point.y * spacing);
                    Canvas.SetLeft(r, Center.X + Point.x * spacing);
                }
            }
            //int zIndex = 0;
            //for (int i = 0; i < drawingboard.Children.Count; i++)
            //{
            //    UIElement child = drawingboard.Children[i] as UIElement;
            //    if (drawingboard.Children[i] is UIElement) Canvas.SetZIndex(child, zIndex++);
            //}
        }


        public float equation(float x, float y)
        {
            if (radioButton1.IsChecked == true)
            {
                float sqrt = (float)Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
                float sin = float.Parse((4 * Math.Sin(sqrt)).ToString());
                float result = sin / sqrt;
                return result;
            }
            else if (radioButton2.IsChecked == true)
            {
                //1/(15(x^2+y^2))
                return (float)(((Math.Pow(x, 2) + Math.Pow(y, 2))));
            }
            else if (radioButton3.IsChecked == true)
            {
                //cos(abs(x)+abs(y))*(abs(x)+abs(y))
                return (float)Math.Cos(Math.Abs(x) + Math.Abs(y)) + Math.Abs(x) + Math.Abs(y);
            }
            return 0;
        }





        private void button_Click(object sender, RoutedEventArgs e)
        {

            deleteFunction();
            InitializePoints(int.Parse(textBox.Text));
            drawFunction(P);
        }
        public _3DPoint[] newP;
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.W)
            {
                moveFunction(P, 0, (float)-0.1);
            }
            if (e.Key == Key.S)
            {
                moveFunction(P, 0, (float)0.1);
            }
            if (e.Key == Key.D)
            {
                moveFunction(P, (float)0.1, 0);
            }
            if (e.Key == Key.A)
            {
                moveFunction(P, (float)-0.1, 0);
            }
            if (e.Key == Key.P)
            {
                double[][] getR = createRotatedMatrix(Math.PI / 6, "xaxis");
                newP = multiplyMartix(P, getR);
                for (int i = 0; i < newP.Length; i++)
                {
                    movePoint(i, newP[i], newP[i].x - P[i].x, newP[i].y - P[i].y);
                }
                P = newP;
            }
            if (e.Key == Key.O)
            {
                double[][] getR = createRotatedMatrix(-Math.PI / 6, "yaxis");
                newP = multiplyMartix(P, getR);
                for (int i = 0; i < newP.Length; i++)
                {
                    movePoint(i, newP[i], newP[i].x - P[i].x, newP[i].y - P[i].y);
                }
                P = newP;
            }
            if (e.Key == Key.NumPad1)
            {
                double[][] getR = createRotatedMatrix(-0.017, "yaxis");
                P = multiplyMartix(P, getR);
                newP = multiplyMartix(P, getR);
                for (int i = 0; i < newP.Length; i++)
                {
                    movePoint(i, newP[i], newP[i].x - P[i].x, newP[i].y - P[i].y);
                }
                P = newP;
            }
            else if (e.Key == Key.NumPad2)
            {
                double[][] getR = createRotatedMatrix(0.017, "yaxis");
                P = multiplyMartix(P, getR);
                newP = multiplyMartix(P, getR);
                for (int i = 0; i < newP.Length; i++)
                {
                    movePoint(i, newP[i], newP[i].x - P[i].x, newP[i].y - P[i].y);
                }
                P = newP;
            }
            else if (e.Key == Key.NumPad3)
            {
                double[][] getR = createRotatedMatrix(-0.017, "xaxis");
                P = multiplyMartix(P, getR);
                newP = multiplyMartix(P, getR);
                for (int i = 0; i < newP.Length; i++)
                {
                    movePoint(i, newP[i], newP[i].x - P[i].x, newP[i].y - P[i].y);
                }
                P = newP;
            }
            else if (e.Key == Key.NumPad6)
            {
                double[][] getR = createRotatedMatrix(0.017, "xaxis");
                P = multiplyMartix(P, getR);
                newP = multiplyMartix(P, getR);
                for (int i = 0; i < newP.Length; i++)
                {
                    movePoint(i, newP[i], newP[i].x - P[i].x, newP[i].y - P[i].y);
                }
                P = newP;
            }
            else if (e.Key == Key.NumPad9)
            {
                double[][] getR = createRotatedMatrix(0.017, "xaxis");
                P = multiplyMartix(P, getR);
                newP = multiplyMartix(P, getR);
                for (int i = 0; i < newP.Length; i++)
                {
                    movePoint(i, newP[i], newP[i].x - P[i].x, newP[i].y - P[i].y);
                }
                P = newP;
            }
            else if (e.Key == Key.NumPad4)
            {
                double[][] getR = createRotatedMatrix(0.017, "zaxis");
                P = multiplyMartix(P, getR);
                newP = multiplyMartix(P, getR);
                for (int i = 0; i < newP.Length; i++)
                {
                    movePoint(i, newP[i], newP[i].x - P[i].x, newP[i].y - P[i].y);
                }
                P = newP;
            }
            else if (e.Key == Key.NumPad5)
            {
                double[][] getR = createRotatedMatrix(-0.017, "zaxis");
                P = multiplyMartix(P, getR);
                newP = multiplyMartix(P, getR);
                for (int i = 0; i < newP.Length; i++)
                {
                    movePoint(i, newP[i], newP[i].x - P[i].x, newP[i].y - P[i].y);
                }
                P = newP;
            }
            else
            {


                base.OnKeyDown(e);
            }
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            deleteFunction();
            InitializePoints(int.Parse(textBox.Text));
            textBox.IsEnabled = false;
            if (form.IsLoaded)
                drawFunction(P);

        }

        private void form_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (spacing + e.Delta / 10 > 10)
            {
                spacing += e.Delta / 10;
                for (int i = 0; i < P.Length; i++)
                {
                    movePoint(i, P[i], P[i].x + e.Delta / 10 - P[i].x, P[i].y + e.Delta / 10 - P[i].y);
                }
            }
        }
    }
}
