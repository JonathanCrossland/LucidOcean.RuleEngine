/*=====================================================================
Authors: Jonathan Crossland
Copyright © Jonathan Crossland. All Rights Reserved.

The license is on the root of the main source-code directory.
=====================================================================*/

using System;
using System.Collections;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using LucidOcean.RuleEngine.Configuration;
using LucidOcean.RuleEngine.Services;


namespace LucidOcean.RuleEngine
{
    /// <summary>
    /// 
    /// </summary>
    public static class Factory
    {

        private static Hashtable _ActionInstances = new Hashtable();
        private static Hashtable _ActionServiceInstances = new Hashtable();

        /// <summary>
        /// Creates an Action based on the ActionElement configuration node.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static ActionBase CreateAction(ActionElement element)
        {

            ActionBase obj = null;

            if (_ActionInstances == null)
                _ActionInstances = new Hashtable();

            try
            {
                string[] aryAssemblyType = GetAssemblyAndTypeAsStringArray(element.TypeName);
                object obj1 = GetActionInstance(aryAssemblyType[0]);

                if (obj == null)
                    obj1 = CreateAction(aryAssemblyType[1], aryAssemblyType[0]);

                if (obj1 != null)
                    obj = (ActionBase)obj1;

                if (obj!=null)
                    obj.Name = element.Name;

                if (obj is CompositeAction)
                {
                    if (element.Actions.Count > 0)
                    {
                        foreach (ActionElement el in element.Actions)
                        {
                            object objItem = CreateAction(el);

                            ((CompositeAction)obj).Items.Add((ActionBase)objItem);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new RuleActionException($"CreateAction ", ex);
            }

            return (ActionBase)obj;
        }

        /// <summary>
        /// Creates an instance of the object specified by assembly and typename
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public static Object CreateAction(string assemblyName, string typeName)
        {
            try
            {
                object obj = null;

                System.Reflection.Assembly toLoad = System.Reflection.Assembly.Load(new AssemblyName(assemblyName));

                Type type = toLoad.GetType(typeName);
                obj = Activator.CreateInstance(type);

                return obj;

            }
            catch (Exception ex)
            {
                throw new RuleEngineException($"CreateAction ${assemblyName}:${typeName}", ex);
            }
        }

        public static RuntimeService CreateActionService(RuntimeServiceElement element)
        {
            Object obj = null;

            if (_ActionServiceInstances == null)
                _ActionServiceInstances = new Hashtable();

            try
            {
                string[] aryAssemblyType = GetAssemblyAndTypeAsStringArray(element.TypeName);
                obj = GetActionServiceInstance(element.TypeName);

                if (obj == null)
                    obj = CreateActionInstance(aryAssemblyType[1], aryAssemblyType[0]);

            }
            catch (Exception ex)
            {
                throw new RuleRuntimeException($"CreateActionService ", ex);
            }

            return (RuntimeService)obj;
        }

        public static Object CreateActionInstance(string assemblyName, string typeName)
        {
            try
            {
                object obj = null;

                System.Reflection.Assembly toLoad = System.Reflection.Assembly.Load(new AssemblyName(assemblyName));

                Type type = toLoad.GetType(typeName);
                object[] param = new object[] { ActionManagement.CreateInstance() };
                obj = Activator.CreateInstance(type, param);

                return obj;
            }
            catch (Exception ex)
            {
                throw new RuleActionException($"CreateAction ", ex);
            }
        }

        public static Object CreateInstance(string assemblyName, string typeName)
        {
            try
            {
                object obj = null;

                System.Reflection.Assembly toLoad = System.Reflection.Assembly.Load(new AssemblyName(assemblyName));

                Type type = toLoad.GetType(typeName);
            
                obj = Activator.CreateInstance(type, null);

                return obj;
            }
            catch (Exception ex)
            {
                throw new RuleActionException($"CreateInstance", ex);
            }
        }
        /// <summary>
        /// Gets an instance from the cache
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public static object GetActionInstance(string typeName)
        {
            if (_ActionInstances[typeName] != null)
            {
                return (ActionBase)_ActionInstances[typeName];
            }

            return null;
        }

        /// <summary>
        /// Gets an instance from the cache
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        private static object GetActionServiceInstance(string typeName)
        {
            if (_ActionServiceInstances[typeName] != null)
            {
                return (RuntimeService)_ActionServiceInstances[typeName];
            }

            return null;
        }

        /// <summary>
        /// Splits the config string 
        /// </summary>
        /// <param name="assemblyString"></param>
        /// <returns></returns>
        public static string[] GetAssemblyAndTypeAsStringArray(string assemblyString)
        {
            return assemblyString.Split(new Char[] { ',' }, 2, StringSplitOptions.RemoveEmptyEntries);
        }

		
    
        public static ActionListProvider CreateActionListProvider(string typeName)
        {
            string[] aryAssemblyType = GetAssemblyAndTypeAsStringArray(typeName);
            if (aryAssemblyType != null)
            {
                if (aryAssemblyType.Length == 2)
                {
                    return (ActionListProvider) CreateInstance(aryAssemblyType[1].ToString(), aryAssemblyType[0].ToString());
                }
            }

            return new ActionListProvider(); // return a default
        }
    }
}
