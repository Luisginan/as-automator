using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace AS_Automator.Task
{
    public class TaskExludeFromWinDefender : AsTask
    {
        public List<String> ListFolderPath { get; set; }
        public override AsTask GetSample()
        {
            var list = new List<String>();
            list.Add($@"%temp$\as\x");
            return new TaskExludeFromWinDefender() { ListFolderPath = list, Title = "" };
        }

        public override void Run(List<Variable> variableList)
        {
            if (Title == "")
                Title = $@"Making folder resistance {ListFolderPath.Count} ... ";

            Console.Write(Title);

            var list = new List<String>();


            foreach (var file in ListFolderPath)
            {
                var f = file;
                foreach (var variable in variableList)
                {           
                    f = Environment.ExpandEnvironmentVariables((f.Replace("@(" + variable.Name + ")", variable.Value)));
                }
                list.Add(f);
            }

            ListFolderPath = list;

            try
            {
                foreach (var f in ListFolderPath)
                {

                    Process process = new Process();
                    process.StartInfo.FileName = "powershell"; ;
                    process.StartInfo.Arguments = $@"Add-MpPreference -ExclusionPath '{f}'";
                    process.StartInfo.Verb = "runas";
                    process.Start();
                    process.WaitForExit();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed : " + ex.Message);
            }
           

            Console.WriteLine("Done");
        }
    }
}
