using System;
using System.Drawing;
using System.Globalization;
using NNPTPZ1.Mathematics;

namespace NNPTPZ1
{
    /// <summary>
    /// This program should produce Newton fractals.
    /// See more at: https://en.wikipedia.org/wiki/Newton_fractal
    /// </summary>
    class Program
    {

        static void Main(string[] args)
        {
            if (args.Length < 6)
            {
                Console.WriteLine("Použití: program <šířka> <výška> <xmin> <xmax> <ymin> <ymax> [výstupní_soubor]");
                return;
            }

            int imageWidth = int.Parse(args[0]);
            int imageHeight = int.Parse(args[1]);

            string output = args.Length > 6 ? args[6] : "../../../out.png";

            Bitmap bmp = new Bitmap(imageWidth, imageHeight);
            double xmin = double.Parse(args[2], CultureInfo.InvariantCulture);
            double xmax = double.Parse(args[3], CultureInfo.InvariantCulture);
            double ymin = double.Parse(args[4], CultureInfo.InvariantCulture);
            double ymax = double.Parse(args[5], CultureInfo.InvariantCulture);

            Poly polynomial = CreatePolynomial();
            Poly derivative = polynomial.Derive();

            Console.WriteLine(polynomial);
            Console.WriteLine(derivative);

            FractalPainter.RenderPixels(
                bmp,
                imageWidth,
                imageHeight,
                xmin,
                xmax,
                ymin,
                ymax,
                polynomial,
                derivative);

            bmp.Save(output);
        }
        private static Poly CreatePolynomial()
        {
            Poly polynomial = new Poly();
            polynomial.Coefficients.Add(new Cplx() { Real = 1 });
            polynomial.Coefficients.Add(Cplx.Zero);
            polynomial.Coefficients.Add(Cplx.Zero);
            polynomial.Coefficients.Add(new Cplx() { Real = 1 });

            return polynomial;
        }
    }
}
