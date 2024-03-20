using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using System.Windows;

namespace WebcamMirror
{
    internal class Utils
    {
        public static BitmapSource ConvertBitmapToBitmapSource(Bitmap bitmap)
        {
            IntPtr hBitmap = bitmap.GetHbitmap();
            BitmapSource bitmapSource;

            try
            {
                bitmapSource = Imaging.CreateBitmapSourceFromHBitmap(
                    hBitmap,
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());
            }
            finally
            {
                // Don't forget to delete the unmanaged bitmap handle.
                DeleteObject(hBitmap);
            }

            return bitmapSource;
        }

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        public static Bitmap CutToSquare(Bitmap originalBitmap)
        {
            int size = Math.Min(originalBitmap.Width, originalBitmap.Height);
            int move = (Math.Max(originalBitmap.Width, originalBitmap.Height) - size) / 2;

            Bitmap resultBitmap = new Bitmap(size, size);

            using (Graphics g = Graphics.FromImage(resultBitmap))
            {
                g.DrawImage(
                    originalBitmap,
                    new System.Drawing.Rectangle(0, 0, size, size),
                    new System.Drawing.Rectangle(move, 0, size, size),
                    GraphicsUnit.Pixel);
                //// Draw the parts from the original bitmap to the result bitmap
                //g.DrawImage(originalBitmap, new System.Drawing.Rectangle(0, 0, cutRectangle.X, originalBitmap.Height), new System.Drawing.Rectangle(0, 0, cutRectangle.X, originalBitmap.Height), GraphicsUnit.Pixel);
                //g.DrawImage(originalBitmap, new System.Drawing.Rectangle(cutRectangle.X, 0, originalBitmap.Width - cutRectangle.Right, originalBitmap.Height), new System.Drawing.Rectangle(cutRectangle.Right, 0, originalBitmap.Width - cutRectangle.Right, originalBitmap.Height), GraphicsUnit.Pixel);
            }

            return resultBitmap;
        }
    }
}
