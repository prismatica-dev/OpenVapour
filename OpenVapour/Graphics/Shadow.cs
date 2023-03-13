using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Graphics = System.Drawing.Graphics;

namespace OpenVapour.Graphics {
    internal class Shadow {
        internal static Image AddOuterShadow(Image image, Color color) {
            Bitmap epicshadow = new Bitmap(image.Width, image.Height);
            using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(epicshadow)) { 
                g.FillRectangle(new SolidBrush(color), new Rectangle(0, 0, image.Width, image.Height)); 
                g.DrawImage(image, new Rectangle(image.Width / 20, image.Width / 20, image.Width - image.Width / 10, image.Height - image.Width / 10)); }
            return epicshadow; }}}
