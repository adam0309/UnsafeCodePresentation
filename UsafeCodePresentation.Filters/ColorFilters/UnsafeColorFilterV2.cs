using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection.Emit;
using UsafeCodePresentation.Filters.Interfaces;

namespace UsafeCodePresentation.Filters.ColorFilters
{
    public struct Bgr24Bit
    {
        
        public byte Blue;
        public byte Green;
        public byte Red;
    }
    
    public class UnsafeColorFilterV2:IFilter
    {
        public Bitmap FilterImage(Bitmap bitmap)
        {
            var color = new Bgr24Bit
            {
                Blue = 230,
                Green = 192,
                Red = 104
            };
            Bgr24Bit fill = new Bgr24Bit()
            {
                Blue=0,
                Green=0,
                Red =0
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
                        if (Equals(*(Bgr24Bit*)ptrByte, color)) continue; //Equals is what slows it down because of boxing of ValueType
                        {
                            *(Bgr24Bit*)ptrByte = fill;
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