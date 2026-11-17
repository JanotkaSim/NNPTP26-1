using NNPTPZ1.Mathematics;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace NNPTPZ1
{
    internal static class FractalPainter
    {
        private static readonly Color[] RootColors =
        {
            Color.Red, Color.Blue, Color.Green, Color.Yellow, Color.Orange,
            Color.Fuchsia, Color.Gold, Color.Cyan, Color.Magenta
        };

        private const double RootMatchSquaredTolerance = 0.01;
        private const double CoordinateOffset = 0.0001;
        private const int ColorDarkeningPerIteration = 2;

        internal static void RenderPixels(
            Bitmap bmp,
            int imageWidth,
            int imageHeight,
            double xmin,
            double xmax,
            double ymin,
            double ymax,
            Poly polynomial,
            Poly derivative)
        {
            double xstep = (xmax - xmin) / imageWidth;
            double ystep = (ymax - ymin) / imageHeight;
            List<Cplx> knownRoots = new List<Cplx>();

            for (int i = 0; i < imageHeight; i++)
            {
                for (int j = 0; j < imageWidth; j++)
                {
                    double y = ymin + i * ystep;
                    double x = xmin + j * xstep;

                    Cplx currentPoint = new Cplx()
                    {
                        Real = x,
                        Imaginary = (float)y
                    };

                    if (currentPoint.Real == 0)
                        currentPoint.Real = CoordinateOffset;
                    if (currentPoint.Imaginary == 0)
                        currentPoint.Imaginary = (float)CoordinateOffset;

                    int iterations;
                    currentPoint = NewtonRootFinder.FindRoot(
                        polynomial, derivative, currentPoint, out iterations);

                    int rootIndex = FindOrRegisterRoot(currentPoint, knownRoots);
                    Color pixelColor = GetPixelColor(rootIndex, iterations);
                    bmp.SetPixel(j, i, pixelColor);
                }
            }
        }

        private static int FindOrRegisterRoot(
            Cplx currentPoint,
            List<Cplx> knownRoots)
        {
            var rootFound = false;
            var rootIndex = 0;

            for (int i = 0; i < knownRoots.Count; i++)
            {
                if (Math.Pow(currentPoint.Real - knownRoots[i].Real, 2)
                    + Math.Pow(currentPoint.Imaginary - knownRoots[i].Imaginary, 2)
                    <= RootMatchSquaredTolerance)
                {
                    rootFound = true;
                    rootIndex = i;
                }
            }

            if (!rootFound)
            {
                knownRoots.Add(currentPoint);
                rootIndex = knownRoots.Count - 1;
            }

            return rootIndex;
        }

        private static Color GetPixelColor(
            int rootIndex,
            int iterations)
        {
            Color color = RootColors[rootIndex % RootColors.Length];

            return Color.FromArgb(
                Math.Min(Math.Max(0, color.R - iterations * ColorDarkeningPerIteration), 255),
                Math.Min(Math.Max(0, color.G - iterations * ColorDarkeningPerIteration), 255),
                Math.Min(Math.Max(0, color.B - iterations * ColorDarkeningPerIteration), 255));
        }
    }
}