/*=====================================================================
Authors: Jonathan Crossland
Copyright © Jonathan Crossland. All Rights Reserved.

The license is on the root of the main source-code directory.
=====================================================================*/

using System.Threading;

namespace LucidOcean.RuleEngine.Services
{
    /// <summary>
    /// Service that executes an Action that passed to execute method.
    /// You can create your own service to execute actions. Inherit from this class and configure it in configuration settings.
    /// </summary>
    public class RuntimeService
    {

        /// <summary>
        /// This should be called by any Execute implementation, before moving on to execute more.
        /// The Execute is expected to quit within a reasonably time of the Aborted action being set.
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        protected internal virtual bool MustQuit(IAction action)
        {
            if (ActionManagement.Runtime.IsAborting)
            {
                ActionManagement.Runtime.RaiseAborting(action);

                return true;
            }

            if (ActionManagement.Runtime.IsStopping)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Code to 'wait'. A loop that waits until the State changes from Paused.
        /// </summary>
        /// <param name="action"></param>
        protected internal virtual void WaitIfPaused(IAction action)
        {
            while (ActionManagement.Runtime.IsPaused)
            {
                //wait for a period of time before executing event
                AutoResetEvent wait = new AutoResetEvent(false);
                wait.WaitOne(10, false);

                ActionManagement.Runtime.RaisePausedEvent(action);
            }
        }

        private ActionManagement _ActionManagement;
        private bool _IsComplete;

        /// <summary>
        /// 
        /// </summary>
        public RuntimeService()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionManagement"></param>
        public RuntimeService(ActionManagement actionManagement)
        {
            _ActionManagement = actionManagement;
        }

        /// <summary>
        /// A standard base implementation of Execute.
        /// Raises before and After Execute events.
        /// Throws any Exceptions if caught.
        /// </summary>
        /// <param name="action"></param>
        public virtual void Execute(IAction action)
        {
            if (!MustQuit(action))
            {
                ActionManagement.Runtime.RaiseBeforeExecute(action);
                action.Execute(_ActionManagement.ActionContext);
                ActionManagement.Runtime.RaiseAfterExecute(action);

                if (action.Status == ActionStatus.Errored)
                {
                    throw action.LastException;
                }

                _IsComplete = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ActionManagement ActionManagement
        {
            get
            {
                return _ActionManagement;
            }
            set
            {
                _ActionManagement = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsComplete
        {
            get
            {
                return _IsComplete;
            }
            set
            {
                _IsComplete = value;
            }
        }

        
    }
}
