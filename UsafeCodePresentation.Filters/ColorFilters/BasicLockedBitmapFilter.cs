﻿using System.Drawing;
using UsafeCodePresentation.Filters.Interfaces;
using UsafeCodePresentation.Filters.Utils;

namespace UsafeCodePresentation.Filters.ColorFilters
{
    public class BasicLockedBitmapFilter: IFilter
    {
        public Bitmap FilterImage(Bitmap bitmap)
        {
            var lockBitmap = new LockBitmap(bitmap);
            lockBitmap.LockBits();
            for(var y=0;y<lockBitmap.Height;y++)
            {
                for (var x = 0; x < lockBitmap.Width; x++)
                {
                    if (lockBitmap.GetPixel(x, y) != Color.FromArgb(104, 192, 230))
                        lockBitmap.SetPixel(x, y, Color.Black);
                }
            }

            lockBitmap.UnlockBits();
            lockBitmap.Dispose();
            return bitmap;
        }
    }
}