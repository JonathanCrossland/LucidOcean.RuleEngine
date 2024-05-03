/*=====================================================================
Authors: Jonathan Crossland
Copyright © Jonathan Crossland. All Rights Reserved.

The license is on the root of the main source-code directory.
=====================================================================*/
namespace LucidOcean.RuleEngine.Services
{

    public class CompositeService : RuntimeService
    {

        public CompositeService(ActionManagement actionManagement)
            : base(actionManagement)
        {
        }
        public override void Execute(IAction action)
        {

            if (action != base.ActionManagement.ActionContext.Runtime.LastRunAction)
            {
                //execute this action
                base.Execute(action);
                base.ActionManagement.ActionContext.Runtime.LastRunAction = action;
            }

            if (action.Status == ActionStatus.Errored)
            {
                throw action.LastException;
            }

            base.WaitIfPaused(action);
            
            //if its a composite we need to execute all the children
            if (action is CompositeAction)
            {
                CompositeAction composite = (CompositeAction)action;
                ExecuteItems(composite);
                return; //quit now since executeitems would have set the iscomplete
            }

            base.IsComplete = true;
       
        }

        /// <summary>
        /// Executes all composite children.
        /// </summary>
        /// <param name="composite"></param>
        private void ExecuteItems(CompositeAction composite)
        {
            foreach (ActionBase actionItem in composite.Items)
            {
                if (!base.ActionManagement.Runtime.IsRunning)
                {
                    base.IsComplete = false;
                    return;
                }

                WaitIfPaused(composite);
                Execute(actionItem);
                WaitIfPaused(composite);

                if (actionItem.Status == ActionStatus.Errored)
                {
                    throw actionItem.LastException;
                }
            }

            base.IsComplete = true;
        }
        
    }
}
