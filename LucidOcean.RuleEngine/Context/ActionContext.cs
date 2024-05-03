/*=====================================================================
Authors: Jonathan Crossland
Copyright © Jonathan Crossland. All Rights Reserved.

The license is on the root of the main source-code directory.
=====================================================================*/

using System;
using System.Collections;

namespace LucidOcean.RuleEngine.Context
{
    
    public partial class ActionContext //partial containing static members
    {
      
        private static volatile ActionContext _ActionContext;
        private static object syncRoot = new object();


        public static void DestroyInstance()
        {
            _ActionContext = null;
        }
        
    }

    public partial class ActionContext
    {

        private Runtime _Runtime;
        private Hashtable _Properties;
        private IConfigurationAccessor _IConfiguration;
        private ActionBase _LastRunAction;

        public event EventHandler<ActionLogEventArgs> OnWriteToLog;


        public void WriteToLog(string value)
        {
            if (OnWriteToLog != null)
                OnWriteToLog(this, new ActionLogEventArgs(value));
        }


        public ActionContext()
        {
            _IConfiguration = new AppSettingsAdaptor();
        }
       

        /// <summary>
        /// 
        /// </summary>
        public Hashtable Properties
        {
            get
            {
                if (_Properties == null)
                    _Properties = new Hashtable();

                return _Properties;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Runtime Runtime
        {
            get
            {
                if (_Runtime == null)
                    _Runtime = new Runtime();

                return _Runtime;
            }
            set
            {
                _Runtime = value;
            }
        }

     
        /// <summary>
        /// 
        /// </summary>
        public IConfigurationAccessor Configuration
        {
            get
            {
                return (IConfigurationAccessor)_IConfiguration;
            }
        }

        
    }
}
