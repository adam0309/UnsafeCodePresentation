using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UsafeCodePresentation.Filters.ColorFilters;

namespace UnsafeCodePresentation.Tests
{
    [TestClass]
    public class BenchmarkFilter
    {
        string TestPatch = Environment.CurrentDirectory + @"\..\..\TestFiles\";
        [TestMethod]
        public void FilterBenchmark()
        {
            Debug.Print($"Test time: {DateTime.Now} \n");
            var bitmap = new Bitmap(TestPatch + "TestImage.jpg");
            var customFilter = new AForgeColorFilter();

            //////////////  /\
            int minTime = int.MaxValue;


            customFilter.FilterImage(bitmap);
            GC.Collect();
            GC.WaitForPendingFinalizers();
            for (int i = 0; i < 10; i++)
            {
                var startTime = Environment.TickCount;
                customFilter.FilterImage(bitmap);
                var endTime = Environment.TickCount;
                var resultTime = endTime - startTime;
                if (minTime > resultTime) minTime = resultTime;
            }
            Debug.Print($"Shortest baseline test took: {minTime} ms");

            /////////////   \/

            var customFilter2 = new UnsafeColorFilterOptimisedV2();
            var res = customFilter2.FilterImage(bitmap);

            //////////////  /\

            minTime = int.MaxValue;
            GC.Collect();
            GC.WaitForPendingFinalizers();
            for (int i = 0; i < 10; i++)
            {
                var startTime = Environment.TickCount;
                /////////////   \/
                customFilter2.FilterImage(bitmap);
                //////////////  /\
                var endTime = Environment.TickCount;
                var resultTime = endTime - startTime;
                if (minTime > resultTime) minTime = resultTime;
            }
            Debug.Print($"Candidate shortest test took: {minTime} ms");



        }

        [TestMethod]
        public void MyTest()
        {
            using (var bitmap = new Bitmap(TestPatch + "TestImage.jpg"))
            {
                var customFilter2 = new BasicLockedBitmapFilter();
                var res = customFilter2.FilterImage(bitmap);
            }



        }
    }
}