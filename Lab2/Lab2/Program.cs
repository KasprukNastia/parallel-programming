using System.Threading;

namespace Lab2
{
    class Program
    {
        static void Main(string[] args)
        {
            var firstProcessesQueue = new CPUQueue(maxSize: 7);
            var secondProcessesQueue = new CPUQueue(maxSize: 7);

            var cpu = new CPU(firstProcessesQueue, secondProcessesQueue);

            var firstCpuProcess = new CPUProcess(
                cpuProcessId: 1, firstProcessesQueue, cpu, minProcessDurationMs: 3000, maxProcessDurationMs: 6000);
            var secondCpuProcess = new CPUProcess(
                cpuProcessId: 2, secondProcessesQueue, cpu, minProcessDurationMs: 2000, maxProcessDurationMs: 5000);

            var cpuThread = new Thread(cpu.Run);
            var firstCpuProcessThread = new Thread(firstCpuProcess.GenerateProcesses);
            var secondCpuProcessThread = new Thread(secondCpuProcess.GenerateProcesses);

            cpuThread.Start();
            firstCpuProcessThread.Start();
            secondCpuProcessThread.Start();
        }
    }
}
