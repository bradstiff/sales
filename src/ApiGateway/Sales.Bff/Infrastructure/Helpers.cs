using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sales.Bff.Infrastructure
{
    public static class Helpers
    {
        public static IEnumerable<long> GetBits(this long bitfield)
        {
            var bitArray = new BitArray(BitConverter.GetBytes(bitfield));
            for (int i = 0; i < bitArray.Length; i++)
            {
                if (bitArray[i])
                {
                    yield return (long)Math.Pow(2, i);
                }
            }

        }
    }
}
