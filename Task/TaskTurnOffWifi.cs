using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace AS_Automator.Task
{
    public class TaskTurnOffWifi : AsTask
    {
        public override AsTask GetSample()
        {
            return new TaskTurnOffWifi();
        }

        public override void Run(List<Variable> variableList)
        {
            if (Title == "")
                Title = "Making network silent ...";

            Console.Write(Title);

            Process process = new Process();
            process.StartInfo.FileName = "netsh";
            process.StartInfo.Arguments = "wlan disconnect";
            process.StartInfo.Verb = "runas";
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();
            process.WaitForExit();
            Console.WriteLine("  Done.");

        }
    }
}
