using AS_Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace AS_Automator.Task
{
    public class TaskAddHost : AsTask
    {
        public List<String> ListHost { get; set; }
        public override AsTask GetSample()
        {
            List<String> listHost = new List<string>
            {
                "e834.g.akamaiedge.net"
            };
            return new TaskAddHost { ListHost = listHost, Title ="" };
        }

        public override void Run(List<Variable> variableList)
        {
            if (Title == "")
                Title = $"Lockdown {ListHost.Count} host ... ";
            
            Console.Write(Title);
            var fileMaker = new NgFileMaker();

            var path = Environment.ExpandEnvironmentVariables($@"%WINDIR%\system32\drivers\etc\hosts");
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                path = "/etc/hosts";
            }

            NgFileMaker.SetAttributeFile(new FileInfo(path), FileAttributes.Normal);

            var hostText = NgFileMaker.ReadFromFile(path);
            var hostLine = hostText.Split(Environment.NewLine);

            var validHost = new List<String>();
            foreach (var host in ListHost)
            {
                var exist = false;
                foreach (var line in hostLine)
                {
                    if (line.ToLower().Contains(host.ToLower()))
                    {
                        if (!line.ToLower().Contains("#"))
                        {
                            exist = true;
                            break;
                        }
                    }
                }

                if (!exist)
                {
                    if (validHost.Where(x => x.ToLower() == host.ToLower()).Count() == 0)
                    {
                        validHost.Add(host);
                    }
                }
            }

            var newHost = new StringBuilder();
            newHost.AppendLine(hostText);

            foreach (var h in validHost)
            {
                newHost.AppendLine($"127.0.0.1 {h}");
            }

            NgFileMaker.WriteToFile(path, newHost.ToString());
            NgFileMaker.SetAttributeFile(new FileInfo(path), FileAttributes.ReadOnly);

            Console.WriteLine("Done");

        }
    }
}
