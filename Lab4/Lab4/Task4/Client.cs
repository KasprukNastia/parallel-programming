using System;
using System.Threading;
using System.Threading.Tasks;

namespace Lab4.Task4
{
    public class Client
    {
        private readonly BarberShop _barberShop;
        public CancellationTokenSource CancellationTokenSource { get; }

        public Client(BarberShop barberShop)
        {
            _barberShop = barberShop ?? throw new ArgumentNullException(nameof(barberShop));
            CancellationTokenSource = new CancellationTokenSource();
        }

        public Task Run()
        {
            return Task.Run(() =>
            {
                var random = new Random();
                // Run in an infinite loop to simulate multiple customers.
                while (!CancellationTokenSource.IsCancellationRequested)
                {
                    // Try to get access to the waiting room chairs.
                    _barberShop.SeatsSemaphore.WaitOne();
                    // If there are any free seats:
                    if (_barberShop.TakenSeats < _barberShop.MaxClientsCount)
                    {
                        // sit down in a chair
                        _barberShop.TakenSeats++;
                        // notify the barber, who's waiting until there is a customer
                        _barberShop.ClientsSemaphore.Release();
                        // don't need to lock the chairs anymore
                        _barberShop.SeatsSemaphore.Release();
                        // wait until the barber is ready
                        _barberShop.BarberSemaphore.WaitOne();

                        // Have hair cut here.
                        int haircutDuretionMs = random.Next(100, 1000);
                        Console.WriteLine($"Client '{Guid.NewGuid()}' has hair cutted for {haircutDuretionMs} ms");
                        Thread.Sleep(haircutDuretionMs);
                    }
                    // otherwise, there are no free seats; tough luck --
                    else
                    {
                        // but don't forget to release the lock on the seats!
                        _barberShop.SeatsSemaphore.Release();
                    }
                }
            }, CancellationTokenSource.Token);
        }
    }

}
