using Lab4.Task1;
using Lab4.Task2;
using Lab4.Task3;
using Lab4.Task4;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab4
{
    class Program
    {
        static void Main(string[] args)
        {
            RunTask4(5);
        }

        // Task 1: Producer-consumer
        public static void RunTask1()
        {
            var collection = new Collection(maxSize: 5);
            var prodecer = new Producer(collection);
            var consumer = new Consumer(collection);

            var producerTask = prodecer.Produce();
            var consumerTask = consumer.Consume();

            Task.WaitAll(producerTask, consumerTask);
        }

        // Task 2: Readers/Writers
        public static void RunTask2(int readredsCount, int writersCount)
        {
            IEnumerable<Writer> writers = 
                Enumerable.Range(1, writersCount + 1).Select(i => new Writer(i));
            IEnumerable<Reader> readers = 
                Enumerable.Range(1, readredsCount + 1).Select(i => new Reader(i, Writer.WriterSemaphore));

            Task[] readersWritersTasks = readers.Select(r => r.Read()).Concat(writers.Select(w => w.Write())).ToArray();

            Task.WaitAll(readersWritersTasks);
        }

        // Task 3: Dining Philosophers
        public static void RunTask3()
        {
            int numberOfForksAndPhilosophers = 5;
            Philosopher[] philosophers = new Philosopher[numberOfForksAndPhilosophers];
            Task[] philosophersTasks = new Task[numberOfForksAndPhilosophers];
            Fork[] forks = new Fork[numberOfForksAndPhilosophers];

            for (int i = 0; i < numberOfForksAndPhilosophers; i++)
                forks[i] = new Fork();

            Fork leftFork, rightFork;
            Task philosopherTask;
            for (int i = 0; i < numberOfForksAndPhilosophers; i++)
            {
                leftFork = forks[i];
                rightFork = forks[(i + 1) % numberOfForksAndPhilosophers];

                if (i == numberOfForksAndPhilosophers - 1)
                {
                    philosophers[i] = new Philosopher(i + 1, rightFork, leftFork);
                }
                else
                {
                    philosophers[i] = new Philosopher(i + 1, leftFork, rightFork);
                }

                philosopherTask = philosophers[i].Run();
                philosophersTasks[i] = philosopherTask;
            }

            Task.WaitAll(philosophersTasks);
        }

        // Task 4: Sleeping Barber
        public static void RunTask4(int maxClientsCount)
        {
            var barberShop = new BarberShop(maxClientsCount);
            var barber = new Barber(barberShop);

            List<Task> allTasks = new List<Task>(maxClientsCount * 3 + 1);
            allTasks.Add(barber.Run());
            Client client;
            for(int i = 0; i < maxClientsCount * 3; i++)
            {
                client = new Client(i, barberShop);
                allTasks.Add(client.Run());
            }

            Task.WaitAll(allTasks.ToArray());
        }
    }
}
