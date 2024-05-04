/*=====================================================================
Authors: Jonathan Crossland
Copyright © Jonathan Crossland. All Rights Reserved.

The license is on the root of the main source-code directory.
=====================================================================*/


using LucidOcean.RuleEngine;

namespace Example
{
    public class AbortingAction :  ActionBase
    {
        public AbortingAction()
        {
        }

        public override void Execute(LucidOcean.RuleEngine.Context.ActionContext context)
        {
            
            context.Runtime.Abort();
            
            base.Execute(context);
        }
    }
}
