using OpenVapour.Steam;
using System.Drawing;

namespace OpenVapour.OpenVapourAPI {
    internal class Graphics {
        internal static Image ManipulateDisplayBitmap(Image image, Color color, int BorderRadius = 20, Font OverlayFont = null, string Overlay = "") {
            Bitmap shadow = new Bitmap(image.Width, image.Height);
            if (shadow.Width > BorderRadius * 2 && shadow.Height > BorderRadius * 2)
                using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(shadow)) { 
                    g.FillRectangle(new SolidBrush(color), new Rectangle(0, 0, image.Width, image.Height)); 
                    g.DrawImage(image, new Rectangle(BorderRadius - 1, BorderRadius - 1, image.Width - BorderRadius * 2, image.Height - BorderRadius * 2));
                    if (Overlay.Length > 0) {
                        Font overlayfont = Utilities.FitFont(OverlayFont, Overlay, new Size(shadow.Width, BorderRadius * 2));
                        g.FillRectangle(Brushes.Black, new RectangleF(0, 0, shadow.Width, BorderRadius * 2));
                        g.DrawString(Overlay, new Font(overlayfont.FontFamily, overlayfont.Size, FontStyle.Bold), Brushes.White, new PointF(0, 0)); }}
            return shadow; }}}
