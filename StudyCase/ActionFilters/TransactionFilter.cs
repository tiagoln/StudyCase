using Core.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;

namespace StudyCase.ActionFilters
{
    public class TransactionFilter : ActionFilterAttribute
    {
        private readonly IUnityOfWork _uow;

        public TransactionFilter(IUnityOfWork uow)
        {
            _uow = uow;
        }
        
        public override void OnResultExecuted(ResultExecutedContext context)
        {
            if (context.Exception == null)
            {
                _uow.Commit();
            }
            else
            {
                _uow.Rollback();
            }
            
            base.OnResultExecuted(context);
        }
    }
}