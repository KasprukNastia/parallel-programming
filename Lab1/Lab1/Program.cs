using System;
using System.Diagnostics;

namespace Lab1
{
    class Program
    {
        static void Main(string[] args)
        {
            int dimension = 20000;
            var matrix = new Matrix(dimension);
            
            var swOneThread = Stopwatch.StartNew();
            matrix.CalcMainDiagonal();
            swOneThread.Stop();
            Console.WriteLine(
                $"One thread matrix main diagonal calculation time: {swOneThread.ElapsedMilliseconds} ms");
            Console.WriteLine();

            var swMultipleThreads = Stopwatch.StartNew();
            matrix.CalcMainDiagonalAsParallelOldFashioned();
            swMultipleThreads.Stop();
            Console.WriteLine(
                $"Multiple threads matrix main diagonal calculation time (old fashioned parallel programming style): {swMultipleThreads.ElapsedMilliseconds} ms");
            double accelerationFactorOld = (double)swOneThread.ElapsedMilliseconds / swMultipleThreads.ElapsedMilliseconds;
            Console.WriteLine(
                $"Acceleration factor (Коефiцiєнт прискорення) for old fashioned parallel programming style: {accelerationFactorOld}");
            Console.WriteLine($"Algorithm efficiency (Ефективнiсть алгоритму): {accelerationFactorOld / dimension}");
            Console.WriteLine();

            var swMultipleThreadsTask = Stopwatch.StartNew();
            matrix.CalcMainDiagonalAsParallelNewStyleAsync().Wait();
            swMultipleThreadsTask.Stop();
            Console.WriteLine(
                $"Multiple threads matrix main diagonal calculation time (new style of parallel programming): {swMultipleThreadsTask.ElapsedMilliseconds} ms");
            double accelerationFactorNew = (double)swOneThread.ElapsedMilliseconds / swMultipleThreadsTask.ElapsedMilliseconds;
            Console.WriteLine(
                $"Acceleration factor (Коефiцiєнт прискорення) for new style of parallel programming: {accelerationFactorNew}");
            Console.WriteLine($"Algorithm efficiency (Ефективнiсть алгоритму): {accelerationFactorNew / dimension}");
            Console.WriteLine();
        }
    }
}
