/*=====================================================================
Authors: Jonathan Crossland
Copyright © Jonathan Crossland. All Rights Reserved.

The license is on the root of the main source-code directory.
=====================================================================*/

using System.Configuration;

namespace LucidOcean.RuleEngine.Configuration
{

    /// <summary>
    /// Collection of file sets to watch for changes.
    /// </summary>
    public class RuntimeServiceElementCollection :  ConfigurationElementCollectionBase<RuntimeServiceElement> //ConfigurationElementCollection
    {
		
        /// <summary>
        /// Collection type of this collection.
        /// </summary>
        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.BasicMapAlternate;
            }
        }

        public new RuntimeServiceElement this[int index]
        {
            get
            {
                return (RuntimeServiceElement)base.BaseGet(index);
            }
        }

        public new RuntimeServiceElement this[string key]
        {
            get
            {
                return (RuntimeServiceElement)base.BaseGet(key);
            }
        }


        protected override ConfigurationElement CreateNewElement()
        {
            return new RuntimeServiceElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((RuntimeServiceElement)element).ActionTypeName;
        }

        protected override bool IsElementName(string elementName)
        {
            if ((string.IsNullOrEmpty(elementName)) || (elementName != "RuntimeServiceElement"))
                return false;

            return true;
        }

		
    }

    public class RuntimeServiceElement : ConfigurationElement
    {
		

        /// <summary>
        /// 
        /// </summary>
        [ConfigurationProperty("actionTypeName", IsRequired = true)]
        public string ActionTypeName
        {
            get
            {
                return (string)this["actionTypeName"];
            }
            set
            {
                this["actionTypeName"] = value;
            }
        }

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
