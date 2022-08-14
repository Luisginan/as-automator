﻿using System;
using System.Collections.Generic;
using AS_Utility;
using System.Diagnostics;
using System.IO;

namespace AS_Automator.Task
{
    public class TaskMacUnzip : AsTask
    {
        public String FilePath { get; set; } = "";
        public String ExtractFolder { get; set; } = "";
        public TaskMacUnzip()
        {
        }

        public override AsTask GetSample()
        {
            return new TaskMacUnzip { FilePath = "x.asx", ExtractFolder = $@"@(temp)" };
        }

        public override void Run(List<Variable> variableList)
        {
            Console.Write("Extracting app ... ");
            foreach (var variable in variableList)
            {
                FilePath = FilePath.Replace("@(" + variable.Name + ")", variable.Value);
                ExtractFolder = ExtractFolder.Replace("@(" + variable.Name + ")", variable.Value);
            }

            FilePath = Environment.ExpandEnvironmentVariables(FilePath);
            ExtractFolder = Environment.ExpandEnvironmentVariables(ExtractFolder);

            var fileMaker = new NgFileMaker();
            fileMaker.DeleteFolder(new DirectoryInfo(ExtractFolder));

            Process process = new Process();
            process.StartInfo.FileName = @$"/bin/bash";
            process.StartInfo.Arguments = $@"-c ""unzip -o -q {FilePath} -d {ExtractFolder}""";
            process.StartInfo.Verb = "runas";
            process.Start();
            process.WaitForExit();
            if (process.ExitCode != 0)
            {
                throw new Exception($@"unzip app failed.");
            }

            process = new Process();
            process.StartInfo.FileName = @$"/bin/bash";
            process.StartInfo.Arguments = $@"-c ""chmod -R +r+w+x {ExtractFolder}""";
            process.StartInfo.Verb = "runas";
            process.Start();
            process.WaitForExit();
            if (process.ExitCode != 0)
            {
                throw new Exception($@"unzip app failed.");
            }

            Console.WriteLine("Done");
        }
    }
}
