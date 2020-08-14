using AS_Utility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace AS_Automator.Task
{
    public class TaskDeleteService : AsTask
    {
        public String ServiceName { get; set; } = "";
        public String FilePath { get; set; } = "";
        public override AsTask GetSample()
        {
            return new TaskDeleteService() { ServiceName = "notepad", FilePath = "C:\\notepad" };
        }

        public override void Run(List<Variable> variableList)
        {
            Console.Write($@"Safe Service {ServiceName} ... ");
            foreach (var variable in variableList)
            {
                ServiceName = ServiceName.Replace("@(" + variable.Name + ")", variable.Value);
                FilePath = FilePath.Replace("@(" + variable.Name + ")", variable.Value);
            }

            FilePath = Environment.ExpandEnvironmentVariables(FilePath);

            Process process = new Process();
            process.StartInfo.FileName = "sc"; ;
            process.StartInfo.Arguments = $@"stop {ServiceName}";
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.Verb = "runas";
            process.Start();
            process.WaitForExit();

            var maxLoop = 2;
            for (int i = 0; i < maxLoop; i++)
            {
                try
                {
                    Thread.Sleep(10000);
                    new NgFileMaker().DeleteFile(new System.IO.FileInfo(FilePath), true);
                    break;
                }
                catch (Exception ex)
                {
                  
                    if (i== (maxLoop -1))
                        throw;

                    Console.WriteLine(ex.Message);
                    Console.WriteLine("retrying...");
                }
            }
           


            Console.WriteLine("Done");
             
        }
    }
}
