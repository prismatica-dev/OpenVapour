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
        internal static Image QuickModify(Image image, Color color, int BorderRadius = 5) {
            if (image == null) return null;
            Bitmap r = ((Bitmap)image).Clone(new Rectangle(0, 0, image.Width, image.Height), image.PixelFormat);
            using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(r)) {
                g.FillRectangles(Brushes.Black, new RectangleF[] { new RectangleF(0, 0, r.Width, BorderRadius), new RectangleF(0, BorderRadius, BorderRadius, r.Height - BorderRadius * 2), new RectangleF(r.Width - BorderRadius, BorderRadius, BorderRadius, r.Height - BorderRadius * 2), new Rectangle(0, r.Height - BorderRadius, r.Width, BorderRadius) });
                g.FillRectangles(new SolidBrush(color), new RectangleF[] { new RectangleF(0, 0, r.Width, BorderRadius), new RectangleF(0, BorderRadius, BorderRadius, r.Height - BorderRadius * 2), new RectangleF(r.Width - BorderRadius, BorderRadius, BorderRadius, r.Height - BorderRadius * 2), new Rectangle(0, r.Height - BorderRadius, r.Width, BorderRadius) }); }
            return r; }
        internal static Image QuickModify(Image image, Color color, int BorderRadius, Font OverlayFont, string Overlay, Color OverlayColor) {
            if (image == null) return null;
            Bitmap r = ((Bitmap)image).Clone(new Rectangle(0, 0, image.Width, image.Height), image.PixelFormat);
            using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(r)) {
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.CompositingMode = CompositingMode.SourceOver;
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.FillRectangles(Brushes.Black, new RectangleF[] { new RectangleF(0, 0, r.Width, BorderRadius * 4), new RectangleF(0, BorderRadius * 4, BorderRadius, r.Height - BorderRadius * 5), new RectangleF(r.Width - BorderRadius, BorderRadius * 4, BorderRadius, r.Height - BorderRadius * 5), new Rectangle(0, r.Height - BorderRadius, r.Width, BorderRadius) });
                g.FillRectangles(new SolidBrush(color), new RectangleF[] { new RectangleF(0, 0, r.Width, BorderRadius * 4), new RectangleF(0, BorderRadius * 4, BorderRadius, r.Height - BorderRadius * 5), new RectangleF(r.Width - BorderRadius, BorderRadius * 4, BorderRadius, r.Height - BorderRadius * 5), new Rectangle(0, r.Height - BorderRadius, r.Width, BorderRadius) });
                Font overlayfont = Utilities.FitFont(OverlayFont, Overlay, new Size(r.Width, BorderRadius * 4));
                g.DrawString(Overlay, new Font(overlayfont.FontFamily, overlayfont.Size, FontStyle.Regular), Brushes.White, new PointF(0, 0)); }
            return r; }
        internal static Image ManipulateDisplayBitmap(Image image, Color color, int BorderRadius = 5) {
            Size _ = GetRatioSize(image);
            Bitmap shadow = new Bitmap(_.Width, _.Height);
            if (shadow.Width > BorderRadius * 2 && shadow.Height > BorderRadius * 2)
                using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(shadow)) { 
                    g.FillRectangle(new SolidBrush(color), new Rectangle(0, 0, shadow.Width, shadow.Height)); 
                    g.DrawImage(image, new Rectangle(BorderRadius, BorderRadius, shadow.Width - BorderRadius * 2, shadow.Height - BorderRadius * 2)); }
            return shadow; }
        internal static Image ManipulateDisplayBitmap(Image image, Color color, int BorderRadius, Font OverlayFont, string Overlay, Color OverlayColor) {
            Size _ = GetRatioSize(image);
            Bitmap shadow = new Bitmap(_.Width, _.Height);
            if (shadow.Width > BorderRadius * 2 && shadow.Height > BorderRadius * 2)
                using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(shadow)) { 
                    g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    g.CompositingMode = CompositingMode.SourceOver;
                    g.CompositingQuality = CompositingQuality.HighQuality;

                    g.FillRectangle(new SolidBrush(color), new Rectangle(0, 0, shadow.Width, shadow.Height)); 
                    g.DrawImage(image, new Rectangle(BorderRadius, BorderRadius * 4, shadow.Width - BorderRadius * 2, shadow.Height - BorderRadius * 5));
                    if (Overlay.Length > 0) {
                        Font overlayfont = Utilities.FitFont(OverlayFont, Overlay, new Size(shadow.Width, BorderRadius * 4));
                        g.FillRectangle(new SolidBrush(Color.FromArgb(255, OverlayColor)), new RectangleF(0, 0, shadow.Width, BorderRadius * 4));
                        g.DrawString(Overlay, new Font(overlayfont.FontFamily, overlayfont.Size, FontStyle.Regular), Brushes.White, new PointF(0, 0)); }}
            return shadow; }}}
