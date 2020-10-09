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
                _barberShop.SeatsSemaphore.WaitOne();
                if (_barberShop.TakenSeats < _barberShop.MaxClientsCount)
                {
                    Console.WriteLine($"The number of taken seats is {_barberShop.TakenSeats} so new client with id '{ClientId}' WILL be added to the queue");
                    
                    _barberShop.ClientsSemaphore.WaitOne();
                    _barberShop.Clients.Enqueue(this);
                    _barberShop.TakenSeats++;
                    _barberShop.ClientsSemaphore.Release();

                    if (_barberShop.Clients.Count == 1)
                    {
                        _barberShop.SeatsSemaphore.Release();
                        _barberShop.BarberSemaphore.WaitOne();
                    }
                    else
                    {
                        _barberShop.SeatsSemaphore.Release();
                    }          
                }
                else
                {
                    Console.WriteLine($"Queue is filled. The number of taken seats is {_barberShop.TakenSeats} so new client with id '{ClientId}' WILL NOT be added to the queue");
                    _barberShop.SeatsSemaphore.Release();
                }
            });
        }
    }

}
