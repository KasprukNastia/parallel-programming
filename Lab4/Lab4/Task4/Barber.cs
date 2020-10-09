using System;
using System.Threading;
using System.Threading.Tasks;

namespace Lab4.Task4
{
    public class Barber
    {
        private readonly BarberShop _barberShop;
        public CancellationTokenSource CancellationTokenSource { get; }

        public Barber(BarberShop barberShop)
        {
            _barberShop = barberShop ?? throw new ArgumentNullException(nameof(barberShop));
            CancellationTokenSource = new CancellationTokenSource();
        }

        public Task Run()
        {
            return Task.Run(() =>
            {
                while (!CancellationTokenSource.IsCancellationRequested)
                {
                    _barberShop.SeatsSemaphore.WaitOne();
                    if (_barberShop.TakenSeats == 0)
                    {
                        _barberShop.SeatsSemaphore.Release();
                        _barberShop.BarberSemaphore.Release();
                        _barberShop.SeatsSemaphore.WaitOne();
                    }

                    _barberShop.ClientsSemaphore.WaitOne();
                    Client client = _barberShop.Clients.Dequeue();
                    _barberShop.TakenSeats--;
                    Console.WriteLine($"{DateTime.Now.ToString("hh:mm:ss.fff")}: Barber serves the client with id '{client.ClientId}' for {client.HaircutDuration} ms");
                    Thread.Sleep(client.HaircutDuration);
                    _barberShop.ClientsSemaphore.Release();
                    _barberShop.SeatsSemaphore.Release();
                }
            }, CancellationTokenSource.Token);
        }
    }
}
