using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace AS_Automator.Task
{
    public class TaskRegeditImport : AsTask
    {
        public List<String> ListFilePath { get; set; }
        public override AsTask GetSample()
        {
            var list = new List<String>();
            list.Add($@"reg.reg");
            return  new TaskRegeditImport() { ListFilePath = list };
        }

        public override void Run(List<Variable> variableList)
        {
            Console.Write($@"Importing {ListFilePath.Count} Registration Key ... ");
            var list = new List<String>();

            foreach (var file in ListFilePath)
            {
                var f = file;
                foreach (var variable in variableList)
                {
                    f = Environment.ExpandEnvironmentVariables((f.Replace("@(" + variable.Name + ")", variable.Value)));
                }
                list.Add(f);
            }

            ListFilePath = list;

            foreach (var f in ListFilePath)
            {             
                Process process = new Process();
                process.StartInfo.FileName = "reg"; ;
                process.StartInfo.Arguments = $@"import ""{f}""";
                process.StartInfo.Verb = "runas";
                process.StartInfo.RedirectStandardOutput = true;
                process.Start();
                process.WaitForExit();
            }

            Console.WriteLine("Done");
        }
    }
}
