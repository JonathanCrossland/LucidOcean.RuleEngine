/*=====================================================================
Authors: Jonathan Crossland
Copyright © Jonathan Crossland. All Rights Reserved.

The license is on the root of the main source-code directory.
=====================================================================*/

namespace LucidOcean.RuleEngine
{
    public abstract class GoToAction : ActionBase
    {
		
        public abstract bool Evaluate();

        public abstract string GetActionName();

		
    }
}
