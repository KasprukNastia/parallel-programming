using System;
using System.Diagnostics;

namespace Lab1
{
    class Program
    {
        static void Main(string[] args)
        {
            var matrix = new Matrix(20000);
            
            var sw = Stopwatch.StartNew();
            matrix.CalcMainDiagonal();
            sw.Stop();
            Console.WriteLine($"Simple matrix main diagonal calculation: {sw.ElapsedMilliseconds} ms");
            //Console.WriteLine(matrix.ToString());
            Console.WriteLine();

            sw = Stopwatch.StartNew();
            matrix.CalcMainDiagonalAsParallelOldFashioned();
            sw.Stop();
            Console.WriteLine($"Old fashioned parallel matrix main diagonal calculation: {sw.ElapsedMilliseconds} ms");
            //Console.WriteLine(matrix.ToString());
            Console.WriteLine();

            sw = Stopwatch.StartNew();
            matrix.CalcMainDiagonalAsParallelNewStyleAsync().Wait();
            sw.Stop();
            Console.WriteLine($"New style parallel matrix main diagonal calculation: {sw.ElapsedMilliseconds} ms");
            //Console.WriteLine(matrix.ToString());
            Console.WriteLine();
        }
    }
}
