using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TL_Feladat05
{
    class Producer
    {
        private Scheduler scheduler;
        private int pauseTimeInMilis;
        private bool enabled;

        public Producer(Scheduler scheduler, int pauseInMilis)
        {
            this.scheduler = scheduler;
            this.pauseTimeInMilis = pauseInMilis;
            enabled = false;
            Thread thread = new Thread(run);
            thread.Start();
        }

        public void Enable()
        {
            enabled = true;
            if (Scheduler.log)
                Console.WriteLine("Producer enabled");
        }
        public void Disable()
        {
            enabled = false;
        }

        public void run()
        {
            while (true)
            {
                if (enabled)
                {
                    int taskColor = new Random().Next(Task.MAXCOLOUR);
                    Task task = new Task(taskColor);
                    if (Scheduler.log)
                        Console.WriteLine("Task created with id: " + task.Id);
                    scheduler.Push(task);
                }

                Thread.Sleep(pauseTimeInMilis);
            }
        }
    }
}
