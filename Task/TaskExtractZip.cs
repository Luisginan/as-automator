using AS_Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AS_Automator.Task
{
    public class TaskExtractZip : AsTask
    {
        public String FilePath { get; set; } = "";
        public String ExtractFolder { get; set; } = "";
        public override AsTask GetSample()
        {
            return new TaskExtractZip { FilePath = "x.asx", ExtractFolder = $@"@(temp)", Title = "" };
        }

        public override void Run(List<Variable> variableList)
        {
            if (Title == "")
                Title = "Extracting app ... ";

            Console.Write(Title);

            foreach (var variable in variableList)
            {
                FilePath = FilePath.Replace("@(" + variable.Name + ")", variable.Value);
                ExtractFolder = ExtractFolder.Replace("@(" + variable.Name + ")", variable.Value);
            }

            FilePath = Environment.ExpandEnvironmentVariables(FilePath);
            ExtractFolder = Environment.ExpandEnvironmentVariables(ExtractFolder);

            var fileMaker = new NgFileMaker();
            fileMaker.ExtractZip(new DirectoryInfo(ExtractFolder), new FileInfo(FilePath));
            Console.WriteLine("Done");
        }
    }
}
