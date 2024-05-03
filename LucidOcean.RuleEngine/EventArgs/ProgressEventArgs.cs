/*=====================================================================
Authors: Jonathan Crossland
Copyright © Jonathan Crossland. All Rights Reserved.

The license is on the root of the main source-code directory.
=====================================================================*/

using System;
using System.Collections.Generic;


namespace LucidOcean.RuleEngine
{

    public class ProgressEventArgs : EventArgs
    {
        private int _Value;

        public ProgressEventArgs(int value)
        {
            _Value = value;
        }


        /// <summary>
        /// 
        /// </summary>
        public int Value
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
