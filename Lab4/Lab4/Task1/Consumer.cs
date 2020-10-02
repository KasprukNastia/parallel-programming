using System;
using System.Threading;
using System.Threading.Tasks;

namespace Lab4.Task1
{
    public class Consumer
    {
        private readonly Collection _consumptionCollection;
        public CancellationTokenSource CancellationTokenSource { get; }

        public Consumer(Collection consumptionCollection)
        {
            _consumptionCollection = consumptionCollection ?? throw new ArgumentNullException(nameof(consumptionCollection));
            CancellationTokenSource = new CancellationTokenSource();
        }

        public Task Consume()
        {
            return Task.Run(() =>
            {
                var random = new Random();
                Item item;
                while (!CancellationTokenSource.IsCancellationRequested)
                {
                    item = _consumptionCollection.Remove();

                    Console.WriteLine($"Consumer consumes the item with the id = '{item.Id}'");
                    Console.WriteLine();

                    Thread.Sleep(TimeSpan.FromMilliseconds(random.Next(100, 1000)));
                }
            }, CancellationTokenSource.Token);
        }
    }
}
