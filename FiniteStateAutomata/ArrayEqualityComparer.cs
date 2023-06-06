using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiniteStateAutomata
{
    public class ArrayEqualityComparer<TArray> : IEqualityComparer<TArray[]> where TArray : IComparable
    {
        public bool Equals(TArray[] x, TArray[] y)
        {
            if (x.Length != y.Length) //If the arrays are different lengths, they are definitely not the same
            {
                return false;
            }
            for (int i = 0; i < x.Length; i++)
            {
                if (!x[i].Equals(y[i])) //Check equality for each element
                {
                    return false;
                }
            }
            return true;
        }

        public int GetHashCode(TArray[] obj)
        {
            int hash = 17; //Choose a random prime
            foreach (TArray item in obj)
            {
                hash = hash * 23 + item.GetHashCode(); //Do some multiplying and adding with some primes
            }

            return hash;
        }
    }
}
