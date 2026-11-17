using NNPTPZ1.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NNPTPZ1
{

    class NewtonRootFinder
    {
        private const int MaximumNewtonIterations = 30;
        private const double NewtonStepSquaredThreshold = 0.5;

        public static Cplx FindRoot(Poly polynomial, Poly derivative, Cplx initialValue, out int iterations)
        {
            Cplx currentValue = initialValue;
            iterations = 0;

            for (int q = 0; q < MaximumNewtonIterations; q++)
            {
                var diff = polynomial.Eval(currentValue).Divide(derivative.Eval(currentValue));
                currentValue = currentValue.Subtract(diff);

                if (Math.Pow(diff.Real, 2) + Math.Pow(diff.Imaginary, 2) >= NewtonStepSquaredThreshold)
                {
                    q--;
                }

                iterations++;
            }

            return currentValue;
        }
    }
}
