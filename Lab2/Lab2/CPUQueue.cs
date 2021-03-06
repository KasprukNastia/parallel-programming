﻿using System.Collections.Generic;
using System.Threading;

namespace Lab2
{
    public class CPUQueue
    {
        private readonly object _locker = new object();

        private readonly int _maxSize;
        private readonly LinkedList<Process> _processesQueue;

        public int Count
        {
            get
            {
                bool acquiredLock = false;
                try
                {
                    Monitor.Enter(_locker, ref acquiredLock);
                    return _processesQueue.Count;
                }
                finally
                {
                    if (acquiredLock) 
                        Monitor.Exit(_locker);
                }
            }
        }

        public CPUQueue(int maxSize = 20)
        {
            _maxSize = maxSize;
            _processesQueue = new LinkedList<Process>();
        }

        public Process Dequeue()
        {
            bool acquiredLock = false;
            try
            {
                Monitor.Enter(_locker, ref acquiredLock);

                while (_processesQueue.Count == 0)
                    Monitor.Wait(_locker);
                
                Process toReturn = _processesQueue.First.Value;
                _processesQueue.RemoveFirst();
                return toReturn;
            }
            finally
            {
                if (acquiredLock) 
                    Monitor.Exit(_locker);
            }
        }

        public void Enqueue(Process process)
        {
            bool acquiredLock = false;
            try
            {
                Monitor.Enter(_locker, ref acquiredLock);

                while (_processesQueue.Count >= _maxSize)
                    Monitor.Wait(_locker);
                
                _processesQueue.AddLast(process);
            }
            finally
            {
                if (acquiredLock) 
                    Monitor.Exit(_locker);
            }
        }
    }
}
