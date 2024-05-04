/*=====================================================================
Authors: Jonathan Crossland
Copyright © Jonathan Crossland. All Rights Reserved.

The license is on the root of the main source-code directory.
=====================================================================*/

using LucidOcean.RuleEngine;

namespace Example
{
    public class FileAddContentAction : CompositeAction
    {
        public override void Execute(LucidOcean.RuleEngine.Context.ActionContext context)
        {
            string content = context?.Properties["filecontents"]?.ToString();

            context.Properties["filecontents"] = content + "...";
        }
    }
}
