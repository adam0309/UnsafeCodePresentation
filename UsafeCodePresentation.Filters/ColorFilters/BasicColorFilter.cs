using System.Drawing;
using UsafeCodePresentation.Filters.Interfaces;

namespace UsafeCodePresentation.Filters.ColorFilters
{
    public class BasicColorFilter: IFilter
    {
        public Bitmap FilterImage(Bitmap bitmap)
        {
            for(int y=0;y<bitmap.Height;y++)
            {
                for (int x = 0; x < bitmap.Width;x++)
                {
                    if(bitmap.GetPixel(x,y)!=Color.FromArgb(104,192,230))
                        bitmap.SetPixel(x,y,Color.Black);
                }
            }

            return bitmap;
        }
    }
}