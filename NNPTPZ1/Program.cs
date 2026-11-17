using System;
using System.Collections.Generic;
using System.Drawing;
using NNPTPZ1.Mathematics;

namespace NNPTPZ1
{
    /// <summary>
    /// This program should produce Newton fractals.
    /// See more at: https://en.wikipedia.org/wiki/Newton_fractal
    /// </summary>
    class Program
    {

        private const double RootMatchSquaredTolerance = 0.01;
        private const double CoordinateOffset = 0.0001;
        private const int ColorDarkeningPerIteration = 2;
        static void Main(string[] args)
        {
            int[] intargs = new int[2];
            for (int i = 0; i < intargs.Length; i++)
            {
                intargs[i] = int.Parse(args[i]);
            }
            double[] doubleargs = new double[4];
            for (int i = 0; i < doubleargs.Length; i++)
            {
                doubleargs[i] = double.Parse(args[i + 2]);
            }
            string output = args[6];
            // TODO: add parameters from args?
            Bitmap bmp = new Bitmap(intargs[0], intargs[1]);
            double xmin = doubleargs[0];
            double xmax = doubleargs[1];
            double ymin = doubleargs[2];
            double ymax = doubleargs[3];

            double xstep = (xmax - xmin) / intargs[0];
            double ystep = (ymax - ymin) / intargs[1];

            List<Cplx> knownRoots = new List<Cplx>();
            // TODO: poly should be parameterised?
            Poly polynomial = new Poly();
            polynomial.Coefficients.Add(new Cplx() { Real = 1 });
            polynomial.Coefficients.Add(Cplx.Zero);
            polynomial.Coefficients.Add(Cplx.Zero);
            //polynomial.Coefficients.Add(Cplx.Zero);
            polynomial.Coefficients.Add(new Cplx() { Real = 1 });
            Poly derivative = polynomial.Derive();

            Console.WriteLine(polynomial);
            Console.WriteLine(derivative);

            var rootColors = new Color[]
            {
                Color.Red, Color.Blue, Color.Green, Color.Yellow, Color.Orange, Color.Fuchsia, Color.Gold, Color.Cyan, Color.Magenta
            };


            // TODO: cleanup!!!
            // for every pixel in image...
            for (int i = 0; i < intargs[0]; i++)
            {
                for (int j = 0; j < intargs[1]; j++)
                {
                    // find "world" coordinates of pixel
                    double y = ymin + i * ystep;
                    double x = xmin + j * xstep;

                    Cplx currentPoint = new Cplx()
                    {
                        Real = x,
                        Imaginary = (float)(y)
                    };

                    if (currentPoint.Real == 0)
                        currentPoint.Real = CoordinateOffset;
                    if (currentPoint.Imaginary == 0)
                        currentPoint.Imaginary = (float)CoordinateOffset;

                    //Console.WriteLine(currentPoint);

                    // find solution of equation using newton's iteration
                    int iterations;
                    currentPoint = NewtonRootFinder.FindRoot(polynomial, derivative, currentPoint, out iterations);

                    //Console.ReadKey();

                    // find solution root number
                    int rootIndex = FindOrRegisterRoot(currentPoint, knownRoots);
                    Color pixelColor = GetPixelColor(rootIndex, iterations, rootColors);
                    bmp.SetPixel(j, i, pixelColor);
                }
            }

            bmp.Save(output ?? "../../../out.png");
            //Console.ReadKey();

        }

        private static int FindOrRegisterRoot(Cplx currentPoint, List<Cplx> knownRoots)
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
        int iterations,
        Color[] rootColors)
        {
            Color color = rootColors[rootIndex % rootColors.Length];

            return Color.FromArgb(
                Math.Min(Math.Max(0, color.R - iterations * ColorDarkeningPerIteration), 255),
                Math.Min(Math.Max(0, color.G - iterations * ColorDarkeningPerIteration), 255),
                Math.Min(Math.Max(0, color.B - iterations * ColorDarkeningPerIteration), 255));
        }
    }
}
