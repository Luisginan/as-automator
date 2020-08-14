using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace AS_Automator.Task
{
    public class TaskRunProgram : AsTask
    {
        public String Program { get; set; } = "";
        public String Arguments { get; set; } = "";
        public String WorkingDirectory { get; set; } = "";

        public override AsTask GetSample()
        {
            return new TaskRunProgram { Program = "setup.exe", Arguments = "/silent", WorkingDirectory = @"@(tempx)" };
        }

        public override void Run(List<Variable> variablesList)
        {

            foreach (var variable in variablesList)
            {
                Program = Program.Replace("@(" + variable.Name + ")", variable.Value);
                Arguments = Arguments.Replace("@(" + variable.Name + ")", variable.Value);
                WorkingDirectory = WorkingDirectory.Replace("@(" + variable.Name + ")", variable.Value);
            }

            Program = Environment.ExpandEnvironmentVariables(Program);
            WorkingDirectory = Environment.ExpandEnvironmentVariables(WorkingDirectory);
            var p = new FileInfo(Program);
            Console.Write($"Run as.{p.Name.Replace(".exe", ".kapak").Replace("\\", "-").ToLower()} ... ");

            Process process = new Process();
            process.StartInfo.WorkingDirectory = WorkingDirectory;
            process.StartInfo.FileName = Program;
            process.StartInfo.Arguments = Arguments;
            process.StartInfo.Verb = "runas";
            process.StartInfo.UseShellExecute = true;
            process.Start();
            process.WaitForExit();

            Console.WriteLine("Done");
        }

    }
}
