using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacmanForms.Common
{
    class Pacman
    {
        public int x { get; set; } 
        public int y { get; set; } 
        public Animation right { get; set; }
        public Animation left { get; set; }
        public Animation up { get; set; }
        public Animation down { get; set; }
        public int direction { get; set; }

        private readonly int size = 24;

        public enum directions
        {
            left,
            right,
            up,
            down
        }

        public Pacman(int x, int y)
        {
            this.x = x;
            this.y = y;

            direction = (int)directions.right;

            Cropper c = new Cropper(new Bitmap("../../Assets/pacman.png"));

            right = new Animation(300, new Bitmap[] {
                c.Subimage(0,0, size, size),
                c.Subimage(0, size, size, size)
            });

            left = new Animation(300, new Bitmap[] {
                c.Subimage(size, 0, size, size),
                c.Subimage(size, size, size, size)
            });

            up = new Animation(300, new Bitmap[] {
                c.Subimage(size*2, 0, size, size),
                c.Subimage(size*2, size, size, size)
            });

            down = new Animation(300, new Bitmap[] {
                c.Subimage(size*3, 0, size, size),
                c.Subimage(size*3, size, size, size)
            });

        }

        public void tick(ushort[,] mapPacman)
        {
            int oldX = x,
                oldY = y;

            move();

            if ( (x < Display.side && y < Display.side) || (x > 0 && y > 0) )
            {
                mapPacman[oldX, oldY] = (int)Display.entities.air;
                mapPacman[x, y] = (int)Display.entities.pacman;
            } else
            {
                int oldDirection = direction;
                Random rnd = new Random();

                while(oldDirection == direction)
                    direction = rnd.Next(4);
                oldX = x;
                oldY = y;

                move();
                mapPacman[oldX, oldY] = (int)Display.entities.air;
                mapPacman[x, y] = (int)Display.entities.pacman;
            }
        }

        public void move()
        {
            switch (direction)
            {
                case (int)directions.left:
                    left.tick();
                    x--;
                    break;

                case (int)directions.right:
                    right.tick();
                    x++;
                    break;

                case (int)directions.up:
                    up.tick();
                    y++;
                    break;

                case (int)directions.down:
                    down.tick();
                    y--;
                    break;
            }
        }

        public void render(Graphics g, int x, int y, int width, int height)
        {
            Bitmap bmp = null;
            switch (direction)
            {
                case (int)directions.left:
                    bmp = left.getCurrentFrame();
                    break;

                case (int)directions.right:
                    bmp = right.getCurrentFrame();
                    break;

                case (int)directions.up:
                    bmp = up.getCurrentFrame();
                    break;

                case (int)directions.down:
                    bmp = down.getCurrentFrame();
                    break;
            }

            g.DrawImage(bmp, x, y, width, height);
        }

    }
}
