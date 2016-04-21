using System.Drawing;
using AForge;
using PointersPresentation.Filters.Interfaces;

namespace PointersPresentation.Filters.ColorFilters
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