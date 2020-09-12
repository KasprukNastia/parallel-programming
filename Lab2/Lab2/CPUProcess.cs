using System;
using System.Threading;

namespace Lab2
{
    public class CPUProcess
    {
        private readonly int _minProcessDurationMs;
        private readonly int _maxProcessDurationMs;

        private readonly int _cpuProcessId;

        private readonly CPUQueue _queue;
        private readonly CPU _cpu;

        public CPUProcess(int cpuProcessId,
            CPUQueue queue, 
            CPU cpu, 
            int minProcessDurationMs = 8000, 
            int maxProcessDurationMs = 20000)
        {
            _cpuProcessId = cpuProcessId;
            _queue = queue ?? throw new ArgumentNullException(nameof(queue));
            _cpu = cpu ?? throw new ArgumentNullException(nameof(cpu));
            _minProcessDurationMs = minProcessDurationMs;
            _maxProcessDurationMs = maxProcessDurationMs;
        }

        public void GenerateProcesses()
        {
            var random = new Random();

            Process process;
            while (true)
            {
                process = new Process(random.Next(_minProcessDurationMs, _maxProcessDurationMs));

                Console.WriteLine($"Потiк {_cpuProcessId} згенерував процес '{process.ProcessGuid}' тривалiстю {process.ExecutionDurationMs} мс \n");

                if (!_cpu.IsBusy)
                {
                    Console.WriteLine($"Процес '{process.ProcessGuid}' надiслався на виконання CPU \n");
                    _cpu.Run(process);
                }
                else
                {
                    Console.WriteLine($"Процес '{process.ProcessGuid}' помiстився в чергу \n");
                    _queue.Enqueue(process);
                }

                Thread.Sleep(random.Next(_minProcessDurationMs, (_minProcessDurationMs + _maxProcessDurationMs) / 2));
            }
        }
    }
}
