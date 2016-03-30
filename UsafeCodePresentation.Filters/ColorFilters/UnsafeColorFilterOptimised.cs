using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using UsafeCodePresentation.Filters.Interfaces;

namespace UsafeCodePresentation.Filters.ColorFilters
{
    public class UnsafeColorFilterOptimised : IFilter
    {
        public Bitmap FilterImage(Bitmap bitmap)
        {
            Rectangle rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            var bitmapData = bitmap.LockBits(rect, ImageLockMode.ReadWrite, bitmap.PixelFormat);
            int height = bitmapData.Height;
            int width = bitmapData.Width;
            int stride = bitmapData.Stride - (width * 3);
            
            unsafe
            {
                byte* ptr = (byte*)bitmapData.Scan0.ToPointer();
                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j+=2, ptr+=6)
                    {
                        if (ptr[0] != 230 && ptr[1] != 192 && ptr[2] != 104)
                        {
                            ptr[0] = 0;
                            ptr[1] = 0;
                            ptr[2] = 0;
                        }
                        if (ptr[3] != 230 && ptr[4] != 192 && ptr[5] != 104)
                        {
                            ptr[3] = 0;
                            ptr[4] = 0;
                            ptr[5] = 0;
                        }
                    }
                    //Calculating next row offset
                    ptr += stride;
                }
            }

            bitmap.UnlockBits(bitmapData);
            return bitmap;
        }
    }
}