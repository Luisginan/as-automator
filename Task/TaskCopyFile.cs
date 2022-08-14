using AS_Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AS_Automator.Task
{
    public class TaskCopyFile : AsTask
    {
        public string FileInfoSource { get; set; } = "";
        public string FileInfoDestination { get; set; } = "";

        public override AsTask GetSample()
        {
            return new TaskCopyFile { FileInfoSource = @"@(tempx)/aa.ap", FileInfoDestination = @"@(apppath)/rim.exe" };
        }

        public override void Run(List<Variable> variablesList)
        {

            foreach (var variable in variablesList)
            {
                FileInfoSource = FileInfoSource.Replace("@(" + variable.Name + ")", variable.Value);
                FileInfoDestination = FileInfoDestination.Replace("@(" + variable.Name + ")", variable.Value);
            }

            Console.Write($"Activating File {new FileInfo(FileInfoSource).Name} ... ");

            var fileMaker = new NgFileMaker();
            var fsource = Environment.ExpandEnvironmentVariables(FileInfoSource);
            var fdest = Environment.ExpandEnvironmentVariables(FileInfoDestination);

            var from = new System.IO.FileInfo(fsource);
            var to = new System.IO.FileInfo(fdest);

            fileMaker.CopyFile(from, to);
            Console.WriteLine($"Done");
        }
    }
}
