/*=====================================================================
Authors: Jonathan Crossland
Copyright © Jonathan Crossland. All Rights Reserved.

The license is on the root of the main source-code directory.
=====================================================================*/

using System;
using System.Collections.Generic;


namespace LucidOcean.RuleEngine
{
    

    public class ActionRuntimeEventArgs : EventArgs
    {

        private IAction _Action;

        public ActionRuntimeEventArgs(IAction action)
        {
            _Action = action;
        }

        public ActionRuntimeEventArgs()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public IAction Action
        {
            get
            {
                return _Action;
            }
            set
            {
                _Action = value;
            }
        }

    }

    public class RuntimeEventArgs : EventArgs
    {
      
        private List<IAction> _Actions;


        public RuntimeEventArgs(List<IAction> state)
        {
            _Actions = state;
        }

        public RuntimeEventArgs()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public List<IAction> Actions
        {
            get
            {
                return _Actions;
            }
            set
            {
                _Actions = value;
            }
        }

    }
}
