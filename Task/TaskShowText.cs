using System;
using System.Collections.Generic;

namespace AS_Automator.Task
{
    public class TaskShowText : AsTask
    {
        public List<String> ListText { get; set; }
        public TaskShowText()
        {
        }

        public override AsTask GetSample()
        {
            var list = new List<String>();
            list.Add("Hello");
            return new TaskShowText { ListText = list };
        }

        public override void Run(List<Variable> variableList)
        {
            Console.WriteLine("Catatan : ");
            foreach (var text in ListText)
            {
                var text2 = "";
                foreach (var variable in variableList)
                {
                    text2 = text.Replace("@(" + variable.Name + ")", variable.Value);
                }
                Console.WriteLine("  " + text2);
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("  (Tekan Enter untuk Melanjutkan)");
            Console.ResetColor();
            Console.ReadLine();
        }
    }
}
