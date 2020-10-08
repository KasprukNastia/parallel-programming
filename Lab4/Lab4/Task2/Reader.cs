using System;
using System.Threading;
using System.Threading.Tasks;

namespace Lab4.Task2
{
    public class Reader
    {
        private static long _readersCount = 0;
        public static Semaphore ReaderSemaphore { get; } = new Semaphore(1, 1);

        private readonly Semaphore _writerSemaphore;

        public int Id { get; }
        public CancellationTokenSource CancellationTokenSource { get; }

        public Reader(int readerId, Semaphore writerSemaphore)
        {
            _writerSemaphore = writerSemaphore ?? throw new ArgumentNullException(nameof(writerSemaphore));
            Id = readerId;
            CancellationTokenSource = new CancellationTokenSource();
        }

        public Task Read()
        {
            return Task.Run(() =>
            {
                ReaderSemaphore.WaitOne();
                _readersCount++;
                if (_readersCount == 1)
                    _writerSemaphore.WaitOne();
                ReaderSemaphore.Release();

                int readingDirationMs = new Random().Next(1000, 3000);
                Console.WriteLine($"{DateTime.Now.ToString("hh:mm:ss.fff")}: Reader '{Id}' reads for {readingDirationMs} ms");
                Thread.Sleep(TimeSpan.FromMilliseconds(readingDirationMs));

                ReaderSemaphore.WaitOne();
                _readersCount--;
                if (_readersCount == 0)
                    _writerSemaphore.Release();
                ReaderSemaphore.Release();

            }, CancellationTokenSource.Token);
        }
    }
}
