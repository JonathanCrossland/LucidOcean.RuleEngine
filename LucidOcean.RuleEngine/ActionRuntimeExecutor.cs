/*=====================================================================
Authors: Jonathan Crossland
Copyright © Jonathan Crossland. All Rights Reserved.

The license is on the root of the main source-code directory.
=====================================================================*/

using System;
using LucidOcean.RuleEngine.Services;


namespace LucidOcean.RuleEngine
{
    internal class ActionRuntimeExecutor
    {

        private ActionRuntimeState _State;

        internal ActionRuntimeExecutor(ActionRuntimeState state)
        {
            _State = state;

            _State.Context.Runtime.ProgressStart += new EventHandler<ProgressStartEventArgs>(Runtime_ProgressStart);
            _State.Context.Runtime.Progress += new EventHandler<ProgressEventArgs>(Runtime_Progress);
            _State.Context.Runtime.ProgressEnd += new EventHandler<ProgressEndEventArgs>(Runtime_ProgressEnd);

        }


        public event EventHandler<ActionRuntimeEventArgs>   Aborting;
        public event EventHandler<ActionRuntimeEventArgs>   AfterExecute;
        public event EventHandler<ActionRuntimeEventArgs>   BeforeExecute;
        public event EventHandler<ActionRuntimeEventArgs>   OnException;
        public event EventHandler<ProgressEventArgs>        Progress;
        public event EventHandler<ProgressEndEventArgs>     ProgressEnd;
        public event EventHandler<ProgressStartEventArgs>   ProgressStart;


        public void Continue()
        {
            _State.Context.Runtime.Continue();
        }

        public void Pause()
        {
            _State.Context.Runtime.Pause();
        }

        public void Start()
        {
            StartExecute("");
        }

        public void Start(string actionName)
        {
            StartExecute(actionName);
        }

        public void Stop()
        {
            _State.Context.Runtime.Abort();
        }
        internal void StartExecute(string actionName)
        {
            //dont run if its paused.
            if (_State.Context.Runtime.Paused)
            {
                //_State.Context.Runtime.Paused = false;
                return;
            }

            int found = 0;

        Start: // i know , i know!!! look at the msil - it will appear a lot neater!
            if (!string.IsNullOrEmpty(actionName))
            {
                found = _State.Actions.FindIndex(delegate(IAction x) { return IsActionEqual(x, actionName); });
            }

            if (found < 0)
                found = 0;

            _State.Context.Runtime.BeginProgress(found, _State.Actions.Count);
            

            for (int i = found; i < _State.Actions.Count; i++)
            {
                IAction action = _State.Actions[i];
                RuntimeService service = _State.GetService(action);

                service.WaitIfPaused(action); // check if paused before and after we execute.

                _State.Context.Runtime.UpdateProgress(i);

                ExecuteServiceForAction(service, action);

                _State.Context.Runtime.LastRunAction = action;

                service.WaitIfPaused(action);

                if (service.MustQuit(action))
                {
                    return;
                }
                
                if (action is GoToAction)
                {
                    GoToAction gotoAction = (GoToAction)action;
                    actionName = gotoAction.GetActionName();

                    if (gotoAction.Evaluate())
                    {
                        goto Start;
                    }
                }
            }

            _State.Context.Runtime.EndProgress();

            _State.Context.Runtime.ResetRuntimeState();
        }


        private void ExecuteServiceForAction(RuntimeService service, IAction action)
        {
            if (service == null)
                return;

            try
            {
                ThrowBeforeExecute(action, new ActionRuntimeEventArgs());
                service.Execute(action);
                ThrowAfterExecute(action, new ActionRuntimeEventArgs());
            }
            catch (Exception ex)
            {
                action.LastException = new RuleActionException("ActionRuntimeExecutor Failed. See inner exception for details.", ex);
                action.Status = ActionStatus.Errored;

                if (action.Status == ActionStatus.Errored)
                {
                    if (OnException != null)
                        OnException(this, new ActionRuntimeEventArgs(action));
                }
            }
        }

        private static bool IsActionEqual(IAction x, string actionName)
        {
            if (x.Name == actionName)
                return true;
            return
                false;
        }

        void Runtime_Progress(object sender, ProgressEventArgs e)
        {
            if (Progress != null)
            {
                Progress(sender, e);
            }
        }

        void Runtime_ProgressEnd(object sender, ProgressEndEventArgs e)
        {
            if (ProgressEnd != null)
            {
                ProgressEnd(sender, e);
            }
        }

        void Runtime_ProgressStart(object sender, ProgressStartEventArgs e)
        {
            if (ProgressStart != null)
            {
                ProgressStart(sender, e);
            }
        }

        void ThrowBeforeExecute(object sender, ActionRuntimeEventArgs e)
        {
            if (BeforeExecute != null)
            {
                BeforeExecute(sender, e);
            }
        }

        void ThrowAfterExecute(object sender, ActionRuntimeEventArgs e)
        {
            if (AfterExecute != null)
            {
                AfterExecute(sender, e);
            }
        }

    }
}
