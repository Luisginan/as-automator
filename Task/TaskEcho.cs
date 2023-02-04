using System;
using System.Collections.Generic;

namespace AS_Automator.Task
{
    public class TaskEcho : AsTask
    {
        public String Message { get; set; } = "";
        public TaskEcho()
        {
        }

        public override AsTask GetSample()
        {
            return new TaskEcho { Message = "Enjoy!", Title = "" };
        }

        public override void Run(List<Variable> variableList)
        {
            foreach (var variable in variableList)
            {
                Message = Message.Replace("@(" + variable.Name + ")", variable.Value);
            }


            Console.WriteLine(Message);
            Console.ResetColor();
            Console.ReadLine();
        }
    }
}
