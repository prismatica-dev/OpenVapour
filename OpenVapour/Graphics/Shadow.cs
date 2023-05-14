using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Graphics = System.Drawing.Graphics;

namespace OpenVapour.Graphics {
    internal class Shadow {
        internal const int BorderRadius = 20;
        internal static Image AddOuterShadow(Image image, Color color) {
            Bitmap epicshadow = new Bitmap(image.Width, image.Height);
            if (epicshadow.Width > BorderRadius * 2 && epicshadow.Height > BorderRadius * 2)
                using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(epicshadow)) { 
                    g.FillRectangle(new SolidBrush(color), new Rectangle(0, 0, image.Width, image.Height)); 
                    g.DrawImage(image, new Rectangle(BorderRadius - 1, BorderRadius - 1, image.Width - BorderRadius * 2, image.Height - BorderRadius * 2)); }
            return epicshadow; }}}
