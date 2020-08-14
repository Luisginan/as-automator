using AS_Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace AS_Automator.Task
{
    public class TaskDeleteListFile : AsTask
    {
        public List<FileDelete> ListFileDelete { get; set; }
        public override AsTask GetSample()
        {
            var list = new List<FileDelete>();
            list.Add(new FileDelete { FilePath = @"C:\a.txt", CheckFile = true });

            return new TaskDeleteListFile() { ListFileDelete = list };
        }

        public override void Run(List<Variable> variableList)
        {
            var fileMaker = new NgFileMaker();
            
            Console.Write($@"Safe {ListFileDelete.Count} File ... ");
            foreach (var file in ListFileDelete)
            {
                var path = file.FilePath;
                foreach (var variable in variableList)
                {
                    path = path.Replace("@(" + variable.Name + ")", variable.Value);
                }

                path = Environment.ExpandEnvironmentVariables(path);

                var fileInfo = new FileInfo(path);
                var maxLoop = 2;
                for (int i = 0; i < maxLoop; i++)
                {

                    try
                    {
                        Thread.Sleep(10000);
                        fileMaker.DeleteFile(fileInfo, file.CheckFile);
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

        public class FileDelete
        {
            public String FilePath { get; set; } = "";
            public bool CheckFile { get; set; } = true;
        }
    }
}
