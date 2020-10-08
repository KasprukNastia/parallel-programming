using System;
using System.Threading;
using System.Threading.Tasks;

namespace Lab4.Task2
{
    public class Writer
    {
        public static Semaphore WriterSemaphore { get; } = new Semaphore(1, 1);

        public int Id { get; }
        public CancellationTokenSource CancellationTokenSource { get; }

        public Writer(int writerId)
        {
            Id = writerId;
            CancellationTokenSource = new CancellationTokenSource();
        }

        public Task Write()
        {
            return Task.Run(() =>
            {
                WriterSemaphore.WaitOne();

                int writingDirationMs = new Random().Next(1000, 5000);
                Console.WriteLine($"{DateTime.Now.ToString("hh:mm:ss.fff")}: Writer '{Id}' writes for {writingDirationMs} ms");
                Thread.Sleep(TimeSpan.FromMilliseconds(writingDirationMs));

                WriterSemaphore.Release();
            }, CancellationTokenSource.Token);
        }
    }
}
