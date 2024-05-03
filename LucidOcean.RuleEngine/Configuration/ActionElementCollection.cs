/*=====================================================================
Authors: Jonathan Crossland
Copyright © Jonathan Crossland. All Rights Reserved.

The license is on the root of the main source-code directory.
=====================================================================*/

using System.Configuration; 

namespace LucidOcean.RuleEngine.Configuration
{
    /// <summary>
    /// Collection of ActionElements
    /// </summary>
    public class ActionElementCollection : ConfigurationElementCollectionBase<ActionElement> //ConfigurationElementCollection
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




        protected override void SetReadOnly()
        {
            //base.SetReadOnly();
        }
        protected override ConfigurationElement CreateNewElement()
        {
            return new ActionElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ActionElement)element).Name;
        }

        protected override bool IsElementName(string elementName)
        {
            if ((string.IsNullOrEmpty(elementName)) || (elementName != "ActionElement"))
                return false;

            return true;
        }


    }
}
