using AS_Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace AS_Automator.Task
{
    public class TaskDeleteListFolder : AsTask
    {
        public List<FolderDelete> ListFolderDelete { get; set; }
        public override AsTask GetSample()
        {
            var list = new List<FolderDelete>();
            list.Add(new FolderDelete { FolderPath = @"@(apppath)\folder", CheckFile = true });

            return new TaskDeleteListFolder() { ListFolderDelete = list };
        }

        public override void Run(List<Variable> variableList)
        {
            var fileMaker = new NgFileMaker();

            Console.Write($@"Safe {ListFolderDelete.Count} Folder ... ");
            foreach (var folder in ListFolderDelete)
            {
                var path = folder.FolderPath;
                foreach (var variable in variableList)
                {
                    path = path.Replace("@(" + variable.Name + ")", variable.Value);
                }

                path = Environment.ExpandEnvironmentVariables(path);

                var fileInfo = new DirectoryInfo(path);

                var maxLoop = 2;
                for (int i = 0; i < maxLoop; i++)
                {
                    try
                    {
                        Thread.Sleep(10000);
                        fileMaker.DeleteFolder(fileInfo, folder.CheckFile);
                        break;
                    }
                    catch (Exception ex)
                    {

                        if (i == (maxLoop - 1))
                            throw;

                        Console.WriteLine(ex.Message);
                        Console.WriteLine("retrying...");
                    }
                }


            }
            Console.WriteLine("Done");
        }

        public class FolderDelete
        {
            public String FolderPath { get; set; } = "";
            public bool CheckFile { get; set; } = true;
        }
    }
}
