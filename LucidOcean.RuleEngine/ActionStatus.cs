/*=====================================================================
Authors: Jonathan Crossland
Copyright © Jonathan Crossland. All Rights Reserved.

The license is on the root of the main source-code directory.
=====================================================================*/


namespace LucidOcean.RuleEngine
{
    public enum ActionStatus
    {
        /// <summary>
        /// 
        /// </summary>
        Unknown,
        /// <summary>
        /// Typically set as the last line in an Execute() action
        /// </summary>
        Ready,
        /// <summary>
        /// Typically set as the first line in an Execute() action
        /// </summary>
        Busy,
        /// <summary>
        /// Must be set to Errored if an Exception occurs.
        /// </summary>
        Errored,
        /// <summary>
        /// Must be set when the runtime goes into a paused state
        /// </summary>
        Paused,
        /// <summary>
        /// When aborting. but not aborted.
        /// </summary>
        Aborting,
        /// <summary>
        /// Runtime is complete, no processing to be done.
        /// </summary>
        Complete

    }
}
