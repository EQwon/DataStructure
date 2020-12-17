using System;
using System.Collections.Generic;
using System.Text;

namespace DataStructure.MyDictionary
{
    public static class HashHelper
    {
        public static readonly int[] primes = {
            3, 7, 11, 17, 23, 29, 37, 47, 59, 71, 89, 107, 131, 163, 197, 239, 293, 353, 431, 521, 631, 761, 919,
            1103, 1327, 1597, 1931, 2333, 2801, 3371, 4049, 4861, 5839, 7013, 8419, 10103, 12143, 14591,
            17519, 21023, 25229, 30293, 36353, 43627, 52361, 62851, 75431, 90523, 108631, 130363, 156437,
            187751, 225307, 270371, 324449, 389357, 467237, 560689, 672827, 807403, 968897, 1162687, 1395263,
            1674319, 2009191, 2411033, 2893249, 3471899, 4166287, 4999559, 5999471, 7199369};
        public const int MaxPrimeArrayLength = 0x7FEFFFFD;

        // 기존 소수보다 2배 초과의 소수를 찾기
        public static int ExpandPrime(int oldPrime)
        {
            int newPrime = 2 * oldPrime;

            // 단 int 범위의 최대 소수를 넘는지 체크 한 번 합니다.
            if ((uint)newPrime > MaxPrimeArrayLength && MaxPrimeArrayLength > oldPrime)
                return MaxPrimeArrayLength;

            return GetPrime(newPrime);
        }

        public static int GetPrime(int min)
        {
            if (min < 0) Console.WriteLine("0보다 작은 소수를 찾을 수 없습니다.");

            for (int i = 0; i < primes.Length; i++)
                if (primes[i] >= min) return primes[i];

            // 만약 미리 정의된 소수의 범위를 벗어난다면
            // 그냥 계산해야지 뭐...
            for (int i = (min | 1); i < int.MaxValue; i += 2)     // 여기서 min | 1 의 의미는 홀수로 만들기 위함
            {
                if (IsPrime(i)) return i;
            }

            return min;
        }

        public static bool IsPrime(int candidate)
        {
            if ((candidate & 1) != 0)    // 홀수냐고 물어보는 것
            {
                int limit = (int)Math.Sqrt(candidate);
                for (int divisor = 3; divisor <= limit; divisor += 2)
                {
                    if ((candidate % divisor) == 0) return false;
                }
                return true;
            }
            return (candidate == 2);
        }
    }
}
