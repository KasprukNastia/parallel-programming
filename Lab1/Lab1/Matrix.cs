using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lab1
{
    class Matrix
    {
        private readonly int _dimension;
        private readonly int[,] _matrix;

        public Matrix(int dimension)
        {
            _dimension = dimension;
            _matrix = new int[dimension, dimension];

            var random = new Random();
            for(int i = 0; i < dimension; i++)
            {
                for(int j = 0; j < dimension; j++)
                {
                    if (i == j) continue;
                    _matrix[i, j] = random.Next(0, 100);
                }
            }
        }

        public Matrix(int[,] squareMatrix)
        {
            if(squareMatrix == null)
                throw new ArgumentNullException(nameof(squareMatrix));
            if (squareMatrix.Rank != 2 || squareMatrix.GetLength(0) != squareMatrix.GetLength(1))
                throw new ArgumentException("Matrix is not square matrix");
            _matrix = squareMatrix;
        }

        public override string ToString()
        {
            var strBld = new StringBuilder();
            for (int i = 0; i < _dimension; i++)
            {
                for (int j = 0; j < _dimension; j++)
                {
                    strBld.Append($"{_matrix[i, j]}\t");
                }
                strBld.AppendLine();
            }
            return strBld.ToString();
        }

        // One-threaded execution
        public void CalcMainDiagonal()
        {
            for (int i = 0; i < _dimension; i++)
            {
                CalcDiagonalValue(i);
            }
        }

        // Multi-threaded execution (old style)
        public void CalcMainDiagonalAsParallelOldFashioned()
        {
            List<Thread> threads = new List<Thread>(_dimension);

            Thread thread;
            for (int i = 0; i < _dimension; i++)
            {
                int loc = i;
                thread = new Thread(() => CalcDiagonalValue(loc));
                thread.Start();
                threads.Add(thread);
            }
            threads.ForEach(t => t.Join());
        }

        // Multi-threaded execution (new style)
        public async Task CalcMainDiagonalAsParallelNewStyleAsync()
        {
            List<Task> tasks = new List<Task>(_dimension);

            for (int i = 0; i < _dimension; i++)
            {
                int loc = i;
                tasks.Add(Task.Factory.StartNew(() => CalcDiagonalValue(loc)));
            }

            await Task.WhenAll(tasks);
        }

        private void CalcDiagonalValue(int index)
        {
            for (int j = 0; j < _dimension; j++)
            {
                if (index == j) continue;
                _matrix[index, index] += _matrix[index, j];
                _matrix[index, index] += _matrix[j, index];
            }
        }
    }
}
