using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace AS_Automator.Task
{
    public class TaskRunMsi : AsTask
    {
        public String FilePath { get; set; } = "";
        public String Arguments { get; set; } = "";
        public String WorkingDirectory { get; set; } = "";
        public override AsTask GetSample()
        {
            return new TaskRunMsi() { FilePath = @"@(tempx)\run.msi", Title = "" };
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

            var fileInfo = new FileInfo(FilePath);

            if (Title == "")
                Title = $"Installing as.{fileInfo.Name.Replace(".msi", ".masa").ToLower()} ...";

            Console.Write(Title);

            Process process = new Process();
            process.StartInfo.WorkingDirectory = WorkingDirectory;
            process.StartInfo.FileName = "msiexec"; ;
            process.StartInfo.Arguments = $@"/passive /I ""{FilePath}"" {Arguments}";
            process.StartInfo.Verb = "runas";
            process.Start();
            process.WaitForExit();
            if (process.ExitCode != 0)
                throw new Exception($"Run {fileInfo.Name} failed");
            Console.WriteLine("Done");
        }
    }
}
