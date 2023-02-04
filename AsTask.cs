
using AS_Automator.Task;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace AS_Automator
{
    [XmlInclude(typeof(TaskDelay))]
    [XmlInclude(typeof(TaskRunProgram))]
    [XmlInclude(typeof(TaskCopyFile))]
    [XmlInclude(typeof(TaskWriteTitle))]
    [XmlInclude(typeof(TaskTurnOffWifi))]
    [XmlInclude(typeof(TaskKillApp))]
    [XmlInclude(typeof(TaskRunMsi))]
    [XmlInclude(typeof(TaskDeleteService))]
    [XmlInclude(typeof(TaskBlockFirewall))]
    [XmlInclude(typeof(TaskExtractZip))]
    [XmlInclude(typeof(TaskDeleteListFile))]
    [XmlInclude(typeof(TaskAddHost))]
    [XmlInclude(typeof(TaskRegeditImport))]
    [XmlInclude(typeof(TaskBash))]
    [XmlInclude(typeof(TaskInstallMacApp))]
    [XmlInclude(typeof(TaskMacUnzip))]
    [XmlInclude(typeof(TaskMacOpenApp))]
    [XmlInclude(typeof(TaskMacInstallPkg))]
    [XmlInclude(typeof(TaskShowText))]
    [XmlInclude(typeof(TaskRunMsp))]
    [XmlInclude(typeof(TaskCopyListFile))]
    [XmlInclude(typeof(TaskEcho))]
    [XmlInclude(typeof(TaskDeleteListFolder))]
    [XmlInclude(typeof(TaskCopyListFolder))]
    [XmlInclude(typeof(TaskExludeFromWinDefender))]
    [XmlInclude(typeof(TaskMacWifi))]
    [XmlInclude(typeof(TaskMacGatekeeper))]
    [XmlInclude(typeof(TaskBlockFirewallList))]
    [Serializable]
    public abstract class AsTask
    {
        public abstract void Run(List<Variable> variableList);

        public abstract AsTask GetSample();

        public Boolean Disable { get; set; } = false;

        public String Title { get; set; } = "";

        protected void ShowInfo(string msg)
        {
            Console.ResetColor();
            Console.WriteLine(msg);

        }
    }
}
