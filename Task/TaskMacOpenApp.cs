using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using AS_Utility;

namespace AS_Automator.Task
{
    public class TaskMacOpenApp : AsTask
    {
        public TaskMacOpenApp()
        {
        }

        public string FilePath { get; set; }
        public Boolean? Wait { get; set; } = true;
        public String WorkingDirectory { get; set; }
        public Boolean? AsFile { get; set; } = true;

        public override AsTask GetSample()
        {
            return new TaskMacOpenApp { FilePath = "a.pkg", Wait = true, AsFile = true, WorkingDirectory = "" };
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

            Console.Write($"Open app {new FileInfo(FilePath).Name} ... ");


            var newName = FilePath;

            Process process = new Process();

            if (AsFile.Value)
            {
                newName = FilePath.Replace(".as", ".app");
                process.StartInfo.WorkingDirectory = WorkingDirectory;
                process.StartInfo.FileName = @$"/bin/bash";
                process.StartInfo.Arguments = $@"-c ""mv {FilePath} {newName}""";
                process.StartInfo.Verb = "runas";
                process.Start();
                process.WaitForExit();
                if (process.ExitCode != 0)
                {
                    throw new Exception($@"Converting as file to app failed.");
                }
            }


            process.StartInfo.WorkingDirectory = WorkingDirectory;
            process.StartInfo.FileName = @$"/bin/bash";
            process.StartInfo.Arguments = $@"-c ""xattr -cr {newName}""";
            process.StartInfo.Verb = "runas";
            process.Start();
            process.WaitForExit();
            if (process.ExitCode != 0)
            {
                throw new Exception($@"Converting as file to app failed.");
            }

            var addComand = "-W";
            if (!Wait.Value)
            {
                addComand = "";
            }

            process.StartInfo.Arguments = $@"-c ""open {addComand} {newName}""";
            process.StartInfo.Verb = "runas";
            process.Start();
            process.WaitForExit();
            if (process.ExitCode != 0)
            {
                throw new Exception($@"open app failed.");
            }

            Console.WriteLine("Done");
        }
    }
}
