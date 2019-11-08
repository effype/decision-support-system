using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mainapp
{
    class WierszKolumna
    {
        public int Wiersz { get; set; }
        public int Kolumna { get; set; }
        public int Wartosc { get; set; }

        public int CompareTo(WierszKolumna other)
        {
            return Wartosc.CompareTo(other.Wartosc);
        }
    }
}
