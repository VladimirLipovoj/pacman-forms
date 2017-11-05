using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace PacmanForms
{
    public partial class Form1 : Form
    {
        //ushort[,] map = {
        //    { 0, 0, 0, 0, 0, 0, 0 },
        //    { 0, 0, 1, 1, 1, 0, 0 },
        //    { 0, 1, 1, 1, 1, 1, 0 },
        //    { 0, 0, 0, 1, 0, 0, 0 },
        //    { 0, 1, 1, 0, 1, 1, 0 },
        //    { 0, 0, 1, 0, 1, 0, 0 },
        //    { 0, 0, 0, 0, 0, 0, 0 }
        //};
        Thread t;

        ushort[,] map;

        int size25 = 25,
            size49 = 49,
            size81 = 81,
            size121 = 121,
            size169 = 169,
            size225 = 225,
            size289 = 289,
            size361 = 361,
            size441 = 441,
            size529 = 529,
            size625 = 625;

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            t.Abort();
        }

        int side = 0;

        SolidBrush sb0 = new SolidBrush(Color.Firebrick);
        SolidBrush sb1 = new SolidBrush(Color.ForestGreen);
        Random rnd = new Random();
        Image wall = new Bitmap("wall.png");
        Image floor = new Bitmap("floor.png");

        public Form1()
        {
            InitializeComponent();
            DoubleBuffered = true;


            side = (int)Math.Sqrt(size625);
            map = new ushort[side, side];

            t = new Thread(tick);
            t.Start();
        }

        private void tick()
        {
            while (true)
            {
                
                map[rnd.Next(0, side), rnd.Next(0, side)] = (ushort)rnd.Next(2);
                Invalidate();
                Thread.Sleep(2);
            }
        }
        

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(Color.Black);

            int offsetCenter = -Height / 2 + (Width - side) / 2 + 8;

            //int offsetCenter = (Width/side)*8
            
            for (int j = 0; j < Height / 8; j++)
            {
                for (int i = 0; i < Height / 8; i++)
                {
                    double relX = i / (Height / 8.0);
                    double relY = j / (Height / 8.0);
                    int x =  (int)(relX * side);
                    int y = (int)(relY * side);

                    if (map[x, y] == 0)
                        g.DrawImageUnscaled(wall, new Point(offsetCenter + i * 8, j * 8));
                    else
                        g.DrawImageUnscaled(floor, new Point(offsetCenter + i * 8, j * 8));

                    
                }
            }
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            Invalidate();
        }

    }
}
