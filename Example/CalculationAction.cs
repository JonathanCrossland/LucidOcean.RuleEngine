/*=====================================================================
Authors: Jonathan Crossland
Copyright © Jonathan Crossland. All Rights Reserved.

The license is on the root of the main source-code directory.
=====================================================================*/

using LucidOcean.RuleEngine;

namespace Example
{
    public class CalculationAction : CompositeAction
    {

        
        public override void Execute(LucidOcean.RuleEngine.Context.ActionContext context)
        {
         
            var val1 = context?.Properties["x1"];
            var val2 = context?.Properties["x2"];

            int.TryParse(val1.ToString(), out int x1);
            int.TryParse(val2.ToString(), out int x2);

            context.Properties["calculation"] = x1 + x2;
            
            base.Execute(context);
        }
    }
}
