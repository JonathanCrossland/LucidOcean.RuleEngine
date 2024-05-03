/*=====================================================================
Authors: Jonathan Crossland
Copyright © Jonathan Crossland. All Rights Reserved.

The license is on the root of the main source-code directory.
=====================================================================*/

using System; 

namespace LucidOcean.RuleEngine
{
    interface IActionRuntimeExecutor
    {
        void Start();
        void Stop();

        event EventHandler<ActionRuntimeEventArgs> Aborted;
        event EventHandler<ActionRuntimeEventArgs> Started;
        event EventHandler<ActionRuntimeEventArgs> Stopped;
    }
}
