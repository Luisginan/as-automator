using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace AS_Automator.Task
{
    public class TaskBlockFirewall : AsTask
    {
        public String FilePath { get; set; } = "";
        public override AsTask GetSample()
        {
            return new TaskBlockFirewall() { FilePath = "C:\\a.exe" };
        }

        public override void Run(List<Variable> variableList)
        {
           
            foreach (var variable in variableList)
            {
                FilePath = FilePath.Replace("@(" + variable.Name + ")", variable.Value);
            }

            var fileInfo = new FileInfo(FilePath);
            Console.Write($"Set Firewall for {fileInfo.Name} ... ");

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

            Console.WriteLine("Done");
        }
    }
}
