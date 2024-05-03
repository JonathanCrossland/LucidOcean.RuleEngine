/*=====================================================================
Authors: Jonathan Crossland
Copyright © Jonathan Crossland. All Rights Reserved.

The license is on the root of the main source-code directory.
=====================================================================*/

using System;
using System.Transactions;


namespace LucidOcean.RuleEngine.Services
{
    /// <summary>
    /// This Executes Actions within a transactionContext. 
    /// It uses MSDTC, so please make sure it is configured.
    /// todo: requires action to declare itself outside of transaction
    /// and/or abort/rollback forced if the action requires it.
    /// </summary>
    public class TransactionalService : CompositeService
    {
		
        private void ExecuteItems(TransactionalAction transAction)
        {
            foreach (TransactionalAction actionItem in transAction.Items)
            {
                actionItem.IsInTransaction = true;

                //call this method for recursion
                Execute(actionItem);

                if (actionItem.Status == ActionStatus.Errored)
                {
                    throw actionItem.LastException;
                }

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionManagement"></param>
        public TransactionalService(ActionManagement actionManagement)
            : base(actionManagement)
        {

        }


        /// <summary>
        /// requires MSDTC to be running, Uses transactional support with SQL Server.
        /// </summary>
        /// <param name="actionBase"></param>
        public override void Execute(IAction action)
        {

            try
            {

                TransactionalAction transAction = (TransactionalAction)action;
                long time = TimeSpan.TicksPerSecond * transAction.TransactionTimeOut;
                TimeSpan transactionTimeSpan = new TimeSpan(time);

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, transactionTimeSpan))
                {
                    action.Execute(ActionManagement.ActionContext);

                    if (action.Status == ActionStatus.Errored)
                    {
                        throw action.LastException;
                    }

                    ExecuteItems(transAction);

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                throw new ActionException("Check inner Exception for details", ex);
            }
        }
        
    }
}
