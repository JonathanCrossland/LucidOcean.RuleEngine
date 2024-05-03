/*=====================================================================
Authors: Jonathan Crossland
Copyright © Jonathan Crossland. All Rights Reserved.

The license is on the root of the main source-code directory.
=====================================================================*/

using System.Configuration;


namespace LucidOcean.RuleEngine.Configuration
{

    public class ActionElement : ConfigurationElement
    {
		

        [ConfigurationProperty("Actions", IsDefaultCollection = false)]
        public ActionElementCollection Actions
        {
            get
            {
                return (ActionElementCollection)base["Actions"];
            }
        }

        /// <summary>
        /// The name of the ActionElement. A unique name should be used.
        /// </summary>
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get
            {
                return (string)this["name"];
            }
            set
            {
                this["name"] = value;
            }
        }

        /// <summary>
        /// The name of the Type
        /// </summary>
        [ConfigurationProperty("typeName", IsRequired = false)]
        public string TypeName
        {
            get
            {
                return (string)this["typeName"];
            }
            set
            {
                this["typeName"] = value;
            }
        }

    }
}
