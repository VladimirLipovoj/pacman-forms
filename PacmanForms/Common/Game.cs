using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace PacmanForms.Common
{
    public class Game
    {
        private Display display;
        public Thread t;
        public ushort[,] map;
        public readonly int mapWidth = 25,
                            mapHeight = 25;
        
        
        public Pacman pacman;
        public int ticks { get; set; } = 0;

        SolidBrush sb0 = new SolidBrush(Color.Firebrick);
        SolidBrush sb1 = new SolidBrush(Color.ForestGreen);
        Random rnd = new Random();

        //Game resources
        public Image wall { get; set; } = new Bitmap("../../Assets/wall.png");
        public Image floor { get; set; } = new Bitmap("../../Assets/floor.png");

        public enum tiles { floor, wall }
        public enum entities { air, pacman }
        
        public int tileSize = 25;

        public bool displayThread = false;
        public bool gameThread = false;
        public int orderTimer = 0;

        public Game() {

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            display = new Display(this);
            Application.Run(display);

        }

        public void init()
        {
            Assets.init();

            map = new ushort[mapWidth, mapHeight];

            tileSize = display.Height / (mapHeight - 2);
            pacman = new Pacman((mapWidth / 2) * tileSize, (mapHeight / 2) * tileSize);

            for (int i = 0; i < 120; i++)
            {
                int rndX = rnd.Next(0, mapWidth);
                int rndY = rnd.Next(0, mapHeight);

                if (rndX == mapWidth / 2 && rndY == mapHeight / 2)
                    map[rndX, rndY] = (int)tiles.floor;
                else
                    map[rndX, rndY] = (int)tiles.wall;
            }

            for (int i = 0; i < mapWidth; i++)
                for (int j = 0; j < mapHeight; j++)
                    if (i == 0)
                        map[j, i] = (int)tiles.wall;
                    else if (j == 0)
                        map[j, i] = (int)tiles.wall;
                    else if (i == mapWidth-1)
                        map[j, i] = (int)tiles.wall;
                    else if (j == mapHeight-1)
                        map[j, i] = (int)tiles.wall;

                    //                               fpc
            double timePerTick = 1000000000 / 60;
            double delta = 0;

            long nano = 10000L * Stopwatch.GetTimestamp();
            nano /= TimeSpan.TicksPerMillisecond;
            nano *= 100L;

            long now,
                 timer = 0,
                 lastTime = nano;

            //t.Suspend();

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
                        if (pacman != null && display != null)
                        {
                            pacman.tick(tileSize);
                            display.Invalidate();
                        }

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

        public void Display_MouseMove(object sender, MouseEventArgs e) { }
        public void Display_MouseClick(object sender, MouseEventArgs e) { }
        public void Display_MouseDown(object sender, MouseEventArgs e) { }
        public void Display_SizeChanged(object sender, EventArgs e) { }
        public void Display_FormClosed(object sender, FormClosedEventArgs e) { t.Abort(); }

        public void Display_KeyDown(object sender, KeyEventArgs e)
        {
            //Pacman only
            if (!pacman.moving)
            {
                switch (e.KeyCode)
                {
                    case Keys.Up:
                        pacman.prefferDirection = (int)Pacman.directions.up;
                        break;
                    case Keys.Down:
                        pacman.prefferDirection = (int)Pacman.directions.down;
                        break;
                    case Keys.Left:
                        pacman.prefferDirection = (int)Pacman.directions.left;
                        break;
                    case Keys.Right:
                        pacman.prefferDirection = (int)Pacman.directions.right;
                        break;
                }
            }
        }

        public void Display_Paint(object sender, PaintEventArgs e)
        {

            Graphics g = e.Graphics;
            g.Clear(Color.FromArgb(37,37,37));

            tileSize = display.Height / mapHeight;
            tileSize -= 4;

            int offsetW = display.Width / 2 - mapWidth * tileSize / 2;
            int offsetH = display.Height / 2 - mapHeight * tileSize / 2;

            offsetH -= tileSize;

            for (int j = 0; j < mapWidth; j++)
                {
                    for (int i = 0; i < mapHeight; i++)
                    {
                        if (map[i, j] == (int)Game.tiles.wall)
                            g.DrawImage(wall, offsetW + i * tileSize, offsetH + j * tileSize, tileSize, tileSize);
                        else if (map[i, j] == (int)Game.tiles.floor)
                            g.DrawImage(floor, offsetW + i * tileSize, offsetH + j * tileSize, tileSize, tileSize);

                    }
                }

            pacman.render(g, tileSize, offsetW, offsetH);

            TextRenderer.DrawText(g, $"tileSize = {tileSize}\nX = {pacman.x}\nY = {pacman.y}", 
                                  Assets.menuFont, 
                                  new Point(0, 50), 
                                  Color.White);

            //g.DrawString($"tileSize = {tileSize}\nX = {pacman.x}\nY = {pacman.y}", Assets.menuFont, Brushes.White, 0, 0);

            //Point p = pacmanCords.ToArray()[0];
            //g.DrawImage(pacman, p);

        }
    }
 }

