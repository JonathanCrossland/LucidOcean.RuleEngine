/*=====================================================================
Authors: Jonathan Crossland
Copyright © Jonathan Crossland. All Rights Reserved.

The license is on the root of the main source-code directory.
=====================================================================*/

using System;
using LucidOcean.RuleEngine.Configuration;
using LucidOcean.RuleEngine.Context;
using LucidOcean.RuleEngine.Services;



namespace LucidOcean.RuleEngine
{
    internal class ActionRuntimeState
    {
        private ActionRuntimeSection _ActionRuntimeSection;
        private ActionListCollection _Actions = new ActionListCollection();
        private ActionContext _Context;
        private ActionListProvider _ActionListProvider;

        internal ActionRuntimeState(ActionContext context, ActionRuntimeSection actionRuntimeSection)
        {
            _Context = context;
            _ActionRuntimeSection = actionRuntimeSection;
        }

        /// <summary>
        /// The Configuration root
        /// </summary>
        public ActionRuntimeSection ActionRuntimeSection
        {
            get
            {
                if (_ActionRuntimeSection == null)
                    _ActionRuntimeSection = new ActionRuntimeSection();

                return _ActionRuntimeSection;
            }
            set
            {
                _ActionRuntimeSection = value;
            }
        }

        /// <summary>
        /// List of Actions -
        /// </summary>
        public ActionListCollection Actions
        {
            get
            {
                if (_Actions == null)
                {
                    _ActionListProvider = GetActionListProvider();
                    _Actions = _ActionListProvider.BuildActionsCollection(_ActionRuntimeSection, _Context);
                }

                if (_Actions == null)
                    _Actions = new ActionListCollection();

                return _Actions;
            }
        }

        /// <summary>
        /// The context passed through the execution change.
        /// </summary>
        public ActionContext Context
        {
            get
            {
                return _Context;
            }
            set
            {
                _Context = value;
            }
        }

        /// <summary>
        /// returns a provider that builds an ActionList
        /// </summary>
        /// <returns></returns>
        internal ActionListProvider GetActionListProvider()
        {
            ActionRuntimeSection section = ActionManagement.CreateInstance().Configuration;
            ActionListProvider provider = null;

            if (section.RuntimeProviders[typeof(ActionListCollection).AssemblyQualifiedName] != null)
            {
                provider = Factory.CreateActionListProvider(section.RuntimeProviders[typeof(ActionListCollection).AssemblyQualifiedName].TypeName);
            }

            if (provider == null)
                provider = new ActionListProvider();

            return provider;
        }

        internal ActionBase GetAction(ActionElement element)
        {
            return (ActionBase)Factory.CreateAction(element);
        }

        /// <summary>
        /// Finds a Service for the given action. Firstly checks configuration, then determines a default.
        /// There should always be a service returned, even if it is the simple RuntimeService base.
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        internal RuntimeService GetService(IAction action)
        {
            try
            {
                ActionRuntimeSection section = ActionManagement.CreateInstance().Configuration;
                Type baseType = action.GetType().BaseType;
                RuntimeService service = null;
                string serviceName = action.GetType().FullName;

                if (!serviceName.Contains(","))
                    serviceName += ", " + action.GetType().Assembly.GetName().Name;

                //added a loop to check base types tooo. 
                while (serviceName != null && baseType != null)
                {
                    if (section.RuntimeServices[serviceName] != null && section.RuntimeServices.Count > 0)
                    {
                        try
                        {
                            service = Factory.CreateActionService(section.RuntimeServices[serviceName]);

                            return service;
                        }
                        catch (Exception ex)
                        {
                            _Context.WriteToLog(ex.Message);
                            //throw; ignore - but log it.
                        }
                    }

                    serviceName = baseType.FullName;
                    
                    if (!serviceName.Contains(","))
                        serviceName += ", " + baseType.Assembly.GetName().FullName;
                    
                    baseType = baseType.BaseType;

                    if (baseType.FullName == "System.Object")
                        baseType = null;
                }

                //lets return a default for the type.
                if (service == null)
                {
                    if (typeof(TransactionalAction).IsInstanceOfType(action))
                    {
                        return new TransactionalService(ActionManagement.CreateInstance());
                    }
                    else if (typeof(CompositeAction).IsInstanceOfType(action))
                    {
                        return new CompositeService(ActionManagement.CreateInstance());
                    }
                    else if (typeof(GoToAction).IsInstanceOfType(action))
                    {
                        return new RuntimeService(ActionManagement.CreateInstance());
                    }
                    else if (typeof(ActionBase).IsInstanceOfType(action))
                    {
                        return new RuntimeService(ActionManagement.CreateInstance());
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                Context.WriteToLog(ex.Message); 
            }

            return null;
        }

    }
}
