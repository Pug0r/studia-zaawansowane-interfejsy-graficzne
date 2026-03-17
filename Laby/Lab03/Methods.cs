using System;
using System.Collections.Generic;
using System.Text;

namespace Lab03
{
    public static class Methods
    {
        public static double potega(int podstawa, int wykladnik, int kontrolka)
        {
            if (kontrolka <= 0)
                throw new ArgumentException("trzeci argument jest mniejszy od 0");
            if (wykladnik < 0)
                throw new ArgumentException("wykladnik mniejszy od 0");
            return Math.Pow(podstawa, wykladnik);

        }

        public static int[] zapiszWTablicy(int[] tablica, int mnoznik)
            => [.. tablica.Select(x => x * mnoznik)];

        public static double poleKola(double promien)
        {
            if (promien <= 0) 
                throw new ArgumentException("promien musi byc wiekszy od 0");
            return promien * promien * Math.PI;
        }

        public static string sumaCyfr(double liczba)
        {
            bool isInteger = (liczba % 1 == 0);
            bool hasThreeLeadingDigits = liczba >= 100 && liczba < 1000;
            if ((isInteger == false) || (hasThreeLeadingDigits == false))
                return "liczba nie jest trzycyfrowa";

            return (int)liczba % 3 == 0 ? "liczba podzielna przez 3" : "liczba nie jest podzielna przez 3";
        }

        public static object[] zamienElementy(object[] tablica, int indeksJeden, int indeksDwa, int flaga)
        {
            if (flaga <= 0)
                return tablica;

            bool isValidIndex(int i) => i >= 0 && i < tablica.Length;

            if (isValidIndex(indeksJeden) == false || isValidIndex(indeksDwa) == false)
                throw new ArgumentException("indeks spoza zakresu");

            (tablica[indeksJeden], tablica[indeksDwa]) = (tablica[indeksDwa], tablica[indeksJeden]);
            return tablica;
        }
    }
}
