/*=====================================================================
Authors: Jonathan Crossland
Copyright © Jonathan Crossland. All Rights Reserved.

The license is on the root of the main source-code directory.
=====================================================================*/

using System;

namespace LucidOcean.RuleEngine
{
    public class ActionLogEventArgs : EventArgs
    {

        private string _Value;


        public ActionLogEventArgs(string value)
        {
            _Value = value;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Value
        {
            get
            {
                return _Value;
            }
            set
            {
                _Value = value;
            }
        }
    }
}
