using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace OpenVapour.OpenVapourAPI {
    internal class Graphics {
        internal static Size GetRatioSize(Image img, int MaxDimension = 225, int MinDimension = 150) {
            float MaximumSize = Math.Max(img.Width, img.Height);
            return new Size(
                Math.Max((int)Math.Round(img.Width / MaximumSize * MaxDimension), MinDimension),
                Math.Max((int)Math.Round(img.Height / MaximumSize * MaxDimension), MinDimension)); }
        internal static Bitmap DrawGradient(int Width, int Height) {
            Bitmap bitmap = new Bitmap(Width, Height);
            using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap)) { g.FillRectangle(new LinearGradientBrush(new PointF(0, 0), new PointF(0, Height), UserSettings.WindowTheme["background1"], UserSettings.WindowTheme["background2"]), new Rectangle(0, 0, Width, Height)); }
            return bitmap; }
        internal static Image ManipulateDisplayBitmap(Image image, Color color, int BorderRadius = 5) {
            Size _ = GetRatioSize(image);
            Bitmap shadow = new Bitmap(_.Width, _.Height);
            if (shadow.Width > BorderRadius * 2 && shadow.Height > BorderRadius * 2)
                using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(shadow)) { 
                    g.FillRectangle(new SolidBrush(color), new Rectangle(0, 0, shadow.Width, shadow.Height)); 
                    g.DrawImage(image, new Rectangle(BorderRadius - 1, BorderRadius - 1, shadow.Width - BorderRadius * 2, shadow.Height - BorderRadius * 2)); }
            return shadow; }
        internal static Image ManipulateDisplayBitmap(Image image, Color color, int BorderRadius, Font OverlayFont, string Overlay, Color OverlayColor) {
            Size _ = GetRatioSize(image);
            Bitmap shadow = new Bitmap(_.Width, _.Height);
            if (shadow.Width > BorderRadius * 2 && shadow.Height > BorderRadius * 2)
                using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(shadow)) { 
                    g.FillRectangle(new SolidBrush(color), new Rectangle(0, 0, shadow.Width, shadow.Height)); 
                    g.DrawImage(image, new Rectangle(BorderRadius - 1, BorderRadius - 1, shadow.Width - BorderRadius * 2, shadow.Height - BorderRadius * 2));
                    if (Overlay.Length > 0) {
                        Font overlayfont = Utilities.FitFont(OverlayFont, Overlay, new Size(shadow.Width, BorderRadius * 2));
                        g.FillRectangle(new SolidBrush(Color.FromArgb(255, OverlayColor)), new RectangleF(0, 0, shadow.Width, BorderRadius * 2));
                        g.DrawString(Overlay, new Font(overlayfont.FontFamily, overlayfont.Size, FontStyle.Bold), Brushes.White, new PointF(0, 0)); }}
            return shadow; }}}
