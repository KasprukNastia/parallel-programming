using System;
using System.Threading;
using System.Threading.Tasks;

namespace Lab4.Task3
{
    public class Philosopher
    {
        private readonly int _philosopherId;
        private readonly Fork _leftFork;
        private readonly Fork _rightFork;

        public CancellationTokenSource CancellationTokenSource { get; }

        public Philosopher(int philosopherId, Fork leftFork, Fork rightFork)
        {
            _philosopherId = philosopherId;
            _leftFork = leftFork ?? throw new ArgumentNullException(nameof(leftFork));
            _rightFork = rightFork ?? throw new ArgumentNullException(nameof(rightFork));
            CancellationTokenSource = new CancellationTokenSource();
        }

        public Task Run()
        {
            return Task.Run(() =>
            {
                DoAction($"{DateTime.Now.ToString("hh:mm:ss.fff")}: Thinking");
                lock (_leftFork)
                {
                    DoAction($"{DateTime.Now.ToString("hh:mm:ss.fff")}: Picked up left fork");
                    lock (_rightFork)
                    {
                        DoAction($"{DateTime.Now.ToString("hh:mm:ss.fff")}: Picked up right fork - eating");
                        DoAction($"{DateTime.Now.ToString("hh:mm:ss.fff")}: Put down right fork");
                    }
                    DoAction($"{DateTime.Now.ToString("hh:mm:ss.fff")}: Put down left fork. Back to thinking");
                }
            }, CancellationTokenSource.Token);
        }

        private void DoAction(string action)
        {
            Console.WriteLine($"{_philosopherId} {action}");
            Thread.Sleep(TimeSpan.FromMilliseconds(new Random().Next(100, 1000)));
        }
    }
}
