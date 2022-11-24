// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Diagnostics;
using System.Text;
using Bodoconsult.Core.App.Interfaces;

namespace Bodoconsult.Core.App.PerformanceLogging
{
    /// <summary>
    /// Current implementation of <see cref="IPerformanceLogger"/> for performance logging
    /// </summary>
    public class PerformanceLogger : IPerformanceLogger
    {
        private readonly PerformanceCounter _privateBytes;

        private readonly PerformanceCounter _gen2Collections;

        private readonly PerformanceCounter _heapSize;

        private readonly PerformanceCounter _gcTime;

        private readonly PerformanceCounter _processorTime;

        private PerformanceCounter _ramCounter;

        private readonly float _numberOfProcessors;


        public PerformanceLogger()
        {
            var currentProcess = Process.GetCurrentProcess().ProcessName;

            _processorTime =new PerformanceCounter(categoryName: "Process", counterName: "% Processor Time", instanceName: currentProcess);
            _privateBytes = new PerformanceCounter(categoryName: "Process", counterName: "Private Bytes", instanceName: currentProcess);
            _gen2Collections = new PerformanceCounter(categoryName: ".NET CLR Memory", counterName: "# Gen 2 Collections", instanceName: currentProcess);
            _heapSize = new PerformanceCounter(categoryName: ".NET CLR Memory", counterName: "# Bytes in all Heaps", instanceName: currentProcess);
            _gcTime = new PerformanceCounter(categoryName: ".NET CLR Memory", counterName: "% time in GC", instanceName: currentProcess);
            _ramCounter = new PerformanceCounter("Process", "Working Set", currentProcess);


            _numberOfProcessors = Environment.ProcessorCount;
        }


        /// <summary>
        /// Get important counters as formatted string
        /// </summary>
        /// <returns>String with performance counter data</returns>
        public string GetCountersAsString()
        {
            var s = new StringBuilder();

            s.AppendLine($"processor time avg % = {GetValueAsFloat(_processorTime )/_numberOfProcessors}");
            s.AppendLine($"private bytes = {GetValue(_privateBytes)}");
            s.AppendLine($"RAM bytes = {GetValue(_ramCounter)}");
            s.AppendLine($"gc time in % = {GetValue(_gcTime)}");
            s.AppendLine($"gen 2 collections = {GetValue(_gen2Collections)}");
            s.AppendLine($"heap size = {GetValue(_heapSize )}");

            return s.ToString();
        }

        /// <summary>
        /// Get the value of a a performance counter as string or "-" in case of an error
        /// </summary>
        /// <param name="counter">Current performance counter</param>
        /// <returns>String with performance counter value</returns>
        private static string GetValue(PerformanceCounter counter)
        {
            try
            {
                return $"{counter.NextValue()}";
            }
            catch
            {
                return "-";
            }
        }

        /// <summary>
        /// Get the value of a a performance counter as string or "-" in case of an error
        /// </summary>
        /// <param name="counter">Current performance counter</param>
        /// <returns>String with performance counter value</returns>
        private static float GetValueAsFloat(PerformanceCounter counter)
        {
            try
            {
                return counter.NextValue();
            }
            catch
            {
                return 0;
            }
        }

    }
}