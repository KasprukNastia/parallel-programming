using Lab4.Task1;
using System.Threading;
using System.Threading.Tasks;

namespace Lab4
{
    class Program
    {
        static void Main(string[] args)
        {
            RunTask1();
        }

        // Task 1 : Producer-consumer
        public static void RunTask1()
        {
            var collection = new Collection(maxSize: 5);

            var producerCancelationTokenSource = new CancellationTokenSource();
            var prodecer = new Producer(collection, producerCancelationTokenSource);

            var consumerCancelationTokenSource = new CancellationTokenSource();
            var consumer = new Consumer(collection, consumerCancelationTokenSource);

            var producerTask = prodecer.Produce();
            var consumerTask = consumer.Consume();

            Task.WaitAll(producerTask, consumerTask);
        }
    }
}
