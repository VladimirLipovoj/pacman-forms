using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacmanForms.Common
{
    public class Assets
    {
        public static Bitmap logo { get; set; }
        public static Font menuFont { get; set; }

        public static void init()
        {
            PrivateFontCollection f = new PrivateFontCollection();
            f.AddFontFile("../../Assets/emulogic.ttf");
            menuFont = new Font(f.Families[0], 10);
        }
    }
}