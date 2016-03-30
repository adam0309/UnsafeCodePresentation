using System.Drawing;
using AForge;
using UsafeCodePresentation.Filters.Interfaces;

namespace UsafeCodePresentation.Filters.ColorFilters
{
    public class AForgeColorFilter:IFilter
    {
        public Bitmap FilterImage(Bitmap bitmap)
        {
            var filter = new AForge.Imaging.Filters.ColorFiltering
            {
                Red = new IntRange(104,104),
                Green = new IntRange(192,192),
                Blue = new IntRange(230,230)
            };

            return filter.Apply(bitmap);
        }
    }
}