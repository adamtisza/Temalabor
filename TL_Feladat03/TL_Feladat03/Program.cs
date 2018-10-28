using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TL_Feladat03
{
    class Program
    {
        static void Main(string[] args)
        {
            int size = int.Parse(args[0]);
            int[] block = generateBlock(size);     
            var watch = new Stopwatch();
            int[] dstBlock = new int[size];
            const int minBlockSize = 10;
            for(int i = 0; i < block.Length; i++)
            {
                dstBlock[i] = block[i];
            }  
            
            switch (args[1])
            {
                case "1":
                    watch.Start();
                    quickSort(block, 0, size - 1); 
                    break;
                case "2":
                    watch.Start();
                    ParalellMergeSort(block, 0, size - 1, dstBlock, minBlockSize);
                    break;
                default:
                    break;
            }
            watch.Stop();
            double elapsed = watch.ElapsedMilliseconds;

            

            Console.WriteLine(elapsed + "ms");
            Console.ReadKey();

            
        }



        static int[] generateBlock(int size)
        {
            int[] block = new int[size];
            Random rnd = new Random();

            for(int i = 0; i < size; i++)
            {
                block[i] = rnd.Next();
            }
            return block;

        }


        static int partition(int[] arr, int left, int right)
        {
            int i = left, j = right;
            int tmp;
            int pivot = arr[(left + right) / 2];

            while (i <= j)
            {

                while (arr[i] < pivot)
                    i++;
                while (arr[j] > pivot)
                    j--;
                if (i <= j)
                {
                    tmp = arr[i];
                    arr[i] = arr[j];
                    arr[j] = tmp;
                    i++;
                    j--;
                }
            };
            return i;
        }

        static void quickSort(int[] arr, int left, int right)
        {
            int index = partition(arr, left, right);
            if (left < index - 1)
                quickSort(arr, left, index - 1);
            if (index < right)
                quickSort(arr, index, right);
        }

        static void ParalellMergeSort(int[] src, int left, int right, int[] dst, int minTaskSize, bool srcToDst = true)
        {
            if (right == left)
            {
                if (srcToDst) dst[left] = src[left];
                return;
            }
            int mid = (right + left) / 2;

            if(right - left >= minTaskSize)
            {
                Task taskLeft = Task.Run(() => ParalellMergeSort(src, left, mid, dst, minTaskSize, !srcToDst));
                Task taskRight = Task.Run(() => ParalellMergeSort(src, mid + 1, right, dst, minTaskSize, !srcToDst));

                taskLeft.Wait();
                taskRight.Wait();
            }
            else
            {
                ParalellMergeSort(src, left, mid, dst, minTaskSize, !srcToDst);
                ParalellMergeSort(src, mid + 1, right, dst, minTaskSize, !srcToDst);
            }

            if (srcToDst)
                Merge(src, dst, left, mid, mid + 1, right, left);
            else
                Merge(dst, src, left, mid, mid + 1, right, left);


        }

        private static void Merge(int[] to, int[] temp, int lowX, int highX, int lowY, int highY, int lowTo)
            {
                var highTo = lowTo + highX - lowX + highY - lowY + 1;
                for (; lowTo <= highTo; lowTo++)
                {
                    if (lowX > highX)
                        to[lowTo] = temp[lowY++];
                    else if (lowY > highY)
                        to[lowTo] = temp[lowX++];
                    else
                        to[lowTo] = (temp[lowX] < temp[lowY])
                                        ? temp[lowX++]
                                        : temp[lowY++];
                }
            }
    }
}
