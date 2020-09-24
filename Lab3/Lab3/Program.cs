using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Lab3
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }

        // Паралельна функція для знаходження кількості елементів за умовою
        public static long CalcElementsOnCondition(IEnumerable<long> elementsList, Func<long, bool> selector)
        {
            long elementsCount = 0;

            elementsList.AsParallel().ForAll(e =>
            {
                if(selector(e))
                    Interlocked.Increment(ref elementsCount);
            });

            return elementsCount;
        }

        // Паралельна функція для знаходження мінімального та максимального елементів з індексами
        public static (long min, long minIndex, long max, long maxIndex) CalcMinAndMaxWithIndices(IEnumerable<long> elementsList)
        {
            long min = 0, minIndex = 0, max = 0, maxIndex = 0;

            elementsList.AsParallel().Select((elem, index) =>
            {
                if(elem < Interlocked.Read(ref min))
                {
                    Interlocked.Exchange(ref min, elem);
                    Interlocked.Exchange(ref minIndex, index);
                }
                if(elem > Interlocked.Read(ref max))
                {
                    Interlocked.Exchange(ref max, elem);
                    Interlocked.Exchange(ref maxIndex, index);
                }

                return elem;
            });

            return (min, minIndex, max, maxIndex);
        }

        // Паралельна функція для знаходження контрольної суми із використанням XOR
        public static long CalcControlSum(IEnumerable<long> elementsList)
        {
            long controlSum = 0;

            elementsList.AsParallel().ForAll(e =>
            {
                long curSumValue = Interlocked.Read(ref controlSum);
                Interlocked.Exchange(ref controlSum, curSumValue ^ e);
            });

            return controlSum;
        }
    }
}
