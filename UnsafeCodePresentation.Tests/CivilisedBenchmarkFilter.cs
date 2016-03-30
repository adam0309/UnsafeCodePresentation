﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UsafeCodePresentation.Filters.ColorFilters;
using UsafeCodePresentation.Filters.Interfaces;

namespace UnsafeCodePresentation.Tests
{
    [TestClass]
    public class CivilisedBenchmarkFilter
    {
        private string _testPatch = Environment.CurrentDirectory + @"\..\..\TestFiles\";

        [TestMethod]
        public void BenchmarkFilters()
        {
            Debug.Print($"Test time: {DateTime.Now} \n");

            var filterAssembly = AppDomain.CurrentDomain.Load(
                    "UsafeCodePresentation.Filters, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
            var filters =
                filterAssembly.GetTypes()
                    .Where(filterClass => typeof(IFilter).IsAssignableFrom(filterClass) && !filterClass.IsInterface).ToList();

            var filterInstances = filters.Select(filter => (IFilter) Activator.CreateInstance(filter)).ToList();

            foreach (var filterInstance in filterInstances)
            {
                Benchmark(filterInstance);
            }
        }

        protected void Benchmark(IFilter filter)
        {
            using (var bitmap = new Bitmap(_testPatch + "TestImage.jpg"))
            {
                var minTime = int.MaxValue;


                filter.FilterImage(bitmap);
                GC.Collect();
                GC.WaitForPendingFinalizers();
                for (int i = 0; i < 10; i++)
                {
                    var startTime = Environment.TickCount;
                    filter.FilterImage(bitmap);
                    var endTime = Environment.TickCount;
                    var resultTime = endTime - startTime;
                    if (minTime > resultTime) minTime = resultTime;
                    if (resultTime > 2000) break;

                }
                Debug.Print($"Shortest {filter.GetType().Name.Split('.').Last()} test took: {minTime} ms");
            }


        }

    }
}