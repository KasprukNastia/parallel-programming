using System;
using System.Threading;
using System.Threading.Tasks;

namespace Lab4.Task1
{
    public class Producer
    {
        private readonly Collection _consumptionCollection;
        public CancellationTokenSource CancellationTokenSource { get; }

        public Producer(Collection consumptionCollection)
        {
            _consumptionCollection = consumptionCollection ?? throw new ArgumentNullException(nameof(consumptionCollection));
            CancellationTokenSource = new CancellationTokenSource();
        }

        public Task Produce()
        {
            return Task.Run(() =>
            {
                var random = new Random();
                Item item;
                while (!CancellationTokenSource.IsCancellationRequested)
                {
                    item = new Item { Id = Guid.NewGuid() };

                    Console.WriteLine($"Producer produces the item with the id = '{item.Id}'");
                    Console.WriteLine();

                    _consumptionCollection.Add(item);

                    Thread.Sleep(TimeSpan.FromMilliseconds(random.Next(100, 1000)));
                }
            }, CancellationTokenSource.Token);
        }
    }
}
