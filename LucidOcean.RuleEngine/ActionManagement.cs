/*=====================================================================
Authors: Jonathan Crossland
Copyright © Jonathan Crossland. All Rights Reserved.

The license is on the root of the main source-code directory.
=====================================================================*/

using LucidOcean.RuleEngine.Configuration;
using LucidOcean.RuleEngine.Context;
using LucidOcean.RuleEngine.Services;
using System;

namespace LucidOcean.RuleEngine
{
    public partial class ActionManagement
    {
     
        private static volatile ActionManagement _Environment;
        private static object _PadLock = new object();
     
        public static ActionManagement CreateInstance()
        {
            if (_Environment == null)
            {
                lock (_PadLock)
                {
                    if (_Environment == null)
                        _Environment = new ActionManagement();
                }
            }

            return _Environment;
        }

        public static void DestroyInstance()
        {
            _Environment = null;
            ActionContext.DestroyInstance();
        }

    }

    public partial class ActionManagement
    {

        private ActionContext _ActionContext;
        private ActionRuntimeSection _Configuration;
        private string _Name;
        private ActionRuntime _Runtime;
        private ActionRuntimeState _State;

        private ActionManagement()
        {

            _Configuration = ActionRuntimeSection.CreateInstance();
            _ActionContext = new ActionContext();

            _State = new ActionRuntimeState(_ActionContext, _Configuration);
            _Runtime = new ActionRuntime(_State);

            WireEvents();

        }


        ~ActionManagement()
        {
            UnWireEvents();
        }

        private void WireEvents()
        {
            _Runtime.Started += new System.EventHandler<ActionRuntimeEventArgs>(_Runtime_Started);
            _Runtime.Stopped += new System.EventHandler<ActionRuntimeEventArgs>(_Runtime_Stopped);
            _Runtime.Aborted += new System.EventHandler<ActionRuntimeEventArgs>(_Runtime_Aborted);
            _Runtime.OnException += new System.EventHandler<ActionRuntimeEventArgs>(_Runtime_OnException);
            _Runtime.BeforeExecute += new System.EventHandler<ActionRuntimeEventArgs>(_Runtime_BeforeExecute);
            _Runtime.AfterExecute += new EventHandler<ActionRuntimeEventArgs>(_Runtime_AfterExecute);
        }


        private void UnWireEvents()
        {

            _Runtime.Started -= new System.EventHandler<ActionRuntimeEventArgs>(_Runtime_Started);
            _Runtime.Stopped -= new System.EventHandler<ActionRuntimeEventArgs>(_Runtime_Stopped);
            _Runtime.Aborted -= new System.EventHandler<ActionRuntimeEventArgs>(_Runtime_Aborted);
            _Runtime.OnException -= new System.EventHandler<ActionRuntimeEventArgs>(_Runtime_OnException);
            _Runtime.BeforeExecute -= new System.EventHandler<ActionRuntimeEventArgs>(_Runtime_BeforeExecute);
            _Runtime.AfterExecute -= new EventHandler<ActionRuntimeEventArgs>(_Runtime_AfterExecute);
        }
     
        public ActionContext ActionContext
        {
            get
            {
                return _ActionContext;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Configuration.ActionRuntimeSection Configuration
        {
            get
            {
                if (_Configuration == null)
                    _Configuration = ActionRuntimeSection.CreateInstance();

                return _Configuration;
            }
            set
            {
                _Configuration = value;
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
            set
            {
                _Name = value;
            }
        }

        public ActionRuntime Runtime
        {
            get
            {
                return _Runtime;
            }
            set
            {

                if (value == null)
                {
                    if (_Runtime != null)
                        UnWireEvents();
                }

                _Runtime = value;

                if (_Runtime != null)
                {
                    WireEvents();
                }

            }
        }


        /// <summary>
        /// The main execution of actions - ActionRuntime is invoked
        /// </summary>
        public void Execute(string actionName)
        {
            if (IsValidRuntime())
            {
                _Runtime.Start(actionName);
            }
        }

        /// <summary>
        /// checks a few things to see if the runtime will run correctly.
        /// It is not fool proof, but can be expanded with time.
        /// For now it checks actions, init, and if there is a service for each action.
        /// </summary>
        /// <returns></returns>
        public bool IsValidRuntime()
        {
            try
            {
                if (_Configuration.Actions.Count <= 0)
                    return false;

                foreach (ActionElement actionElement in _Configuration.Actions)
                {
                    ActionBase action = _State.GetAction(actionElement);
                    if (action == null) return false;

                    RuntimeService service = _State.GetService(action);
                    if (service == null) return false;


                    action.Initialize(_State.Context);
                }
            }
            catch (Exception ex)
            {
                _State.Context.WriteToLog(ex.Message);
            }

            return true;
        }
        
        void _Runtime_Aborted(object sender, ActionRuntimeEventArgs e)
        {

        }

        void _Runtime_BeforeExecute(object sender, ActionRuntimeEventArgs e)
        {
         
        }


        void _Runtime_AfterExecute(object sender, ActionRuntimeEventArgs e)
        {
        }


        void _Runtime_OnException(object sender, ActionRuntimeEventArgs e)
        {

        }

        void _Runtime_Started(object sender, ActionRuntimeEventArgs e)
        {

        }

        void _Runtime_Stopped(object sender, ActionRuntimeEventArgs e)
        {

        }

    }
}
