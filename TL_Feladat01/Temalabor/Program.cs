using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Temalabor
{
    class Program
    {
        static void Main(string[] args)
        {
            int size = int.Parse(args[0]);
            Matrix a = new Matrix(size);
            Matrix b = new Matrix(size);
            var watch = new Stopwatch();
            watch.Start();
            a.Multiply01(b);
            watch.Stop();
            double elapsed = watch.ElapsedMilliseconds;
            Console.WriteLine(elapsed + "ms");
            Console.ReadKey();
        }
    }

    class Matrix
    {
        private int[,] data;
        private int size;

        public Matrix Multiply01(Matrix b)
        {
            int size = this.size;
            Matrix m = new Matrix(size, 0);
            for(int i = 0; i < size; i++)
            {
                for(int j = 0; j < size; j++)
                {
                    for( int k = 0; k < size; k++)
                    {
                        m.data[i,j] += this.data[i,k] * b.data[k,j];
                    }
                }
            }
            return m;
        }

        public Matrix(int size)
        {
            this.size = size;
            data = new int[size, size];
            var rnd =new Random();
            for (int i = 0; i < size; i++)
            {
                for(int j = 0; j < size; j++)
                {
                    data[i,j] = rnd.Next();
                }
            }
        }

        public Matrix(int size, int number)
        {
            this.size = size;
            data = new int[size, size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    data[i,j] = number;
                }
            }
        }

    }
}
