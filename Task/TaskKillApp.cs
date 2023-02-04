using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace AS_Automator.Task
{
    public class TaskKillApp : AsTask
    {
        public String AppName { get; set; } = "";
        public override AsTask GetSample()
        {
            return new TaskKillApp() { AppName = "notepad", Title = "" };
        }

        public override void Run(List<Variable> variableList)
        {
            foreach (var variable in variableList)
            {
                AppName = AppName.Replace("@(" + variable.Name + ")", variable.Value);
            }

            if (Title == "")
                Title = $"Kill App {AppName}";

            Console.Write(Title);

            Process[] proc = Process.GetProcessesByName($"{AppName}");
            if (proc.Length > 0)
                proc[0].Kill();
            else
                Console.WriteLine($"{AppName} tidak ditemukan");
        }
    }
}
