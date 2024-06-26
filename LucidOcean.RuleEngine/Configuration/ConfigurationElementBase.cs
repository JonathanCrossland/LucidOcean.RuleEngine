﻿/*=====================================================================
Authors: Jonathan Crossland
Copyright © Jonathan Crossland. All Rights Reserved.

The license is on the root of the main source-code directory.
=====================================================================*/

using System;
using System.Configuration;


namespace LucidOcean.RuleEngine.Configuration
{

    public abstract class ConfigurationElementCollectionBase<T> : ConfigurationElementCollection where T : ConfigurationElement
    {

        public override ConfigurationElementCollectionType CollectionType
        {
            get 
            { 
                return ConfigurationElementCollectionType.AddRemoveClearMap; 
            }
        }

        public T this[int index]
        {
            get 
            { 
                return (T)base.BaseGet(index); 
            }
            set
            {
                if (base.BaseGet(index) != null)
                    base.BaseRemoveAt(index);

                base.BaseAdd(index, value);
            }
        }

        new public T this[string key]
        {
            get 
            { 
                return (T)base.BaseGet(key); 
            }
        }


        public void Add(T element)
        {
            base.BaseAdd(element);
        }

        public void Clear()
        {
            
            base.BaseClear();
        }

        public bool Contains(string key)
        {
            return base.BaseGet(key) != null;
        }

        public void Remove(string key)
        {
            base.BaseRemove(key);
        }

        public void RemoveAt(int index)
        {
            base.BaseRemoveAt(index);
        }

	    protected override ConfigurationElement CreateNewElement()
        {
            return Activator.CreateInstance<T>();
        }

        protected override void SetReadOnly()
        {
          
        }

    }
}
