﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PacmanForms.Common
{
    public class Pacman
    {
        public double x { get; set; } 
        public double y { get; set; } 
        public Animation right { get; set; }
        public Animation left { get; set; }
        public Animation up { get; set; }
        public Animation down { get; set; }
        public int direction { get; set; }

        private readonly int size = 72;
        //24
        private double speed = 0;
        private readonly int animationSpeed = 100;

        public bool moving { get; set; }

        public int prefferDirection { get; set; } = -1;

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

            Cropper c = new Cropper(new Bitmap("../../Assets/pacmanHD.png"));

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

        public void render(Graphics g, int tileSize, int offsetW, int offsetH)
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
            //if(checkCellMatch(25, offsetW, offsetH, tileSize))
            //    TextRenderer.DrawText(g, "true",
            //                      Assets.menuFont,
            //                      new Point(0, 0),
            //                      Color.White);
            //else
            //    TextRenderer.DrawText(g, "true",
            //                      Assets.menuFont,
            //                      new Point(0, 0),
            //                      Color.White);

            if (prefferDirection != -1 && checkCellMatch(25, (int)(offsetW + x), (int)(offsetH + y), tileSize))
            {
                switch(prefferDirection)
                {
                    case (int)directions.left:
                        direction = (int)directions.left;
                        break;

                    case (int)directions.right:
                        direction = (int)directions.right;
                        break;

                    case (int)directions.up:
                        direction = (int)directions.up;
                        break;

                    case (int)directions.down:
                        direction = (int)directions.down;
                        break;
                }

                prefferDirection = -1;
            }

            g.DrawImage(bmp, (int)(offsetW + x), (int)(offsetH + y), tileSize, tileSize);
        }

        private bool checkCellMatch(int side, int offsetW, int offsetH, int tileSize)
        {
            bool isMatching = true;

            for (int j = 0; j < side; j++)
            {
                for (int i = 0; i < side; i++)
                {
                    Rectangle expected = new Rectangle(offsetW + i * tileSize, offsetH + j * tileSize, tileSize, tileSize);
                    Rectangle real = new Rectangle((int)(offsetW + x), (int)(offsetH + y), tileSize, tileSize);
                    if (expected.Equals(real))
                        isMatching = false;
                }
            }

            return isMatching;
        }

    }
}
