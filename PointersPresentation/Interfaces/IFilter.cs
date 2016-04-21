using System.Drawing;

namespace PointersPresentation.Filters.Interfaces
{
    public interface IFilter
    {
        /// <summary>
        /// Method for filtering 104,192,230 color out of an image.
        /// Hardcoded value choosen because of simplicity of an example
        /// </summary>
        /// <param name="bitmap">Bitmap with an image</param>
        /// <returns>Filtred bitmap where all other pixels are filled with black</returns>
        Bitmap FilterImage(Bitmap bitmap);
    }
}