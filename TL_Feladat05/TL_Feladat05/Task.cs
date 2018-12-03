using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TL_Feladat05
{
    class Task
    {
        public int Id, colorID;

        public int Colour;

        public static int MAXCOLOUR = Program.MAXCOLOR;
        public static List<int> nextNumberByColour = new List<int>();
        private static int NEXTID = 0;
        public static int minFib = 15;
        public static int maxFib = minFib + 20;

        public static void init()
        {
            for(int i = 0; i < MAXCOLOUR; i++)
            {
                nextNumberByColour.Add(0);
            }
        }

        public Task(int Colour)
        {
            this.Id = NEXTID;
            NEXTID++;
            this.Colour = Colour;
            this.colorID = nextNumberByColour[Colour];
            nextNumberByColour[Colour]++;
        }

        public void run()
        {
            if (Scheduler.log)
                Console.WriteLine("Task: " + Id + " started executing");
            long result = Fibonacci(new Random().Next(minFib, maxFib));
            if(result <= -4)
                Console.WriteLine("A generált szám: " + result + " < -4\tTask ID: " + Id);
            if (Scheduler.log)
                Console.WriteLine("Task: " + Id + " finished executing");
        }

        private long Fibonacci(int n)
        {
            if (n == 0)
                return 0;
            else if (n == 1)
                return 1;
            else
                return Fibonacci(n - 1) + Fibonacci(n - 2);
        }

    }
}
