/*=====================================================================
Authors: Jonathan Crossland
Copyright © Jonathan Crossland. All Rights Reserved.

The license is on the root of the main source-code directory.
=====================================================================*/

using LucidOcean.RuleEngine;
using LucidOcean.RuleEngine.Configuration;
using LucidOcean.RuleEngine.Context;

namespace Example
{
    public class CustomProviderService : ActionListProvider
    {
        public override ActionListCollection BuildActionsCollection(ActionRuntimeSection runtimeConfig, ActionContext context)
        {
            // when you use this custom provider, it will return a new list
            //call base if you want the default behaviour, which loads actions from the config file
            //base.BuildActionsCollection(runtimeConfig, context);

            //do this to build a custom list and return it
            ActionListCollection actions = new ActionListCollection();

            IAction action = new CalculationAction();
            actions.Add(action);
            
            return actions;
        }


    }
}
