using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading;

namespace Lab4.Task4
{
    public class BarberShop
    {
        public int TakenSeats;
        public int MaxClientsCount { get; }
        public Queue<Client> Clients { get; }
        public Semaphore ClientsSemaphore { get; }
        public Semaphore BarberSemaphore { get; }
        public Semaphore SeatsSemaphore { get; }

        public BarberShop(int maxClientsCount)
        {
            TakenSeats = 0;
            MaxClientsCount = maxClientsCount;
            Clients = new Queue<Client>(maxClientsCount);
            ClientsSemaphore = new Semaphore(0, maxClientsCount);
            BarberSemaphore = new Semaphore(0, 1);
            SeatsSemaphore = new Semaphore(1, 1);
        }
    }
}
