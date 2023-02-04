using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace AS_Automator.Task
{
    public class TaskMacWifi : AsTask
    {
        public Boolean Enable { get; set; } = true;

        public TaskMacWifi()
        {
        }

        public override AsTask GetSample()
        {
            return new TaskMacWifi { Enable = false, Title = "" };
        }

        public override void Run(List<Variable> variableList)
        {
            if (Title == "")
                Title = $@"Set Wifi {Enable} ... ";

            Console.Write(Title);

            Process process = new Process();
            process.StartInfo.FileName = @$"/bin/bash";
            if (Enable)
                process.StartInfo.Arguments = $@"-c ""networksetup -setairportpower en0 on""";
            else
                process.StartInfo.Arguments = $@"-c ""networksetup -setairportpower en0 off""";

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
