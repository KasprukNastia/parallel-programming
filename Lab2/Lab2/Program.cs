using System.Threading;

namespace Lab2
{
    class Program
    {
        static void Main(string[] args)
        {
            var firstProcessesQueue = new CPUQueue();
            var secondProcessesQueue = new CPUQueue();

            var cpu = new CPU(firstProcessesQueue, secondProcessesQueue);

            var firstCpuProcess = new CPUProcess(
                cpuProcessId: 1, firstProcessesQueue, cpu, minProcessDurationMs: 8000, maxProcessDurationMs: 20000);
            var secondCpuProcess = new CPUProcess(
                cpuProcessId: 2, secondProcessesQueue, cpu, minProcessDurationMs: 7000, maxProcessDurationMs: 15000);

            var cpuThread = new Thread(cpu.Run);
            var firstCpuProcessThread = new Thread(firstCpuProcess.GenerateProcesses);
            var secondCpuProcessThread = new Thread(secondCpuProcess.GenerateProcesses);

            cpuThread.Start();
            firstCpuProcessThread.Start();
            secondCpuProcessThread.Start();
        }
    }
}
