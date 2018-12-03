using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TL_Feladat05
{
    

    class ThreadStateChecker
    {
        private List<Thread> threads;
        private Scheduler scheduler;
        private static int checkTime = 100;

        public ThreadStateChecker(List<Thread> threads, Scheduler scheduler)
        {
            this.threads = threads;
            this.scheduler = scheduler;
            Thread thread = new Thread(Check);
        }

        private void Check()
        {
            while (true)
            {
                if (threads.Count > 0)
                {
                    foreach (Thread t in threads)
                    {
                        if (!t.IsAlive)
                        {
                            scheduler.ThreadDied(t);
                        }
                    }
                }
                Thread.Sleep(checkTime);
            }

        }
    }
}
