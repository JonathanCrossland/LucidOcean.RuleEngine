/*=====================================================================
Authors: Jonathan Crossland
Copyright © Jonathan Crossland. All Rights Reserved.

The license is on the root of the main source-code directory.
=====================================================================*/
using System.Collections.Generic;

namespace LucidOcean.RuleEngine
{
    public abstract class CompositeAction : ActionBase
    {

        private List<ActionBase> _Items;

        public List<ActionBase> Items
        {
            get
            {
                if (_Items == null)
                    _Items = new List<ActionBase>();

                return _Items;
            }
        }
    }
}