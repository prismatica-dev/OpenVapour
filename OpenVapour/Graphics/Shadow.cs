using System.Drawing;

namespace OpenVapour.Graphics {
    internal class Shadow {
        internal const int BorderRadius = 20;
        internal static Image AddOuterShadow(Image image, Color color) {
            Bitmap shadow = new Bitmap(image.Width, image.Height);
            if (shadow.Width > BorderRadius * 2 && shadow.Height > BorderRadius * 2)
                using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(shadow)) { 
                    g.FillRectangle(new SolidBrush(color), new Rectangle(0, 0, image.Width, image.Height)); 
                    g.DrawImage(image, new Rectangle(BorderRadius - 1, BorderRadius - 1, image.Width - BorderRadius * 2, image.Height - BorderRadius * 2)); }
            return shadow; }}}
