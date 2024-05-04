/*=====================================================================
Authors: Jonathan Crossland
Copyright © Jonathan Crossland. All Rights Reserved.

The license is on the root of the main source-code directory.
=====================================================================*/

using LucidOcean.RuleEngine;
using System.IO;

namespace Example
{
    public class FileReadAction : CompositeAction
    {
        public override void Execute(LucidOcean.RuleEngine.Context.ActionContext context)
        {

            string filename = context?.Properties["filename"].ToString();

            if (File.Exists(filename)){
                context.Properties["filecontents"] = File.ReadAllText(filename);
            }

        }
    }
}
