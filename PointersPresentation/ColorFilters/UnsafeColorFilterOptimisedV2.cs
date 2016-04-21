using System.CodeDom.Compiler;
using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using PointersPresentation.Filters.Interfaces;

namespace PointersPresentation.Filters.ColorFilters
{
    public class UnsafeColorFilterOptimisedV2 : IFilter
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [CompilationRelaxations(CompilationRelaxations.NoStringInterning)]
        public Bitmap FilterImage(Bitmap bitmap)
        {
            
            Rectangle rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            var bitmapData = bitmap.LockBits(rect, ImageLockMode.ReadWrite, bitmap.PixelFormat);
            Parallel.Invoke();

            unsafe
            {
                byte* ptr = (byte*)bitmapData.Scan0.ToPointer();
                int height = bitmapData.Height;
                int width = bitmapData.Width;
                byte* widthPtr = ptr+(width*3);
                int stride = bitmapData.Stride - (bitmapData.Width * 3);
                //http://puu.sh/nV20C/9cdd98bf49.png
                
                for (int i = 0; i < height; i++)
                {
       
                    //for (int j = 0; j < width; j += 2, ptr += 6)
                    do
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
                        ptr += 6;
                    } while (ptr < widthPtr);
                    //Calculating next row offset
                    ptr += stride;
                    widthPtr = ptr + (width * 3);
                }
            }

            bitmap.UnlockBits(bitmapData);
            return bitmap;
        }
    }
}