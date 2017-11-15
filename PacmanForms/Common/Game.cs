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

        SolidBrush sb0 = new SolidBrush(Color.Firebrick);
        SolidBrush sb1 = new SolidBrush(Color.ForestGreen);
        Random rnd = new Random();

        //Game resources
        public Image wall { get; set; } = new Bitmap("../../Assets/wall.png");
        public Image floor { get; set; } = new Bitmap("../../Assets/floor.png");

        public enum tiles { floor, wall }
        public enum entities { air, pacman }

        public Game()
        {

            t = new Thread(init);
            t.Start();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            display = new Display(this);
            Application.Run(display);
        }

        private void init()
        {
            Assets.init();

            map = new ushort[mapWidth, mapHeight];
            //mapPacman = new ushort[side, side];

            for (int i = 0; i < 120; i++)
            {
                int rndX = rnd.Next(0, mapWidth);
                int rndY = rnd.Next(0, mapHeight);

                if (rndX == mapWidth / 2 && rndY == mapHeight / 2)
                    map[rndX, rndY] = (int)tiles.floor;
                else
                    map[rndX, rndY] = (int)tiles.wall;
            }

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

            t.Suspend();

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
                    pacman.tick(display.tileSize);
                    display.Invalidate();

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
    }
 }

