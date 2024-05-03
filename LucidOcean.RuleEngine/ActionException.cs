/*=====================================================================
Authors: Jonathan Crossland
Copyright © Jonathan Crossland. All Rights Reserved.

The license is on the root of the main source-code directory.
=====================================================================*/

using System;
using System.Runtime.Serialization;

namespace LucidOcean.RuleEngine
{
    /// <summary>
    /// All Exceptions thrown from this Assembly should be wrapped by this Exception.
    /// </summary>
    [Serializable]
    public class ActionException : Exception
    {
        public ActionException()
            : base()
        {
        }

        public ActionException(string message)
            : base(message)
        {
        }

        public ActionException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected ActionException(SerializationInfo info, StreamingContext context)
             : base(info, context) 
        { 
        }
        
    }

}
