/*=====================================================================
Authors: Jonathan Crossland
Copyright © Jonathan Crossland. All Rights Reserved.

The license is on the root of the main source-code directory.
=====================================================================*/

namespace LucidOcean.RuleEngine
{
    public abstract class TransactionalAction : CompositeAction
    {
		

        public override void Initialize(LucidOcean.RuleEngine.Context.ActionContext context)
        {
            base.Initialize(context);

            int timeout = 0;
            string transactionTimeOut = base.Context.Configuration.GetValue("TransactionActionTimeout");

            int.TryParse(transactionTimeOut, out timeout);

            if (timeout <= 0)
                _TransactionTimeOut = TransactionTimeoutDefault;
        }

	

        public const int TransactionTimeoutDefault = 10;

     

        private bool _IsInTransaction;
        private int _TransactionTimeOut = 0;

   

        /// <summary>
        /// TransactionTimeout in Seconds. Default is 10
        /// </summary>
        public int TransactionTimeOut
        {
            get
            {
                return _TransactionTimeOut;
            }
            set
            {
                if (value <= 0)
                    value = TransactionTimeoutDefault;

                _TransactionTimeOut = value;
            }
        }

        /// <summary>
        /// Determines whether the current action is within a TransactionScope.
        /// </summary>
        public bool IsInTransaction
        {
            get
            {
                return _IsInTransaction;
            }
            internal set
            {
                _IsInTransaction = value;
            }
        }

    
    }
}
