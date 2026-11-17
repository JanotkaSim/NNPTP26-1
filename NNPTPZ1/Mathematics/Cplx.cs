using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NNPTPZ1.Mathematics
{
    public class Cplx
    {
        public double Real { get; set; }
        public float Imaginary { get; set; }

        public override bool Equals(object obj)
        {
            Cplx other = obj as Cplx;

            return other != null
                && Real.Equals(other.Real)
                && Imaginary.Equals(other.Imaginary);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 31 + Real.GetHashCode();
                hash = hash * 31 + Imaginary.GetHashCode();
                return hash;
            }
        }

        public readonly static Cplx Zero = new Cplx()
        {
            Real = 0,
            Imaginary = 0
        };

        public Cplx Multiply(Cplx b)
        {
            Cplx a = this;
            // aRe*bRe + aRe*bIm*i + aIm*bRe*i + aIm*bIm*i*i
            return new Cplx()
            {
                Real = a.Real * b.Real - a.Imaginary * b.Imaginary,
                Imaginary = (float)(a.Real * b.Imaginary + a.Imaginary * b.Real)
            };
        }
        public double GetAbS()
        {
            return Math.Sqrt(Real * Real + Imaginary * Imaginary);
        }

        public Cplx Add(Cplx b)
        {
            Cplx a = this;
            return new Cplx()
            {
                Real = a.Real + b.Real,
                Imaginary = a.Imaginary + b.Imaginary
            };
        }
        public double GetAngleInDegrees()
        {
            return Math.Atan(Imaginary / Real);
        }
        public Cplx Subtract(Cplx b)
        {
            Cplx a = this;
            return new Cplx()
            {
                Real = a.Real - b.Real,
                Imaginary = a.Imaginary - b.Imaginary
            };
        }

        public override string ToString()
        {
            return $"({Real} + {Imaginary}i)";
        }

        internal Cplx Divide(Cplx b)
        {
            // (aRe + aIm*i) / (bRe + bIm*i)
            // ((aRe + aIm*i) * (bRe - bIm*i)) / ((bRe + bIm*i) * (bRe - bIm*i))
            //  bRe*bRe - bIm*bIm*i*i
            var tmp = this.Multiply(new Cplx() { Real = b.Real, Imaginary = -b.Imaginary });
            var tmp2 = b.Real * b.Real + b.Imaginary * b.Imaginary;

            return new Cplx()
            {
                Real = tmp.Real / tmp2,
                Imaginary = (float)(tmp.Imaginary / tmp2)
            };
        }
    }
}
