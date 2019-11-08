using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mainapp
{
    class IndeksWartosc : IComparable<IndeksWartosc>
    {
        public int Indeks { get; set; }
        public double Wartosc { get; set; }

        public int CompareTo(IndeksWartosc other)
        {
            return Wartosc.CompareTo(other.Wartosc);
        }
    }
}
