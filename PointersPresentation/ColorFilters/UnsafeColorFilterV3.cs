using System;
using System.Drawing;
using System.Drawing.Imaging;
using PointersPresentation.Filters.Interfaces;

namespace PointersPresentation.Filters.ColorFilters
{
    public struct Bgr24BitV2:IEquatable<Bgr24BitV2>
    {
        public byte Blue;
        public byte Green;
        public byte Red;
        public bool Equals(Bgr24BitV2 other)
        {
            return this.Blue == other.Blue && this.Green == other.Green && this.Red == other.Red;
        }
    }

    public class UnsafeColorFilterV3 : IFilter
    {
        public Bitmap FilterImage(Bitmap bitmap)
        {
            var color = new Bgr24BitV2
            {
                Blue = 230,
                Green = 192,
                Red = 104
            };
            var fill = new Bgr24BitV2()
            {
                Blue = 0,
                Green = 0,
                Red = 0
            };
            Rectangle rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            var bitmapData = bitmap.LockBits(rect, ImageLockMode.ReadWrite, bitmap.PixelFormat);

            unsafe
            {
                byte* ptrByte = (byte*)bitmapData.Scan0.ToPointer();
                for (int i = 0; i < bitmapData.Height; i++)
                {
                    for (int j = 0; j < bitmapData.Width; j++, ptrByte += 3)
                    {
                        if (color.Equals(*(Bgr24BitV2*)ptrByte)) continue; 
                        {
                            *(Bgr24BitV2*)ptrByte = fill;
                        }
                    }
                    //Calculating next row offset
                    ptrByte += bitmapData.Stride - (bitmapData.Width * 3);
                }
            }
            bitmap.UnlockBits(bitmapData);
            return bitmap;
        }
    }
}