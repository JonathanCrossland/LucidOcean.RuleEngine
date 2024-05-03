/*=====================================================================
Authors: Jonathan Crossland
Copyright © Jonathan Crossland. All Rights Reserved.

The license is on the root of the main source-code directory.
=====================================================================*/

using System.Configuration; 

namespace LucidOcean.RuleEngine.Configuration
{

    public partial class ActionRuntimeSection : ConfigurationSection
    {

        private static volatile ActionRuntimeSection _ActionRuntimeSection;
        private static object syncRoot = new object();

       
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static ActionRuntimeSection CreateInstance()
        {
            if (_ActionRuntimeSection == null)
            {
                lock (syncRoot)
                {
                    
                    if (_ActionRuntimeSection == null)
                        _ActionRuntimeSection = (ActionRuntimeSection)System.Configuration.ConfigurationManager.GetSection("ActionRuntimeSection"); 

                    if (_ActionRuntimeSection == null)
                        _ActionRuntimeSection = new ActionRuntimeSection();
                }
            }
            
            return _ActionRuntimeSection;
        }

        
    }
    
    /// <summary>
    /// 
    /// </summary>
    public partial class ActionRuntimeSection : ConfigurationSection
    {
	
        public ActionRuntimeSection()
        {
        }

		

        [ConfigurationProperty("Actions", IsDefaultCollection = false)]
        public ActionElementCollection Actions
        {
            get
            {
                if (base["Actions"] == null)
                    base["Actions"] = new ActionElementCollection();

                return (ActionElementCollection)base["Actions"];
            }
        }

        [ConfigurationProperty("RuntimeServices", IsDefaultCollection = false)]
        public RuntimeServiceElementCollection RuntimeServices
        {
            get
            {
                if (base["RuntimeServices"] == null)
                    base["RuntimeServices"] = new ActionElementCollection();

                return (RuntimeServiceElementCollection)base["RuntimeServices"];
            }
        }

        [ConfigurationProperty("RuntimeProviders", IsDefaultCollection = false)]
        public RuntimeProviderElementCollection RuntimeProviders
        {
            get
            {
                if (base["RuntimeProviders"] == null)
                    base["RuntimeProviders"] = new ActionElementCollection();

                return (RuntimeProviderElementCollection)base["RuntimeProviders"];
            }
        }

    }
}
