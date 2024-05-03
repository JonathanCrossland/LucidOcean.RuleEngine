using LucidOcean.RuleEngine;
using LucidOcean.RuleEngine.Services;

namespace Example
{
    public class CustomRuntimeService : RuntimeService
    {
        public CustomRuntimeService(ActionManagement actionManagement)
            : base(actionManagement)
        {
        }
        public override void Execute(IAction action)
        {

            //do this if you want the default runtime to be invoked
            //base.Execute(action);

            //do this to call the action, or do something else first
            action.Execute(this.ActionManagement.ActionContext);
            //or after
            //consult this code CompositeService or TransactionalService to show other kind of services.

        }

        
    }
}
