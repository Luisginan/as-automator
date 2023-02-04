using AS_Utility;
using System;
using System.Collections.Generic;
using System.Text;

namespace AS_Automator.Task
{
    public class TaskWriteTitle : AsTask
    {
        public String Text { get; set; } = "";

        public override AsTask GetSample()
        {
            return new TaskWriteTitle { Text = "Welcome to my Apps", Title = "" };
        }

        public override void Run(List<Variable> variablesList)
        {

            var style = new StyleUtil();
            var textResult = Text;
            foreach (var variable in variablesList)
            {
                textResult = textResult.Replace("@(" + variable.Name + ")", variable.Value);
            }

            Console.Write(Text + " ");
            Console.Write(" (Tekan Enter untuk Melanjutkan)");
            Console.ReadLine();
        }
    }
}
