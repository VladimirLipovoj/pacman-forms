using PacmanForms.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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
        //Map good
        ushort[,] map;
        //ushort[,] mapPacman;

        public enum tiles
        {
            floor,
            wall
        }

        public enum entities
        {
            air,
            pacman
        }

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

        public static int side = 0;
        int addSize = 0;

        SolidBrush sb0 = new SolidBrush(Color.Firebrick);

        SolidBrush sb1 = new SolidBrush(Color.ForestGreen);
        Random rnd = new Random();

        Image wall = new Bitmap("../../Assets/wall.png");
        Image floor = new Bitmap("../../Assets/floor.png");

        Pacman pacman;

        int tileSize = 25;

        private void Display_MouseMove(object sender, MouseEventArgs e)
        {
           
        }

        private void Display_MouseClick(object sender, MouseEventArgs e)
        {
            
        }


        private void Display_MouseDown(object sender, MouseEventArgs e)
        {
                
        }

        private void Display_KeyDown(object sender, KeyEventArgs e)
        {
            //Pacman only
            if (!pacman.moving) {
                switch (e.KeyCode)
                {
                    case Keys.Up:
                        pacman.direction = (int)Pacman.directions.up;
                        break;
                    case Keys.Down:
                        pacman.direction = (int)Pacman.directions.down;
                        break;
                    case Keys.Left:
                        pacman.direction = (int)Pacman.directions.left;
                        break;
                    case Keys.Right:
                        pacman.direction = (int)Pacman.directions.right;
                        break;
                }
            }

            //Main
            switch (e.KeyCode)
            {
                case Keys.Add:
                    addSize++;
                    break;
                case Keys.Subtract:
                    addSize--;
                    break;
            }

        }
        private void Display_SizeChanged(object sender, EventArgs e)
        {
            Invalidate();
        }

        private void Display_FormClosed(object sender, FormClosedEventArgs e)
        {
            t.Abort();
        }


        public Display() {
            InitializeComponent();
            DoubleBuffered = true;


            side = (int)Math.Sqrt(size625);
            map = new ushort[side, side];
            //mapPacman = new ushort[side, side];

            for(int i=0; i<120; i++)
            {
                int rndX = rnd.Next(0, side);
                int rndY = rnd.Next(0, side);

                if(rndX == side/2 && rndY == side/2)
                    map[rndX, rndY] = (int)tiles.floor;
                else
                    map[rndX, rndY] = (int)tiles.wall;
            }


            tileSize = -Height / 2 - Width / 2 + tileSize;
            tileSize /= 40;
            tileSize = -tileSize;
            tileSize += addSize;

            pacman = new Pacman((side / 2) * tileSize, (side / 2) * tileSize);
            //mapPacman[side / 2, side / 2] = 1;

            t = new Thread(tick);
            t.Start();
        }

        private void tick()
        {
            //                               fpc
            double timePerTick = 1000000000 / 60;
            double delta = 0;

            long nano = 10000L * Stopwatch.GetTimestamp();
            nano /= TimeSpan.TicksPerMillisecond;
            nano *= 100L;

            long now,
                 timer = 0,
                 lastTime = nano;

            int ticks = 0;

            while (true)
            {

                nano = 10000L * Stopwatch.GetTimestamp();
                nano /= TimeSpan.TicksPerMillisecond;
                nano *= 100L;

                now = nano;
                delta += (now - lastTime) / timePerTick;
                timer = now - lastTime;
                lastTime = now;


                if (delta >= 1)
                {
                    pacman.tick(tileSize);
                    Invalidate();

                    ticks++;
                    delta--;
                }

                if (timer >= 1000000000)
                {
                    ticks = 0;
                    timer = 0;
                }

            }
        }

        private void Display_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(Color.Black);

            //List<Point> pacmanCords = new List<Point>();

            //int offsetCenter = -Height / 2 + Width / 2 + tileSize;

            tileSize = -Height / 2 - Width / 2 + tileSize;
            tileSize /= 40;
            tileSize = -tileSize;
            tileSize += addSize;

            int offsetW = Width / 2 - side * tileSize / 2;
            int offsetH = (Height-tileSize) / 2 - side * tileSize / 2;

            //tileSize = (-Height - side * tileSizeWas)/60;

            //int offsetCenter = (Width/side)*8

            for (int j = 0; j < side; j++)
            {
                for (int i = 0; i < side; i++)
                {
                    //double relX = i / (Height / (double)tileSize);
                    //double relY = j / (Height / (double)tileSize);
                    //int x = (int)(relX * side);
                    //int y = (int)(relY * side);

                    //Layer 1
                    if (map[i, j] == (int)tiles.wall)
                        g.DrawImage(wall, offsetW + i * tileSize, offsetH + j * tileSize, tileSize, tileSize);
                    else if(map[i, j] == (int)tiles.floor)
                        g.DrawImage(floor, offsetW + i * tileSize, offsetH + j * tileSize, tileSize, tileSize);
                    //Layer 2
                }
            }

            pacman.render(g, side, tileSize, offsetW, offsetH);
            //Point p = pacmanCords.ToArray()[0];
            //g.DrawImage(pacman, p);
        }

    }
}
