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
    public class RuntimeProviderElementCollection : ConfigurationElementCollectionBase<RuntimeProviderElement> //ConfigurationElementCollection
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

        public new RuntimeProviderElement this[int index]
        {
            get
            {
                return (RuntimeProviderElement)base.BaseGet(index);
            }
        }

        public new RuntimeProviderElement this[string key]
        {
            get
            {
                return (RuntimeProviderElement)base.BaseGet(key);
            }
        }

     

        // Protected Methods (3) 

        protected override ConfigurationElement CreateNewElement()
        {
            return new RuntimeProviderElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((RuntimeProviderElement)element).ProviderTypeName;
        }

        protected override bool IsElementName(string elementName)
        {
            if ((string.IsNullOrEmpty(elementName)) || (elementName != "RuntimeProviderElement"))
                return false;

            return true;
        }

     
    }

    public class RuntimeProviderElement : ConfigurationElement
    {
       

        /// <summary>
        /// The Type you wish to provider a provider for
        /// </summary>
        [ConfigurationProperty("providerTypeName", IsRequired = true)]
        public string ProviderTypeName
        {
            get
            {
                return (string)this["providerTypeName"];
            }
            set
            {
                this["providerTypeName"] = value;
            }
        }

        /// <summary>
        /// The provider, who derives from ActionListProvier as a provider for the value in ProviderTypeName
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
