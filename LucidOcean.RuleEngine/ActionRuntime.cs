/*=====================================================================
Authors: Jonathan Crossland
Copyright © Jonathan Crossland. All Rights Reserved.

The license is on the root of the main source-code directory.
=====================================================================*/
using System;


namespace LucidOcean.RuleEngine
{
    public class ActionRuntime : IActionRuntimeExecutor
    {
		
        private ActionException         _Exception;
        private ActionRuntimeExecutor   _Executor;
        private ActionRuntimeState      _State;
        private ActionStatus            _Status;


        internal ActionRuntime(ActionRuntimeState state)
        {

            if (state == null)
                throw new ActionException("Parameter ActionRuntimeState state cannot be null");

            _State = state;
            Status = ActionStatus.Ready;

            _Executor = new ActionRuntimeExecutor(_State);
            

            //subscribe to events - macke sure the dispose is unhooking all the same events subscribed to here
            _Executor.Aborting += new EventHandler<ActionRuntimeEventArgs>(_Executor_Aborting);
            _Executor.BeforeExecute += new EventHandler<ActionRuntimeEventArgs>(_Executor_BeforeExecute);
            _Executor.AfterExecute += new EventHandler<ActionRuntimeEventArgs>(_Executor_AfterExecute);
            _Executor.OnException += new EventHandler<ActionRuntimeEventArgs>(_Executor_OnException);
            //_Executor.Pausing += new EventHandler<ActionRuntimeEventArgs>(_Executor_Pausing);

            _Executor.ProgressStart += new EventHandler<ProgressStartEventArgs>(_Executor_ProgressStart);
            _Executor.Progress += new EventHandler<ProgressEventArgs>(_Executor_Progress);
            _Executor.ProgressEnd += new EventHandler<ProgressEndEventArgs>(_Executor_ProgressEnd);

        }

        ~ActionRuntime()
        {
            if (_Executor != null)
            {
                _Executor.Aborting -= new EventHandler<ActionRuntimeEventArgs>(_Executor_Aborting);
                _Executor.BeforeExecute -= new EventHandler<ActionRuntimeEventArgs>(_Executor_BeforeExecute);
                _Executor.AfterExecute -= new EventHandler<ActionRuntimeEventArgs>(_Executor_AfterExecute);
                _Executor.OnException -= new EventHandler<ActionRuntimeEventArgs>(_Executor_OnException);
                //_Executor.Pausing += new EventHandler<ActionRuntimeEventArgs>(_Executor_Pausing);

                _Executor.ProgressStart -= new EventHandler<ProgressStartEventArgs>(_Executor_ProgressStart);
                _Executor.Progress -= new EventHandler<ProgressEventArgs>(_Executor_Progress);
                _Executor.ProgressEnd -= new EventHandler<ProgressEndEventArgs>(_Executor_ProgressEnd);

            }
        }

        public bool IsAborting
        {
            get
            {
                return _State.Context.Runtime.Aborting;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsPaused
        {
            get
            {
                return _State.Context.Runtime.Paused;
            }
        }

        public bool IsRunning
        {
            get
            {
                return !_State.Context.Runtime.Paused;
            }
        }

        public bool IsStopping
        {
            get
            {
                return _State.Context.Runtime.Stopping;
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
            protected set
            {
                _Exception = value;
            }
        }

        internal ActionRuntimeState State
        {
            get 
            { 
                return _State; 
            }
            set 
            { 
                _State = value; 
            }
        }

        /// <summary>
        /// Returns the ActionStatus. The Actions Framework occasionally requests a status, on which it will act.
        /// If there is an Exception available, please set the Status to Error
        /// </summary>
        /// <returns>ActionStatus</returns>
        public ActionStatus Status
        {
            get
            {
                return _Status;
            }
            protected set
            {
                _Status = value;
            }

        }


        public event EventHandler<ActionRuntimeEventArgs> Aborted;

        public event EventHandler<ActionRuntimeEventArgs> Aborting;

        public event EventHandler<ActionRuntimeEventArgs> AfterExecute;

        public event EventHandler<ActionRuntimeEventArgs> BeforeExecute;

        public event EventHandler<ActionRuntimeEventArgs> OnComplete;

        //Raise any exceptions   
        public event EventHandler<ActionRuntimeEventArgs> OnException;

        public event EventHandler<ActionRuntimeEventArgs> Paused;

        //runtime raising events raised by actions
        public event EventHandler<ProgressEventArgs> Progress;

        public event EventHandler<ProgressEndEventArgs> ProgressEnd;

        public event EventHandler<ProgressStartEventArgs> ProgressStart;

        //runtime raising runtime events
        public event EventHandler<ActionRuntimeEventArgs> Started;

        public event EventHandler<ActionRuntimeEventArgs> Stopped;

        public void ClearStatus()
        {
            _State.Context.Runtime.Paused = false;
            _State.Context.Runtime.Aborting = false;
            _State.Context.Runtime.Stopping = false;
        }

        public void Continue()
        {
            _Executor.Continue();
        }

        public void Pause()
        {
            _Executor.Pause();

            ThrowPaused(this,new ActionRuntimeEventArgs());
        }

        public void Start()
        {
            Start(""); // no action specified means start from beginnng.
        }

        /// <summary>
        /// Starts the Runtime from a specific point, demarcated by the action.
        /// </summary>
        /// <param name="actionName"></param>
        public void Start(string actionName)
        {
            try
            {
               
                Initialize();

                ThrowStarted(this, new ActionRuntimeEventArgs());
                _Status = ActionStatus.Busy;

                //MAIN RUN START
                //this calls a chain of executing services.
                _Executor.Start(actionName);

                if (_State.Context.Runtime.Aborting)
                {
                    if (_Status != ActionStatus.Errored)
                    {
                        _Status = ActionStatus.Aborting;
                        ThrowAborted(this, new ActionRuntimeEventArgs());
                    }
                    else
                    {
                        _Executor.Stop();
                    }
                }
                else
                {
                    _Executor.Stop();
                    _Status = ActionStatus.Complete;
                    ThrowOnComplete(this,new ActionRuntimeEventArgs());
                }

            }
            catch (Exception ex)
            {
                _State.Context.WriteToLog(ex.Message);
                LastException = new ActionException("Action Execute Failed. See inner exception for details.", ex);
                Status = ActionStatus.Errored;

                if (OnException != null)
                    OnException(this, new ActionRuntimeEventArgs());
            }
        }

        public void Stop()
        {
            _Executor.Stop();

            ThrowStopped(this,new ActionRuntimeEventArgs());

        }

        void _Executor_Aborting(object sender, ActionRuntimeEventArgs e)
        {
            _Status = ActionStatus.Aborting;
            ThrowAborting(sender, e);
        }

        void _Executor_AfterExecute(object sender, ActionRuntimeEventArgs e)
        {
            ThrowAfterExecute(sender, e);
        }

        void _Executor_BeforeExecute(object sender, ActionRuntimeEventArgs e)
        {
            ThrowBeforeExecute(sender, e);
        }

        void _Executor_OnException(object sender, ActionRuntimeEventArgs e)
        {

            this.LastException = e.Action.LastException;

            _Status = ActionStatus.Errored;

            if (OnException != null)
                OnException(sender, e);

            _Executor.Stop();
        }

        void _Executor_Progress(object sender, ProgressEventArgs e)
        {
            ThrowProgress(sender, e);
        }

        void _Executor_ProgressEnd(object sender, ProgressEndEventArgs e)
        {
            ThrowProgressEnd(sender, e);
        }

        void _Executor_ProgressStart(object sender, ProgressStartEventArgs e)
        {
            _Status = ActionStatus.Busy;
            ThrowProgressStart(sender, e);
        }

        /// <summary>
        /// Builds a list of Types(Actions) from the configured Actions.
        /// </summary>
        private void Initialize()
        {

            try
            {
                if (_State.Actions != null)
                    _State.Actions.Clear();

                ActionListProvider stateGetActionListProvider = _State.GetActionListProvider();
                _State.Actions.AddRange(stateGetActionListProvider.BuildActionsCollection(_State.ActionRuntimeSection, _State.Context));
                
               
            }
            catch (Exception ex)
            {
                _State.Context.WriteToLog(ex.Message);
                LastException = new ActionException("Action Execute Failed. See inner exception for details.", ex); ;
                Status = ActionStatus.Errored;
            }
        }

	
        internal void RaiseAborting(IAction action)
        {
            ThrowAborting(this, new ActionRuntimeEventArgs(action));
        }

        internal void RaiseAfterExecute(IAction action)
        {
            ThrowAfterExecute(this, new ActionRuntimeEventArgs(action));
        }

        internal void RaiseBeforeExecute(IAction action)
        {
            ThrowBeforeExecute(this, new ActionRuntimeEventArgs(action));
        }

        internal void RaisePausedEvent(IAction action)
        {
            ThrowPaused(this, new ActionRuntimeEventArgs(action));
        }


        private void ThrowAborted(object sender, ActionRuntimeEventArgs e)
        {
            try
            {
                if (Aborted != null)
                    Aborted(sender, e);
            }
            catch (Exception ex)
            {
                _State.Context.WriteToLog(ex.Message);
                throw new ActionException("Aborted event handler caused an Exception", ex);
            }
        }
        private void ThrowAborting(object sender, ActionRuntimeEventArgs e)
        {
            try
            {
                if (Aborting != null)
                    Aborting(sender, e);
            }
            catch (Exception ex)
            {
                _State.Context.WriteToLog(ex.Message);
                throw new ActionException("Aborted event handler caused an Exception", ex);
            }
        }
        private void ThrowAfterExecute(object sender, ActionRuntimeEventArgs e)
        {
            try
            {
                if (AfterExecute != null)
                    AfterExecute(sender, e);
            }
            catch (Exception ex)
            {
                _State.Context.WriteToLog(ex.Message);
                throw new ActionException("AfterExecute event handler caused an Exception", ex);
            }
        }
        private void ThrowBeforeExecute(object sender, ActionRuntimeEventArgs e)
        {
            try
            {
                if (BeforeExecute != null)
                    BeforeExecute(sender, e);
            }
            catch (Exception ex)
            {
                _State.Context.WriteToLog(ex.Message);
                throw new ActionException("BeforeExecute event handler caused an Exception", ex);
            }
        }
        private void ThrowOnComplete(object sender, ActionRuntimeEventArgs e)
        {
            try
            {
                if (OnComplete != null)
                    OnComplete(sender, e);
            }
            catch (Exception ex)
            {
                _State.Context.WriteToLog(ex.Message);
                throw new ActionException("OnComplete event handler caused an Exception", ex);
            }
        }
        private void ThrowPaused(object sender, ActionRuntimeEventArgs e)
        {
            try
            {
                if (Paused != null)
                    Paused(sender, e);
            }
            catch (Exception ex)
            {
                _State.Context.WriteToLog(ex.Message);
                throw new ActionException("Paused event handler caused an Exception", ex);
            }
        }
        private void ThrowProgress(object sender, ProgressEventArgs e)
        {
            try
            {
                if (Progress != null)
                    Progress(sender, e);
            }
            catch (Exception ex)
            {
                _State.Context.WriteToLog(ex.Message);
                throw new ActionException("AfterExecute event handler caused an Exception", ex);
            }
        }
        private void ThrowProgressEnd(object sender, ProgressEndEventArgs e)
        {
            try
            {
                if (ProgressEnd != null)
                    ProgressEnd(sender, e);
            }
            catch (Exception ex)
            {
                _State.Context.WriteToLog(ex.Message);
                throw new ActionException("AfterExecute event handler caused an Exception", ex);
            }
        }
        private void ThrowProgressStart(object sender,  ProgressStartEventArgs e)
        {
            try
            {
                if (ProgressStart != null)
                    ProgressStart(sender, e);
            }
            catch (Exception ex)
            {
                _State.Context.WriteToLog(ex.Message);
                throw new ActionException("AfterExecute event handler caused an Exception", ex);
            }
        }
        private void ThrowStarted(object sender, ActionRuntimeEventArgs e)
        {
            try
            {
                if (Started != null)
                    Started(sender, e);
            }
            catch (Exception ex)
            {
                _State.Context.WriteToLog(ex.Message);
                throw new ActionException("Started event handler caused an Exception", ex);
            }
        }
        private void ThrowStopped(object sender, ActionRuntimeEventArgs e)
        {
            try
            {
                 if (Stopped != null)
                    Stopped(sender, e);
        
            }
            catch (Exception ex)
            {
                _State.Context.WriteToLog(ex.Message);
                throw new ActionException("Stopped event handler caused an Exception", ex);
            }
        }
       
    }
}
