using Lab4.Task1;
using Lab4.Task3;
using System.Threading.Tasks;

namespace Lab4
{
    class Program
    {
        static void Main(string[] args)
        {
            RunTask3();
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
    }
}
