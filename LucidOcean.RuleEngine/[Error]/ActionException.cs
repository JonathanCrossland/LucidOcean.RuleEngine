using System;

namespace LucidOcean.RuleEngine
{

    [Serializable]
    public class RuleEngineException : Exception
    {
        public RuleEngineException() { }
        public RuleEngineException(string message) : base(message) { }
        public RuleEngineException(string message, Exception inner) : base(message, inner) { }
      
    }

    [System.Serializable]
    public class RuleActionException : RuleEngineException
    {
        public RuleActionException() { }
        public RuleActionException(string message) : base(message) { }
        public RuleActionException(string message, Exception inner) : base(message, inner) { }
      

    }

    [System.Serializable]
    public class RuleRuntimeException : RuleEngineException
    {
        public RuleRuntimeException() { }
        public RuleRuntimeException(string message) : base(message) { }
        public RuleRuntimeException(string message, Exception inner) : base(message, inner) { }
       

    }

    [System.Serializable]
    public class RuleProviderException : RuleEngineException
    {
        public RuleProviderException() { }
        public RuleProviderException(string message) : base(message) { }
        public RuleProviderException(string message, Exception inner) : base(message, inner) { }
      
       

    }

}
