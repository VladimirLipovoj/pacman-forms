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
        public double x { get; set; } 
        public double y { get; set; } 
        public Animation right { get; set; }
        public Animation left { get; set; }
        public Animation up { get; set; }
        public Animation down { get; set; }
        public int direction { get; set; }

        private readonly int size = 24;
        private double speed = 0;
        private readonly int animationSpeed = 100;

        public bool moving { get; set; }

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

            right = new Animation(animationSpeed, new Bitmap[] {
                c.Subimage(0,0, size, size),
                c.Subimage(0, size, size, size)
            });

            left = new Animation(animationSpeed, new Bitmap[] {
                c.Subimage(size, 0, size, size),
                c.Subimage(size, size, size, size)
            });

            up = new Animation(animationSpeed, new Bitmap[] {
                c.Subimage(size*2, 0, size, size),
                c.Subimage(size*2, size, size, size)
            });

            down = new Animation(animationSpeed, new Bitmap[] {
                c.Subimage(size*3, 0, size, size),
                c.Subimage(size*3, size, size, size)
            });

        }

        public void tick(int tileSize)
        {
            speed = tileSize/4;

            double oldX = x,
                   oldY = y;

            switch (direction)
            {
                case (int)directions.left:
                    left.tick();
                    x-= speed;
                    break;

                case (int)directions.right:
                    right.tick();
                    x+= speed;
                    break;

                case (int)directions.up:
                    up.tick();
                    y-= speed;
                    break;

                case (int)directions.down:
                    down.tick();
                    y+= speed;
                    break;
            }
            
        }

        public void render(Graphics g, int side, int tileSize, int offsetW, int offsetH)
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

            moving = checkMoving(side, offsetW, offsetH, tileSize);

            g.DrawImage(bmp, (int)(offsetW + x), (int)(offsetH + y), tileSize, tileSize);
        }

        private bool checkMoving(int side, int offsetW, int offsetH, int tileSize)
        {
            bool isMoving = true;

            for (int j = 0; j < side; j++)
            {
                for (int i = 0; i < side; i++)
                {
                    Rectangle expected = new Rectangle(offsetW + i * tileSize, offsetH + j * tileSize, tileSize, tileSize);
                    Rectangle real = new Rectangle((int)(offsetW + x), (int)(offsetH + y), tileSize, tileSize);
                    if (moving && expected == real)
                        isMoving = false;
                }
            }

            return isMoving;
        }

    }
}
