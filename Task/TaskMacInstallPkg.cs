using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace AS_Automator.Task
{
    public class TaskMacInstallPkg : AsTask
    {
        public String FilePath { get; set; }

        public String WorkingDirectory { get; set; }

        public TaskMacInstallPkg()
        {
        }

        public override AsTask GetSample()
        {
            return new TaskMacInstallPkg { FilePath = "a.pkg", WorkingDirectory = "@(tempx)", Title = "" };
        }

        public override void Run(List<Variable> variableList)
        {
            foreach (var variable in variableList)
            {
                FilePath = FilePath.Replace("@(" + variable.Name + ")", variable.Value);
                WorkingDirectory = WorkingDirectory.Replace("@(" + variable.Name + ")", variable.Value);
            }

            FilePath = Environment.ExpandEnvironmentVariables(FilePath);
            WorkingDirectory = Environment.ExpandEnvironmentVariables(WorkingDirectory);

            if (Title == "")
                Title = $"Installing Package {new FileInfo(FilePath).Name.Replace(".pkg", "")} ... ";

            Console.Write(Title);
           
            Process process = new Process();
            process.StartInfo.WorkingDirectory = WorkingDirectory;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.FileName = @$"/bin/bash";
            process.StartInfo.Arguments = $@"-c ""xattr -cr {FilePath}""";
            process.StartInfo.Verb = "runas";
            process.Start();
            process.WaitForExit();
            if (process.ExitCode != 0)
            {
                throw new Exception($@"Change attribute quarantine failed.");
            }

            process = new Process();
            process.StartInfo.WorkingDirectory = WorkingDirectory;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.FileName = @$"/bin/bash";
            process.StartInfo.Arguments = $@"-c ""installer -pkg {FilePath} -target / """;
            process.StartInfo.Verb = "runas";
            process.Start();
            process.WaitForExit();
            if (process.ExitCode != 0)
            {
                throw new Exception($@"Installing package failed.");
            }

            Console.WriteLine("Done");
        }
    }
}
