using System;
using System.Threading.Tasks;

namespace Lab4.Task4
{
    public class Client
    {
        private readonly BarberShop _barberShop;
        public int ClientId { get; }
        public TimeSpan HaircutDuration { get; private set; }

        public Client(int clientId, BarberShop barberShop)
        {
            ClientId = clientId;
            _barberShop = barberShop ?? throw new ArgumentNullException(nameof(barberShop));
            HaircutDuration = TimeSpan.FromMilliseconds(new Random().Next(1000, 3000));
        }

        public Task Run()
        {
            return Task.Run(() =>
            {
                _barberShop.ClientsSemaphore.WaitOne();
                if (_barberShop.Clients.Count < _barberShop.MaxClientsCount)
                {
                    Console.WriteLine($"The number of taken seats is {_barberShop.Clients.Count} so new client with id '{ClientId}' WILL be added to the queue");       
                    _barberShop.Clients.Enqueue(this);

                    if (_barberShop.Clients.Count == 1)
                        _barberShop.BarberSemaphore.Release();
                }
                else
                {
                    Console.WriteLine($"Queue is filled. The number of taken seats is {_barberShop.Clients.Count} so new client with id '{ClientId}' WILL NOT be added to the queue");
                }
                _barberShop.ClientsSemaphore.Release();
            });
        }
    }

}
