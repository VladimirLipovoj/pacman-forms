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

        public Display(Game game)
        {
            InitializeComponent();
            DoubleBuffered = true;
            this.game = game;


            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(game.Display_FormClosed);
            this.SizeChanged += new System.EventHandler(game.Display_SizeChanged);
            this.Paint += new System.Windows.Forms.PaintEventHandler(game.Display_Paint);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(game.Display_KeyDown);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(game.Display_MouseClick);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(game.Display_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(game.Display_MouseMove);

            game.t = new Thread(game.init);
            game.t.Start();
        }
    }
}
