using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace AS_Automator.Task
{
    public class TaskBlockFirewallList : AsTask
    {
        public List<TaskBlockFirewall> TaskList { get; set; } = new List<TaskBlockFirewall>();
        
        public override AsTask GetSample()
        {
            var list = new TaskBlockFirewallList();
            list.TaskList.Add(new TaskBlockFirewall { FilePath = $@"C:\X.exe" });
            list.TaskList.Add(new TaskBlockFirewall { FilePath = $@"C:\Z.exe" });
            return list;
        }

        public override void Run(List<Variable> variableList)
        {
            Console.Write($"Configuring Security for {TaskList.Count} Program ... ");

            foreach (var task in TaskList)
            {
                var FilePath = task.FilePath;
                foreach (var variable in variableList)
                {
                    FilePath = FilePath.Replace("@(" + variable.Name + ")", variable.Value);
                }

                var fileInfo = new FileInfo(FilePath);


                Process process = new Process();
                process.StartInfo.FileName = "netsh"; ;
                process.StartInfo.Arguments = $@" advfirewall firewall add rule name=""AS-{fileInfo.Name}"" dir=in action=block program=""{fileInfo.FullName}"" enable=yes";
                process.StartInfo.Verb = "runas";
                process.StartInfo.RedirectStandardOutput = true;
                process.Start();
                process.WaitForExit();

                process = new Process();
                process.StartInfo.FileName = "netsh"; ;
                process.StartInfo.Arguments = $@" advfirewall firewall add rule name=""AS-{fileInfo.Name}"" dir=out action=block program=""{fileInfo.FullName}"" enable=yes";
                process.StartInfo.Verb = "runas";
                process.StartInfo.RedirectStandardOutput = true;
                process.Start();
                process.WaitForExit();
            }
           

            Console.WriteLine("Done");
        }
    }
}
