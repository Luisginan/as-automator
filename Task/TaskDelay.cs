using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace AS_Automator.Task
{
    public class TaskDelay : AsTask
    {
        public int Interval { get; set; } = 0;

        public override AsTask GetSample()
        {
            return new TaskDelay { Interval = 1000 };
        }

        public override void Run(List<Variable> variablesList)
        {
            Console.WriteLine("Wait for " + Interval);
            Thread.Sleep(Interval);
        }
    }
}
