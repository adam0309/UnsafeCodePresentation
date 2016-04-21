using System.Drawing;
using System.Drawing.Imaging;
using PointersPresentation.Filters.Interfaces;

namespace PointersPresentation.Filters.ColorFilters
{
    public unsafe class UnsafeColorFilter: IFilter
    {
        public Bitmap FilterImage(Bitmap bitmap)
        {
            Rectangle rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);            var bitmapData = bitmap.LockBits(rect, ImageLockMode.ReadWrite, bitmap.PixelFormat);
            int wysokosc = bitmapData.Height;
            int szerokosc = bitmapData.Width;
            unsafe
            {
                byte* ptr = (byte*)bitmapData.Scan0.ToPointer();
                for (int i = 0; i < wysokosc; i++)
                {
                    for (int j = 0; j < szerokosc; j++, ptr += 3)
                    {
                        if (*ptr == 230 && *(ptr + 1) == 192 && *(ptr + 2) == 104) continue;
                        { 
                            *ptr = 0;
                            *(ptr + 1) = 0;
                            *(ptr + 2) = 0;
                        }
                    }
                    //Calculating next row offset
                    ptr += bitmapData.Stride - (bitmapData.Width * 3);
                }
            }
            
            bitmap.UnlockBits(bitmapData);
            return bitmap;

        }

        void costam(byte* pointer)
        {
        }


    }

}