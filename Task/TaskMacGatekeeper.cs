using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace AS_Automator.Task
{
    public class TaskMacGatekeeper : AsTask
    {
        public Boolean Enable { get; set; } = true;

        public TaskMacGatekeeper()
        {
        }

        public override AsTask GetSample()
        {
            return new TaskMacGatekeeper { Enable = false};
        }

        public override void Run(List<Variable> variableList)
        {
            Console.Write($@"Set Spctl {Enable} ... ");

            Process process = new Process();
            process.StartInfo.FileName = @$"/bin/bash";
            if (Enable)
                process.StartInfo.Arguments = $@"-c ""spctl --master-enable""";
            else
                process.StartInfo.Arguments = $@"-c ""spctl --master-disable""";

            process.StartInfo.Verb = "runas";
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();
            process.WaitForExit();
            if (process.ExitCode != 0)
            {
                throw new Exception($@"Set Spctl failed.");
            }

            Console.WriteLine("Done");
        }
    }
}
