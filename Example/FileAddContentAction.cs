using LucidOcean.RuleEngine;
using System.IO;

namespace Example
{
    public class FileAddContentAction : CompositeAction
    {
        public override void Execute(LucidOcean.RuleEngine.Context.ActionContext context)
        {

            string content = context?.Properties["filecontents"]?.ToString();

            context.Properties["filecontents"] = content + "...";


        }
    }
}
