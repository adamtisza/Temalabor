using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace TL_Feladat05
{
    class Consumer
    {
        private Task task;
        private Thread thread;
        private Scheduler scheduler;

        public Thread Thread{
            get { return thread; }
        }

        public Consumer(Scheduler scheduler)
        {
            this.scheduler = scheduler;
        }

        public Task Task
        {
            get{ return task; }
        }

        public Thread AcceptTask(Task task)
        {
            if (this.task != null)
            {
                return null;
            }
            else
            {
                this.task = task;
                thread = new Thread(new ThreadStart(RunTask));
                thread.Start();
                return thread;
            }
        }

        public void RunTask()
        {
            task.run();
            scheduler.ConsumerReady(this);
            dropTask();
        }

        public void dropTask()
        {
            task = null;
        }


    }
}
