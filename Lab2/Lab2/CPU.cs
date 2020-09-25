using System;
using System.Threading;

namespace Lab2
{
    public class CPU
    {
        private readonly object _locker = new object();

        private readonly CPUQueue _firstQueue;
        private readonly CPUQueue _secondQueue;

        public bool IsBusy { get; private set; } = false;

        public CPU(CPUQueue firstQueue, CPUQueue secondQueue)
        {
            _firstQueue = firstQueue ?? throw new ArgumentNullException(nameof(firstQueue));
            _secondQueue = secondQueue ?? throw new ArgumentNullException(nameof(secondQueue));
            IsBusy = false;
        }

        public void Run(Process process)
        {
            while (IsBusy)
                continue;

            lock (_locker)
            {
                IsBusy = true;

                Console.WriteLine($"|{DateTime.Now.ToString("dd.MM.yyyy hh:mm:ss.fff tt")}| процесор обробляє процес '{process.ProcessGuid}' тривалiстю {process.ExecutionDurationMs} мс \n");

                Thread.Sleep(process.ExecutionDurationMs);

                IsBusy = false;
            }
        }

        public void Run()
        {
            while (true)
            {
                if (!IsBusy)
                {
                    Process process;
                    if (_firstQueue.Count > 0)
                    {
                        process = _firstQueue.Dequeue();

                        Console.WriteLine($"|{DateTime.Now.ToString("dd.MM.yyyy hh:mm:ss.fff tt")}| процесор був вiльним, тому процес '{process.ProcessGuid}' з 1 черги пiшов на обробку CPU \n");

                        Run(process);
                    }
                    else if(_secondQueue.Count > 0)
                    {
                        process = _secondQueue.Dequeue();

                        Console.WriteLine($"|{DateTime.Now.ToString("dd.MM.yyyy hh:mm:ss.fff tt")}| процесор був вiльним, а 1 черга пустою, тому процес '{process.ProcessGuid}' з 2 черги пiшов на обробку CPU \n");

                        Run(process);
                    }  
                }
            }
        }
    }
}
