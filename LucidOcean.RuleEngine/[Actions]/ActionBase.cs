/*=====================================================================
Authors: Jonathan Crossland
Copyright © Jonathan Crossland. All Rights Reserved.

The license is on the root of the main source-code directory.
=====================================================================*/

using System.Threading;
using System.Collections.Generic;
using LucidOcean.RuleEngine.Context;


namespace LucidOcean.RuleEngine
{
    public abstract class ActionBase : IAction
    {
        
        private ActionException _Exception;
        private ActionStatus    _Status;
        private string          _Name;
        private ActionContext   _Context;
        

        /// <summary>
        /// All Actions will be initialized first, before any actions are executed.
        /// </summary>
        /// <param name="context"></param>
        public virtual void Initialize(ActionContext context)
        {
            _Status = ActionStatus.Ready;
            _Context = context;
        }

        public virtual void Execute(ActionContext context)
        {
            _Status = ActionStatus.Ready;
            _Context = context;
        }

        /// <summary>
        /// Allows events to occur. Allows UI to update based on events.
        /// To be called within your long running actions.
        /// </summary>
        public void DoEvents()
        {

            while (_Context.Runtime.Paused && _Context.Runtime.Aborting==false)
            {
                //wait for a period of time before executing event
                AutoResetEvent wait = new AutoResetEvent(false);
                wait.WaitOne(10, false);
            }

            if (_Context.Runtime.Aborting)
            {
                throw new ActionException("This Action has been Aborted, due to Abort being called by the Runtime.");
            }
            
        }

        public ActionContext Context
        {
            get
            {
                return _Context;
            }
            set
            {
                _Context = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            get
            {
                return _Name;
            }
            internal set
            {
                _Name = value;
            }
        }

        /// <summary>
        /// Set this to a valid Exception when it occurs.
        /// </summary>
        /// <returns></returns>
        public virtual ActionException LastException
        {
            get
            {
                return _Exception;
            }
            set
            {
                _Exception = value;
                this.Status = ActionStatus.Errored;
            }
        }

        /// <summary>
        /// Returns the ActionStatus. The Actions Framework occasionally requests a status, on which it will act.
        /// If there is an Exception available, please set the Status to Error
        /// </summary>
        /// <returns>ActionStatus</returns>
        public virtual ActionStatus Status
        {
            get
            {
                return _Status;
            }
            set
            {
                _Status = value;
            }

        }
     
        List<IAction> IAction.Items
        {
            get
            {
                return null;
            }
            set
            {
                
            }
        }
        
    }
}
