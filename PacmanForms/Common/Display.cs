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
    public partial class Display : Form
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

        int side = 0;

        SolidBrush sb0 = new SolidBrush(Color.Firebrick);

        SolidBrush sb1 = new SolidBrush(Color.ForestGreen);
        Random rnd = new Random();
        Image wall = new Bitmap("wall.png");
        Image floor = new Bitmap("floor.png");
        Image pacman = new Bitmap("pacman.png");

        int tileSize = 25;


        public Display() {
            InitializeComponent();
            DoubleBuffered = true;


            side = (int)Math.Sqrt(size625);
            map = new ushort[side, side];

            for(int i=0; i<1000; i++)
                map[rnd.Next(0, side), rnd.Next(0, side)] = (ushort)rnd.Next(2);

            map[side / 2, side / 2] = 2;

            t = new Thread(tick);
            t.Start();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Right)
                tileSize += 1;
            else if (e.KeyCode == Keys.Left)
                tileSize -= 1;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            t.Abort();
        }

        private void Form1_Paint(object sender, PaintEventArgs e) {
            Graphics g = e.Graphics;
            g.Clear(Color.Black);

            //List<Point> pacmanCords = new List<Point>();

            int offsetCenter = -Height / 2 + (Width - side) / 2 + tileSize;

            //int offsetCenter = (Width/side)*8

            for (int j = 0; j < Height / tileSize; j++) {
                for (int i = 0; i < Height / tileSize; i++) {
                    double relX = i / (Height / (double)tileSize);
                    double relY = j / (Height / (double)tileSize);
                    int x = (int)(relX * side);
                    int y = (int)(relY * side);

                    //Layer 1
                    if (map[x, y] == 0)
                        g.DrawImage(wall, offsetCenter + i * tileSize, j * tileSize, tileSize, tileSize);
                    else
                        g.DrawImage(floor, offsetCenter + i * tileSize, j * tileSize, tileSize, tileSize);
                    ////Layer 2
                    if (map[x, y] == 2)
                        g.DrawImage(pacman, offsetCenter + i * tileSize, j * tileSize, tileSize, tileSize);
                }
            }
            //Point p = pacmanCords.ToArray()[0];
            //g.DrawImage(pacman, p);
        }

        private void Form1_SizeChanged(object sender, EventArgs e) {
            Invalidate();
        }

        private void tick()
        {
            while (true)
            {
                label1.Text = $"Size = {tileSize}";
                Invalidate();
                Thread.Sleep(2);
            }
        }
    }
}
