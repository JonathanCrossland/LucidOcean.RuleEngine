/*=====================================================================
Authors: Jonathan Crossland
Copyright © Jonathan Crossland. All Rights Reserved.

The license is on the root of the main source-code directory.
=====================================================================*/

using System;
using LucidOcean.RuleEngine.Context;

namespace LucidOcean.RuleEngine
{
    public class ActionListProvider
    {
		
        public virtual ActionListCollection BuildActionsCollection(Configuration.ActionRuntimeSection runtimeConfig, ActionContext context)
        {
            ActionListCollection actions = new ActionListCollection();

            foreach (LucidOcean.RuleEngine.Configuration.ActionElement actionElement in runtimeConfig.Actions)
            {
                ActionBase action = Factory.CreateAction(actionElement);
                action.Name = actionElement.Name;

                if (action != null)
                {
                    try
                    {
                        action.Initialize(context);
                    }
                    catch (ArgumentException ex)
                    {
                        context.WriteToLog(ex.Message);

                        if (ex.Message.Contains("Item has already been added. Key in dictionary: 'Domain'  Key being added: 'Domain'"))
                        {
                            //lets ignore hashtable duplicates error.
                        }
                    }

                    actions.Add(action);
                }
            }

            return actions;
        }


    }
}
