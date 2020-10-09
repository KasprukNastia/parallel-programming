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
                    // Try to acquire a customer - if none is available, go to sleep.
                    _barberShop.ClientsSemaphore.WaitOne();
                    // Awake - try to get access to modify # of available seats, otherwise sleep.
                    _barberShop.SeatsSemaphore.WaitOne();
                    // One waiting room chair becomes free.
                    _barberShop.TakenSeats--;
                    // I am ready to cut.
                    _barberShop.BarberSemaphore.Release();
                    // Don't need the lock on the chairs anymore.
                    _barberShop.SeatsSemaphore.Release();

                    // Cut hair here.
                    Console.WriteLine($"{DateTime.Now.ToString("hh:mm:ss.fff")}: Barber serves the client");
                }
            }, CancellationTokenSource.Token);
        }
    }
}
