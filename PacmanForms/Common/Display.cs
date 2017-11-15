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

        private Game game;
        public int addSize = 0;
        public int tileSize = 25;

        public Display(Game game)
        {
            InitializeComponent();
            DoubleBuffered = true;
            this.game = game;

            tileSize = -Height / 2 - Width / 2 + tileSize;
            tileSize /= 40;
            tileSize = -tileSize;
            tileSize += addSize;
            game.pacman = new Pacman((game.mapWidth / 2) * tileSize, (game.mapHeight / 2) * tileSize);

            game.t.Resume();
        }

        private void Display_MouseMove(object sender, MouseEventArgs e) { }
        private void Display_MouseClick(object sender, MouseEventArgs e) { }
        private void Display_MouseDown(object sender, MouseEventArgs e) { }

        private void Display_KeyDown(object sender, KeyEventArgs e)
        {
            //Pacman only
            if (!game.pacman.moving) {
                switch (e.KeyCode)
                {
                    case Keys.Up:
                        game.pacman.direction = (int)Pacman.directions.up;
                        break;
                    case Keys.Down:
                        game.pacman.direction = (int)Pacman.directions.down;
                        break;
                    case Keys.Left:
                        game.pacman.direction = (int)Pacman.directions.left;
                        break;
                    case Keys.Right:
                        game.pacman.direction = (int)Pacman.directions.right;
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
            game.t.Abort();
        }

        private void Display_Paint(object sender, PaintEventArgs e)
        {
           
            Graphics g = e.Graphics;
            g.Clear(Color.Black);

            tileSize = -Height / 2 - Width / 2 + tileSize;
            tileSize /= 40;
            tileSize = -tileSize;
            tileSize += addSize;

            int offsetW = Width / 2 - game.mapWidth * tileSize / 2;
            int offsetH = (Height- tileSize) / 2 - game.mapHeight * tileSize / 2;

            for (int j = 0; j < game.mapWidth; j++)
            {
                for (int i = 0; i < game.mapHeight; i++)
                {
                    if (game.map[i, j] == (int)Game.tiles.wall)
                        g.DrawImage(game.wall, offsetW + i * tileSize, offsetH + j * tileSize, tileSize, tileSize);
                    else if(game.map[i, j] == (int)Game.tiles.floor)
                        g.DrawImage(game.floor, offsetW + i * tileSize, offsetH + j * tileSize, tileSize, tileSize);
                    
                }
            }

            game.pacman.render(g, tileSize, offsetW, offsetH);

            g.DrawString($"tileSize = {tileSize}\nX = {game.pacman.x}\nY = {game.pacman.y}", Assets.menuFont, Brushes.White, 0, 0);
            //Point p = pacmanCords.ToArray()[0];
            //g.DrawImage(pacman, p);
        }

    }
}
