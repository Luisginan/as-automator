using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace AS_Automator.Task
{
    public class TaskBash : AsTask
    {
        public String Command { get; set; }
        public String Arguments { get; set; }
        public String WorkingDirectory { get; set; }

        public TaskBash()
        {
        }

        public override AsTask GetSample()
        {
            return new TaskBash { Command = "ls", Arguments = "-a", Title = "" };
        }

        public override void Run(List<Variable> variableList)
        {
            if (Title == "")
                Title = $@"run bash {Command} ... ";

            Console.WriteLine(Title);

            foreach (var variable in variableList)
            {
                Command = Command.Replace("@(" + variable.Name + ")", variable.Value);
                Arguments = Arguments.Replace("@(" + variable.Name + ")", variable.Value);
                WorkingDirectory = WorkingDirectory.Replace("@(" + variable.Name + ")", variable.Value);
            }

            WorkingDirectory = Environment.ExpandEnvironmentVariables(WorkingDirectory);


            Process process = new Process();
            process.StartInfo.WorkingDirectory = WorkingDirectory;
            process.StartInfo.FileName = @$"/bin/bash";
            process.StartInfo.Arguments = $@"-c ""{Command} {Arguments}""";
            process.StartInfo.Verb = "runas";
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();
            process.WaitForExit();
            if (process.ExitCode != 0)
            {
                throw new Exception($@"{Command} failed.");
            }

            Console.WriteLine("Done");
        }
    }
}
