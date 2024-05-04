/*=====================================================================
Authors: Jonathan Crossland
Copyright © Jonathan Crossland. All Rights Reserved.

The license is on the root of the main source-code directory.
=====================================================================*/

using LucidOcean.RuleEngine;
using LucidOcean.RuleEngine.Configuration;
using System;

namespace Example
{
    internal class Program
    {
        static void Main(string[] args)
        {
            BasicExample();

            FileWritingExample();
        }

        private static void FileWritingExample()
        {

            //By default ActionManagement is a singleton, which will last across usecases. The state is remembered from one call to another.
            //if you want a whole new runtime with new state, remember to destroy the previous instance
            //this seems strange at first, but in certain contexts where state should persist over multiple contexts, it is a simple pattern.
            ActionManagement.DestroyInstance();

            ActionManagement man = ActionManagement.CreateInstance();
            //clear the config actions as we dont need them for this example.
            man.Configuration.Actions.Clear();
            man.Configuration.RuntimeServices.Clear();
            man.Configuration.RuntimeProviders.Clear();

            ActionElement actionElement1 = new ActionElement();
            actionElement1.Name = "Start";
            actionElement1.TypeName = "Example.FileReadAction, Example";
            man.Configuration.Actions.Add(actionElement1);
         
            
            //lets add 10 of them
            for (int i = 0; i < 10; i++)
            {
                ActionElement actionElement2 = new ActionElement();
                actionElement2.Name = "AddContent"+i;
                actionElement2.TypeName = "Example.FileAddContentAction, Example";
                man.Configuration.Actions.Add(actionElement2);
            }
            

            ActionElement actionElement3 = new ActionElement();
            actionElement3.Name = "End";
            actionElement3.TypeName = "Example.FileWriteAction, Example";
            man.Configuration.Actions.Add(actionElement3);

            man.ActionContext.Properties["filename"] = "example.txt";
            man.Execute("Start");

            Console.WriteLine(man.ActionContext.Properties["filecontents"]?.ToString());
        }
        
        private static void BasicExample()
        {
            int calc = 0;

            ActionManagement man = ActionManagement.CreateInstance();

            //if you want to clear a runtime service, setup in the config do this
            //man.Configuration.RuntimeServices.Clear();

            //if you want to clear a provider, setup in the config, do this
            //man.Configuration.RuntimeProviders.Clear();



            //if you dont want to use the configuration in the config file, either remove it from the config, or clear the settings
            //manually adding your own in code. The library supports external config and code based config.
            //man.Configuration.Actions.Clear();
            //ActionElement actionElement1 = new ActionElement();
            //actionElement1.Name = "SomeName";
            //actionElement1.TypeName = "Example.CalculationAction,Example";
            //man.Configuration.Actions.Add(actionElement1);

            //add context and run
            man.ActionContext.Properties["x1"] = 1;
            man.ActionContext.Properties["x2"] = 8;
            man.Runtime.Progress += Runtime_Progress;
            man.Runtime.BeforeExecute += Runtime_BeforeExecute;
            man.Runtime.AfterExecute += Runtime_AfterExecute;
            man.Runtime.Start();


            var val = man.ActionContext.Properties["calculation"];
            if (val != null)
            {

                int.TryParse(val.ToString(), out calc);

            }
        }

        private static void Runtime_Progress(object sender, ProgressEventArgs e)
        {
            Console.WriteLine(e.Value.ToString());
        }

        private static void Runtime_AfterExecute(object sender, ActionRuntimeEventArgs e)
        {
            Console.WriteLine("After Execute");
        }

        private static void Runtime_BeforeExecute(object sender, ActionRuntimeEventArgs e)
        {
            Console.WriteLine("Before Execute");
        }
    }
}
