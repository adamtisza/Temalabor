using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace TL_Feladat05
{
    class Scheduler
    {

        Dictionary<int, Queue<Task>> taskQueues;
        List<int> enabledColors;
        List<Producer> producers;
        List<Consumer> consumers;
        List<Consumer> readyConsumers;
        List<Task> failedTasks;
        List<Task> completedTasks;
        List<Thread> threads;
        ThreadStateChecker ThreadStateChecker;
        int lastLoggedTaskCount;
        readonly Object locker;
        bool enabled;
        public static bool log = false;
        public static bool finishLog = false;
        public static bool performanceLog = true;
        public const int logTime = 2000;
        public const int produceTime =20;




        public Scheduler(int consumerCount)
        {
            //init queues
            if (log)
                Console.WriteLine("Initializing queues...");
            taskQueues = new Dictionary<int, Queue<Task>>();
            for(int i = 0; i < Task.MAXCOLOUR; i++)
            {
                taskQueues.Add(i, new Queue<Task>());
            }

            Task.init();
            locker = new Object();
            enabled = true;
            ThreadStateChecker = new ThreadStateChecker(threads, this);

            //init Lists
            if (log)
                Console.WriteLine("Initializing components...");
            enabledColors = new List<int>();
            producers = new List<Producer>();
            consumers = new List<Consumer>();
            readyConsumers = new List<Consumer>();
            failedTasks = new List<Task>();
            completedTasks = new List<Task>();
            threads = new List<Thread>();

            //create producer(s)
            if (log)
                Console.WriteLine("Creating producers...");
            producers.Add(new Producer(this, produceTime));

            //create consumers
            if (log)
                Console.WriteLine("Creating consumers...");
            for(int i = 0; i < consumerCount; i++)
            {
                consumers.Add(new Consumer(this));
            }
            //at start, all consumers are ready
            readyConsumers.AddRange(consumers);
            //at start, all colors are enabled
            for (int i = 0; i < Task.MAXCOLOUR; i++)
            {
                enabledColors.Add(i);
            }
            //Enable producers start running
            if (log)
                Console.WriteLine("Enabling producers...");
            foreach(Producer producer in producers)
            {
                producer.Enable();
            }

            if (performanceLog) {
                Console.WriteLine("Consumer count = " + Program.CONSUMERCOUNT + ", Colors count = " + Task.MAXCOLOUR + ", Sampling frequency = 1/"+ logTime/1000 + "s, n range: " + Task.minFib +  "-" + Task.maxFib + "  (Fibonacci(n))");
                //init timer
                var aTimer = new System.Timers.Timer(logTime);
                // Hook up the Elapsed event for the timer. 
                aTimer.Elapsed += logPerformance;
                aTimer.AutoReset = true;
                aTimer.Enabled = true;
            }

            if(log)
                Console.WriteLine("Starting scheduler...\n\n");
            Thread thread = new Thread(Run);
            thread.Start();

        }

        public void logPerformance(Object source, ElapsedEventArgs e)
        {
            int completedCount = completedTasks.Count - lastLoggedTaskCount;
            lastLoggedTaskCount = completedTasks.Count;
            Console.WriteLine(completedCount + " tasks completed in " + logTime/1000 + "s");
        }

        // When the death of a thread is detected the owner consumer is freed, the task it was runnig is sent to the failed tasks' list
        public void ThreadDied(Thread thread)
        {
            lock (locker)
            {
                foreach (Consumer c in consumers)
                {
                    if (c.Thread.Equals(thread))
                    {
                        c.dropTask();
                        readyConsumers.Add(c);
                        failedTasks.Add(c.Task);
                        enabledColors.Add(c.Task.Colour);
                    }
                }
                threads.Remove(thread);
            }
        }

        //a task is pushed into the scheduler by a producer
        public void Push(Task task)
        {
            lock (locker)
            {
                Queue<Task> queue;
                queue = taskQueues[task.Colour];
                queue.Enqueue(task);
            }
        }

        //a consumer finished its task, and is ready
        public void ConsumerReady(Consumer consumer)
        {
            lock (locker)
            {
                readyConsumers.Add(consumer);
                completedTasks.Add(consumer.Task);
                enabledColors.Add(consumer.Task.Colour);
                threads.Remove(consumer.Thread);
                if(log)
                    Console.WriteLine("Task completed: " + consumer.Task.Id);
            }

        }

        //the scheduler constantly checks queues for waiting tasks, and gives them to free consumers
        public void Run()
        {
            while (enabled)
            {
                lock (locker)
                {
                    for(int i = 0; i < readyConsumers.Count; i++)
                    {
                        Consumer consumer = readyConsumers[i];
                        Task task;
                        Queue<Task> queue;
                        for(int j = 0; j < enabledColors.Count; j++)
                        {
                            int color = enabledColors[j];
                            queue = taskQueues[color];
                            if (queue.Count > 0)
                            {
                                //task átadása consumer-nek, task színének letiltása, consumer készenléti állapotból kivétele
                                task = queue.Dequeue();
                                    threads.Add(consumer.AcceptTask(task));
                                if (log||finishLog)
                                    Console.WriteLine("Task: (Id = " + task.Id + " Color = " + task.Colour + " Number by its colour = " + task.colorID + ") sent to consumer");
                                enabledColors.Remove(task.Colour);
                                readyConsumers.Remove(consumer);
                                break;
                            }
                        }

                    }
                }
                //rövid sleep hogy ne tartsuk folyamatosan lock alatt az ütemezőt, mert akkor a producer-ek és a consumerek nem tudnak dolgozni
                Thread.Sleep(10);
            }
        }
    }


}
