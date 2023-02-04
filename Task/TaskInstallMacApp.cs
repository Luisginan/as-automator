using System;
using System.Collections.Generic;
using System.Diagnostics;
using AS_Utility;

namespace AS_Automator.Task
{
    public class TaskInstallMacApp : AsTask
    {
        public String AppName { get; set; }
        public String FileName { get; set; }
        public String WorkingDirectory { get; set; }

        public TaskInstallMacApp()
        {
        }

        public override AsTask GetSample()
        {
            return new TaskInstallMacApp { AppName = $@"Any App.app", FileName = "x.as", WorkingDirectory = "@(temp)", Title = "" };
        }

        public override void Run(List<Variable> variableList)
        {
            var fileMaker = new NgFileMaker();

            var app = AppName.Replace(@"\", @"").Replace(@".app", @"");

            if (Title == "")
                Title = $@"Install app {app} ... ";

            Console.Write(Title);


            foreach (var variable in variableList)
            {
                AppName = AppName.Replace("@(" + variable.Name + ")", variable.Value);
                FileName = FileName.Replace("@(" + variable.Name + ")", variable.Value);
                WorkingDirectory = WorkingDirectory.Replace("@(" + variable.Name + ")", variable.Value);

            }

            FileName = Environment.ExpandEnvironmentVariables(FileName);
            WorkingDirectory = Environment.ExpandEnvironmentVariables(WorkingDirectory);


            var appUninstallLoc = new System.IO.DirectoryInfo($@"/Applications/{AppName}");
            fileMaker.DeleteFolder(appUninstallLoc);


            var process = new Process();
            process.StartInfo.WorkingDirectory = WorkingDirectory;
            process.StartInfo.FileName = @$"/bin/bash";
            process.StartInfo.Arguments = $@"-c ""mv {FileName} /Applications/{AppName.Replace(@" ", @"\ ")}""";
            process.StartInfo.Verb = "runas";
            process.Start();
            process.WaitForExit();

            process = new Process();
            process.StartInfo.WorkingDirectory = WorkingDirectory;
            process.StartInfo.FileName = @$"/bin/bash";
            process.StartInfo.Arguments = $@"-c ""xattr -cr /Applications/{AppName.Replace(@" ", @"\ ")}""";
            process.StartInfo.Verb = "runas";
            process.Start();
            process.WaitForExit();

            Console.WriteLine("Done");
        }
    }
}
