using System;

namespace Lab2
{
    public class Process
    {
        public int ExecutionDurationMs { get; }

        public Guid ProcessGuid { get; }

        public Process(int executionDurationMs)
        {
            ExecutionDurationMs = executionDurationMs;
            ProcessGuid = Guid.NewGuid();
        }
    }
}
