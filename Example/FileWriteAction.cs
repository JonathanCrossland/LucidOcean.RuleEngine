using LucidOcean.RuleEngine;
using System.IO;

namespace Example
{
    public class FileWriteAction : CompositeAction
    {
        public override void Execute(LucidOcean.RuleEngine.Context.ActionContext context)
        {

            string filename = context?.Properties["filename"].ToString();

            context.Properties["filecontents"] += "|";

            File.WriteAllText(filename, context.Properties["filecontents"].ToString());


        }
    }
}
