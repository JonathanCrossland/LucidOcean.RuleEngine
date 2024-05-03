/*=====================================================================
Authors: Lucid Ocean PTY (LTD)
Copyright © 2014 Lucid Ocean PTY (LTD). All Rights Reserved.

License: Lucid Ocean Wave Business License v1.0
Please refer to http://www.lucidocean.co.za/wbl-license.html for restrictions and freedoms.
The license is on the root of the main source-code directory.
=====================================================================*/


using LucidOcean.RuleEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example
{
    public class AbortingAction :  ActionBase
    {
        public AbortingAction()
        {
        }

        public override void Execute(LucidOcean.RuleEngine.Context.ActionContext context)
        {
            
            context.Runtime.Abort();
            
            base.Execute(context);
        }
    }
}
