using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _3D
{
    public partial class Form1 : Form
    {
        Pen myPen = new Pen(Color.MidnightBlue, 2);
        Brush myBrush = new SolidBrush(Color.MidnightBlue);
        Pen polygonPen = new Pen(Color.SlateBlue, 2);
        Pen turn1Pen = new Pen(Color.Tomato, 2);
        Pen turn2Pen = new Pen(Color.Plum, 2);
        Pen turn3Pen = new Pen(Color.DarkTurquoise, 2);

        float x0;
        float y0;
        float interval;
        float lyambda=15;

        float[,] globalMatrix = new float[12, 6];

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        float[,] multiplier (float[,] a, float[,] b)
        {
            float[,] buff = new float[12,4];

            for ( int i =0; i < 12; i++)
            {
                for ( int j =0; j < 4; j++)
                {
                    buff[i, j] = 0;
                    for ( int k = 0; k < 4; k++)
                    {
                        buff[i,j] += a[i, k] * b[k, j];
                    }
                }
            }

            return buff;           
        }

        void drawAxises(Graphics g, float x0, float y0)
        {
            g.Transform = new System.Drawing.Drawing2D.Matrix(1, 0, 0, 1, x0, y0);

            g.DrawLine(myPen, 0, 0, x0, 0);
            g.DrawLine(myPen, 0, -y0, 0, 0);
            g.DrawLine(myPen, 0, 0, -x0 + 50, y0 - 50);

            PointF[] Y = new PointF[]
            {
                 new PointF(0, -y0),
                 new PointF(5, -y0 + 5),
                 new PointF(-5, -y0 + 5)
            };

            PointF[] X = new PointF[]
            {
                 new PointF(x0, 0),
                 new PointF(x0 - 5, -5),
                 new PointF(x0 - 5, 5)
            };

            PointF[] Z = new PointF[]
            {
                 new PointF(-x0+48,y0-54),
                 new PointF(-x0+54,y0-48),
                 new PointF(-x0+45,y0-45)
            };

            string y = "y";
            string x = "x";
            string z = "z";

            PointF forY = new PointF(-20, -y0);
            PointF forX = new PointF(x0 - 15, 5);
            PointF forZ = new PointF(-x0 + 50, y0 - 45);

            Font f = new Font(this.Font, FontStyle.Bold);

            g.DrawString(y, f, myBrush, forY);
            g.DrawString(x, f, myBrush, forX);
            g.DrawString(z, f, myBrush, forZ);

            g.FillPolygon(myBrush, Y);
            g.FillPolygon(myBrush, X);
            g.FillPolygon(myBrush, Z);
        }

        void drawPolygon(Graphics g, float [,] points, Pen polygonPen)
        {
            float interval = ClientSize.Width / 20;

            PointF[] global =
            {
                new PointF(points[0,0], points[0,1]),
                new PointF(points[1,0], points[1,1]),
                new PointF(points[7,0], points[7,1]),
                new PointF(points[1,0], points[1,1]),
                new PointF(points[2,0], points[2,1]),
                new PointF(points[8,0], points[8,1]),
                new PointF(points[2,0], points[2,1]),
                new PointF(points[3,0], points[3,1]),
                new PointF(points[9,0], points[9,1]),
                new PointF(points[3,0], points[3,1]),
                new PointF(points[4,0], points[4,1]),
                new PointF(points[10,0], points[10,1]),
                new PointF(points[4,0], points[4,1]),
                new PointF(points[5,0], points[5,1]),
                new PointF(points[11,0], points[11,1]),
                new PointF(points[5,0], points[5,1]),
                new PointF(points[0,0], points[0,1]),
                new PointF(points[6,0], points[6,1]),
                new PointF(points[7,0], points[7,1]),
                new PointF(points[8,0], points[8,1]),
                new PointF(points[9,0], points[9,1]),
                new PointF(points[10,0], points[10,1]),
                new PointF(points[11,0], points[11,1]),
                new PointF(points[6,0], points[6,1]),
            };

            g.DrawPolygon(polygonPen, global);
        }

        void turnX(Graphics g, float alpha)
        {
            float[,] rotate =
           {
                { 1, 0, 0, 0 },
                { 0, (float)Math.Cos(alpha), (float)Math.Sin(alpha), 0 },
                { 0, -(float)Math.Sin(alpha), (float)Math.Cos(alpha), 0 },
                {0, 0, 0, 1 },
            };

            globalMatrix = multiplier(globalMatrix, rotate);          
            
            drawPolygon(g, globalMatrix, turn1Pen);
        }

        void turnY(Graphics g, float alpha)
        {
            float[,] rotate =
           {
                { (float)Math.Cos(alpha), 0, -(float)Math.Sin(alpha), 0 },
                { 0, 1, 0, 0 },
                { (float)Math.Sin(alpha), 0, (float)Math.Cos(alpha), 0 },
                {0, 0, 0, 1 },
            };

            globalMatrix = multiplier(globalMatrix, rotate);

            drawPolygon(g, globalMatrix, turn2Pen);
        }

        void turnZ(Graphics g, float alpha)
        {
            float[,] rotate =
           {
                { (float)Math.Cos(alpha), (float)Math.Sin(alpha), 0, 0 },
                { -(float)Math.Sin(alpha), (float)Math.Cos(alpha), 0, 0 },
                { 0, 0, 0, 0 },
                {0, 0, 0, 1 },
            };

            globalMatrix = multiplier(globalMatrix, rotate);

            drawPolygon(g, globalMatrix, turn3Pen);
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Refresh();
            Graphics g = this.CreateGraphics();

            x0 = ClientSize.Width / 2;
            y0 = ClientSize.Height / 2;

            interval = ClientSize.Width / 20;

             float[,] initialState = {
                { -2 * interval, -2 * interval, lyambda, 1 },
                { -2 * interval, 2 * interval, lyambda, 1 },
                { 2 * interval, 2 * interval, lyambda, 1 },
                { 3 * interval, 3 * interval, lyambda, 1 },
                { 4 * interval, 0, lyambda, 1 },
                { 2 * interval, -2 * interval, lyambda, 1 },
                { -2 * interval, -2 * interval, -lyambda, 1 },
                { -2 * interval, 2 * interval, -lyambda, 1 },
                { 2 * interval, 2 * interval, -lyambda, 1 },
                { 3 * interval, 3 * interval, -lyambda, 1 },
                { 4 * interval, 0, -lyambda, 1 },
                { 2 * interval, -2 * interval, -lyambda, 1 },
            };

            globalMatrix = initialState;

            drawAxises(g, x0, y0);
            drawPolygon(g, initialState, polygonPen);

            button2.Enabled = true;
            button3.Enabled = true;
            button4.Enabled = true;
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            Refresh();
            Graphics g = this.CreateGraphics();

            float alpha = 70;

            drawAxises(g, x0, y0);
            turnX(g, alpha);
                  
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            Refresh();
            Graphics g = this.CreateGraphics();

            float alpha = 70;           

            drawAxises(g, x0, y0);
            turnY(g, alpha);
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            Refresh();
            Graphics g = this.CreateGraphics();

            float alpha = 70;

            drawAxises(g, x0, y0);
            turnZ(g, alpha);
        }
    }
}
