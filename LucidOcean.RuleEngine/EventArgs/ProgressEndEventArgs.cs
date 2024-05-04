/*=====================================================================
Authors: Jonathan Crossland
Copyright © Jonathan Crossland. All Rights Reserved.

The license is on the root of the main source-code directory.
=====================================================================*/

using System;
using System.Collections.Generic;


namespace LucidOcean.RuleEngine
{
    public class ProgressEndEventArgs : ProgressEventArgs
    {
        public ProgressEndEventArgs(int value)
            : base(value)
        {
        }

    }
}
