using AS_Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AS_Automator.Task
{
    public class TaskCopyListFile : AsTask
    {
     
        public List<TaskCopyFile> ListFileCopy { get; set; }
        public override AsTask GetSample()
        {
            var list = new List<TaskCopyFile>();
            list.Add(new TaskCopyFile { FileInfoSource = "aa.ap", FileInfoDestination = @"@(apppath)\rim.exe" });
            return new TaskCopyListFile { ListFileCopy = list };
        }

        public override void Run(List<Variable> variableList)
        {
            var fileMaker = new NgFileMaker();

            Console.Write($@"Activating {ListFileCopy.Count} File ... ");
            foreach (var file in ListFileCopy)
            {
                var FileInfoSource = file.FileInfoSource;
                var FileInfoDestination = file.FileInfoDestination;

                foreach (var variable in variableList)
                {
                    FileInfoSource = FileInfoSource.Replace("@(" + variable.Name + ")", variable.Value);
                    FileInfoDestination = FileInfoDestination.Replace("@(" + variable.Name + ")", variable.Value);
                }

                var fsource = Environment.ExpandEnvironmentVariables(FileInfoSource);
                var fdest = Environment.ExpandEnvironmentVariables(FileInfoDestination);

                var from = new FileInfo(fsource);
                var to = new FileInfo(fdest);
                
                fileMaker.CopyFile(from, to);
            }
            Console.WriteLine("Done");
        }
    }
}
