/*=====================================================================
Authors: Jonathan Crossland
Copyright © Jonathan Crossland. All Rights Reserved.

The license is on the root of the main source-code directory.
=====================================================================*/

using System;


namespace LucidOcean.RuleEngine.Context
{
    public class Runtime
    {
	

        internal event EventHandler<ProgressEventArgs> Progress;
        internal event EventHandler<ProgressEndEventArgs> ProgressEnd;
        internal event EventHandler<ProgressStartEventArgs> ProgressStart;


        private bool _Aborting;
        private bool _Stopping;
        private bool _Paused;
        private IAction _LastRunAction;

        internal Runtime()
        {
        }

        /// <summary>
        /// Aborts the entire Runtime Execution
        /// </summary>
        public void Abort()
        {
            _Aborting = true;
            _Paused = false; ;
            _Stopping = true;
        }

        public void Pause()
        {
            _Paused = true;
        }

        public void Continue()
        {
            _Paused = false;
        }

        public void ResetRuntimeState()
        {
            _Aborting = false;
            _Paused = false;
            _Stopping = false;

        }
        public void BeginProgress(int value, int maxValue)
        {
            if (maxValue > 0)
            {
                if (ProgressStart != null)
                {
                    ProgressStart(this, new ProgressStartEventArgs(maxValue));
                }
            }

            UpdateProgress(value);
        }

        public void UpdateProgress(int value)
        {
            if (value > 0)
            {
                if (Progress != null)
                {
                    Progress(this, new ProgressEventArgs(value));
                }
            }
        }

        public void EndProgress()
        {
            if (ProgressEnd != null)
            {
                ProgressEnd(this, new ProgressEndEventArgs(0));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        internal bool Aborting
        {
            get
            {
                return _Aborting;
            }
            set
            {
                _Aborting = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        internal bool Stopping
        {
            get
            {
                return _Stopping;
            }
            set
            {
                _Stopping = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        internal bool Paused
        {
            get
            {
                return _Paused;
            }
            set
            {
                _Paused = value;
            }
        }

        public IAction LastRunAction
        {
            get
            {
                return _LastRunAction;
            }
            set
            {
                _LastRunAction = value;
            }
        }
    }
}
