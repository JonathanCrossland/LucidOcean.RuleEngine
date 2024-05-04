/*=====================================================================
Authors: Jonathan Crossland
Copyright © Jonathan Crossland. All Rights Reserved.

The license is on the root of the main source-code directory.
=====================================================================*/

using System.Collections.Generic; 

namespace LucidOcean.RuleEngine
{
    public interface IAction
    {
        void DoEvents();

        void Execute(LucidOcean.RuleEngine.Context.ActionContext context);

        void Initialize(LucidOcean.RuleEngine.Context.ActionContext context);

        LucidOcean.RuleEngine.Context.ActionContext Context { get; set; }

        LucidOcean.RuleEngine.RuleActionException LastException { get; set; }

        string Name { get; }

        LucidOcean.RuleEngine.ActionStatus Status { get; set; }

        List<IAction> Items { get; set; }
	
    }
}