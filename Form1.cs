using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;


namespace Lab9
{
    public partial class Form1: Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        double FSin(double x, int n)
        {
            double sin = 0;
            for (int i = 1; i <= n; i++)
            {
                sin += Math.Pow(-1, i - 1) * Math.Pow(x, 2 * i - 1) / fact(2 * i - 1);
            }
            return sin;
        }

        double FCos(double x, int n)
        {
            double cos = 0;
            for (int i = 1; i <= n; i++)
            {
                cos += Math.Pow(-1, i - 1) * Math.Pow(x, 2 * i - 2) / fact(2 * i - 2);
            }

            return cos;
        }

        double FArctg(double x, int n)
        {
            double arctg = 0;
            if (Math.Abs(x) <= 1)
            {
                for (int i = 1; i <= n; i++)
                {
                    arctg += Math.Pow(-1, i - 1) * Math.Pow(x, 2 * i - 1) / (2 * i - 1);
                }
            }
            else
            {
                arctg = (x >= 1) ? Math.PI / 2 : -Math.PI / 2;
                for (int i = 0; i < n; i++)
                {
                    arctg -= Math.Pow(-1, i) / ((2 * i + 1) * Math.Pow(x, 2 * i + 1));
                }
            }
            return arctg;
        }

        int fact(int x) => (x <= 0) ? 1 : x * fact(x - 1);

        double DrawLine_SumErr(Point startPoint, Point finishPoint, double step, int n, int r, PaintEventArgs e)
        {
            if (e != null)
            {
                e.Graphics.ScaleTransform(0.5f, 0.5f);
                e.Graphics.DrawEllipse(new Pen(Color.Black, 10), startPoint.X, startPoint.Y, r, r);
                e.Graphics.DrawEllipse(new Pen(Color.Black, 10), finishPoint.X, finishPoint.Y, r, r);
            }

            double Xx1 = startPoint.X;
            double Yy1 = startPoint.Y;
            double Xx2 = finishPoint.X;
            double Yy2 = finishPoint.Y;
            double distance = Math.Sqrt(Math.Pow(Math.Abs(Xx1 - Xx2), 2) + Math.Pow(Math.Abs(Yy1 - Yy2), 2));
            double Err = Math.Abs(Xx1 - Xx2) + Math.Abs(Yy1 - Yy2);
            double Angle = FArctg((Yy2 - Yy1) / (Xx1 - Xx2), n);

            while (distance <= Err)
            {
                Xx1 = Xx1 + step * FCos(Angle, n);
                Yy1 = Yy1 - step * FSin(Angle, n);
                distance = Math.Sqrt(Math.Pow(Math.Abs(Xx1 - Xx2), 2) + Math.Pow(Math.Abs(Yy1 - Yy2), 2));
                Err = (distance < Err) ? distance : Err;

                if (e != null) e.Graphics.DrawEllipse(new Pen(Color.Red, 5), (int)Xx1, (int)Yy1, 1, 1);
            }
            return (Err-r > 0) ? Err - r : 0;
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;

            Point startPoint = new Point(100, 100);
            Point finishPoint = new Point(900, 700);
            double error = DrawLine_SumErr(startPoint, finishPoint, 1, 4, 4, e);

            MessageBox.Show($"Погрешность: {error}");
            
            List<int> nList = new List<int>();
            List<int> rList = new List<int>();
            int n;
            for (int r = 1; r <= 20; r++)
            {
                n = 16;
                do
                {
                    n--;
                    error = DrawLine_SumErr(startPoint, finishPoint, 1, n, r, null);
                } while (error == 0);
                nList.Add(n);
                rList.Add(r);
            }

            for (int i=0; i<nList.Count; i++)
            {
                Console.WriteLine($"R: {rList[i]} N: {nList[i]}");
            }
            Graphic g = new Graphic(rList, nList);
            g.Show();
            
        }
    }
}
