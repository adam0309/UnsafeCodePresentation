using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace UsafeCodePresentation.Filters.Utils
{
        public class LockBitmap : IDisposable
        {
            readonly Bitmap sourceImage;
            IntPtr _pointer = IntPtr.Zero;
            BitmapData _bitmapData = null;

            public byte[] Pixels { get; set; }
            public int Depth { get; private set; }
            public int Width { get; private set; }
            public int Height { get; private set; }

            public LockBitmap(Bitmap image)
            {
                this.sourceImage = image;
            }

            /// <summary>
            /// Lock bitmap data
            /// </summary>
            public void LockBits()
            {

                // Get width and height of bitmap
                Width = sourceImage.Width;
                Height = sourceImage.Height;
                // get total locked pixels count
                var pixelCount = Width * Height;

                // Create rectangle to lock
                var rect = new Rectangle(0, 0, Width, Height);

                // get sourceImage bitmap pixel format size
                Depth = System.Drawing.Bitmap.GetPixelFormatSize(sourceImage.PixelFormat);

                // Check if bpp (Bits Per Pixel) is 8, 24, or 32
                if (Depth != 8 && Depth != 24 && Depth != 32)
                {
                    throw new ArgumentException("Only 8, 24 and 32 bpp images are supported.");
                }
                try
                {
                    // Lock bitmap and return bitmap data
                    _bitmapData = sourceImage.LockBits(rect, ImageLockMode.ReadWrite,
                                                 sourceImage.PixelFormat);


                    // create byte array to copy pixel values
                    var step = Depth / 8;
                    Pixels = new byte[pixelCount * step];
                    _pointer = _bitmapData.Scan0;

                    // Copy data from pointer to array
                    Marshal.Copy(_pointer, Pixels, 0, Pixels.Length);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// Unlock bitmap data
            /// </summary>
            public void UnlockBits()
            {
                try
                {
                    // Copy data from byte array to pointer
                    Marshal.Copy(Pixels, 0, _pointer, Pixels.Length);

                    // Unlock bitmap data
                    sourceImage.UnlockBits(_bitmapData);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// Get the color of the specified pixel
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <returns></returns>
            public Color GetPixel(int x, int y)
            {
                var color = Color.Empty;

                // Get color components count
                var count = Depth / 8;

                // Get start index of the specified pixel
                var i = ((y * Width) + x) * count;

                if (i > Pixels.Length - count)
                    throw new IndexOutOfRangeException();

                if (Depth == 32) // For 32 bpp get Red, Green, Blue and Alpha
                {
                    var b = Pixels[i];
                    var g = Pixels[i + 1];
                    var r = Pixels[i + 2];
                    var a = Pixels[i + 3]; // a
                    color = Color.FromArgb(a, r, g, b);
                }
                if (Depth == 24) // For 24 bpp get Red, Green and Blue
                {
                    var b = Pixels[i];
                    var g = Pixels[i + 1];
                    var r = Pixels[i + 2];
                    color = Color.FromArgb(r, g, b);
                }
                if (Depth == 8)
                // For 8 bpp get color value (Red, Green and Blue values are the same)
                {
                    var c = Pixels[i];
                    color = Color.FromArgb(c, c, c);
                }
                return color;
            }

            /// <summary>
            /// Set the color of the specified pixel
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <param name="color"></param>
            public void SetPixel(int x, int y, Color color)
            {
                // Get color components count
                var count = Depth / 8;

                // Get start index of the specified pixel
                var i = ((y * Width) + x) * count;

                if (Depth == 32) // For 32 bpp set Red, Green, Blue and Alpha
                {
                    Pixels[i] = color.B;
                    Pixels[i + 1] = color.G;
                    Pixels[i + 2] = color.R;
                    Pixels[i + 3] = color.A;
                }
                if (Depth == 24) // For 24 bpp set Red, Green and Blue
                {
                    Pixels[i] = color.B;
                    Pixels[i + 1] = color.G;
                    Pixels[i + 2] = color.R;
                }
                if (Depth == 8)
                // For 8 bpp set color value (Red, Green and Blue values are the same)
                {
                    Pixels[i] = color.B;
                }
            }

        public void Dispose()
        {
            GC.Collect(2, GCCollectionMode.Optimized);
            GC.SuppressFinalize(this);
        }
    }
}

