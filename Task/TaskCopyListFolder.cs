﻿using AS_Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AS_Automator.Task
{
    public class TaskCopyListFolder : AsTask
    {

        public List<CopyFolder> ListCopyFolder { get; set; }
        public override AsTask GetSample()
        {
            var list = new List<CopyFolder>();
            list.Add(new CopyFolder { FolderInfoSource = "dir", FolderInfoDestination = @"@(apppath)\dir" });
            return new TaskCopyListFolder { ListCopyFolder = list };
        }

        public override void Run(List<Variable> variableList)
        {
            var fileMaker = new NgFileMaker();

            Console.Write($@"Activating {ListCopyFolder.Count} Folder ... ");
            foreach (var file in ListCopyFolder)
            {
                var FileInfoSource = file.FolderInfoSource;
                var FileInfoDestination = file.FolderInfoDestination;

                foreach (var variable in variableList)
                {
                    FileInfoSource = FileInfoSource.Replace("@(" + variable.Name + ")", variable.Value);
                    FileInfoDestination = FileInfoDestination.Replace("@(" + variable.Name + ")", variable.Value);
                }

                var fsource = Environment.ExpandEnvironmentVariables(FileInfoSource);
                var fdest = Environment.ExpandEnvironmentVariables(FileInfoDestination);

                fileMaker.CopyFolder(fsource, fdest);
            }
            Console.WriteLine("Done");
        }
    }

    public class CopyFolder
    {
        public string FolderInfoSource { get; set; } = "";
        public string FolderInfoDestination { get; set; } = "";
    }
}
