using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Temalabor
{
    class ThreadInfo
    {
        public Matrix b, m;
        public int startRow, rowCount;
        public CountdownEvent countdownEvent;
        public ThreadInfo(Matrix b, Matrix m, int startRow, int rowCount, CountdownEvent countdownEvent)
        {
            this.b = b;
            this.m = m;
            this.startRow = startRow;
            this.rowCount = rowCount;
            this.countdownEvent = countdownEvent;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            //string[] command = args[0].Split(' ');
            int size = int.Parse(args[0]);
            Matrix a = new Matrix(size);
            Matrix b = new Matrix(size);
            var watch = new Stopwatch();
            switch (args[1])
            {
                case "1":
                    watch.Start();
                    a.Multiply01(b);
                    break;
                case "2":
                    watch.Start();
                    a.Multiply02(b);
                    break;
                case "3":
                    watch.Start();
                    a.MultiplyThread01(b, int.Parse(args[2]));
                    break;
                case "4":
                    watch.Start();
                    a.MultiplyThread02(b);
                    break;
                default:
                    break;
            }
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

        public Matrix Multiply02(Matrix b)
        {
            int size = this.size;
            Matrix m = new Matrix(size, 0);
            for (int i = 0; i < size; i++)
            {
                for (int k = 0; k < size; k++)
                {
                    for (int j = 0; j < size; j++)
                    {
                        m.data[i, j] += this.data[i, k] * b.data[k, j];
                    }
                }

            }
            return m;
        }


        public Matrix MultiplyThread01(Matrix b, int threadCount)
        {
            int size = this.size;
            Matrix m = new Matrix(size, 0);
            int rowPerThread;
            int lastThreadRows;
            if (b.size % threadCount != 0)
            {
                rowPerThread = b.size / (threadCount - 1);
                lastThreadRows = b.size % (threadCount - 1);
            }
            else
            {
                rowPerThread = b.size / threadCount;
                lastThreadRows = 0;
            }

            using (var countdownEvent = new CountdownEvent(threadCount))
                {
                    for (int i = 0; i < threadCount - 1; i++)
                    {
                        ThreadPool.QueueUserWorkItem(new WaitCallback(MultiplyPart), new ThreadInfo(b, m, i * rowPerThread, rowPerThread, countdownEvent));
                    }
                    ThreadPool.QueueUserWorkItem(new WaitCallback(MultiplyPart), new ThreadInfo(b, m, (threadCount - 1) * rowPerThread, lastThreadRows, countdownEvent));

                    countdownEvent.Wait();
                }
                

                return m;
        }

        public void MultiplyPart(Object tI)
        {
            int size = this.size;
                ThreadInfo threadInfo = tI as ThreadInfo;

            for (int i = 0; i < size; i++)
            {
                for (int k = threadInfo.startRow; k < threadInfo.startRow + threadInfo.rowCount; k++)
                {
                    for (int j = 0; j < size; j++)
                    {
                            threadInfo.m.data[i, j] += this.data[i, k] * threadInfo.b.data[k, j];
                    }
                }
            }
                threadInfo.countdownEvent.Signal();

        }

        public Matrix MultiplyThread02(Matrix b)
        {
            int size = this.size;
            Matrix m = new Matrix(size, 0);

            Parallel.For(0, size, i =>
            {
                for(int j = 0; j < size; j++)
                {
                    for( int k = 0; k < size; k++)
                    {
                        m.data[i,j] += this.data[i,k] * b.data[k,j];
                    }
                }
            });


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
