using Microsoft.AspNetCore.Mvc.Filters;

namespace _002_WebApiAutores.Filters
{
    public class MiFiltroDeAccion : IActionFilter
    {
        private readonly ILogger<MiFiltroDeAccion> logger;

        public MiFiltroDeAccion(ILogger<MiFiltroDeAccion> logger)
        {
            this.logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            logger.LogInformation("Before to execute the action.");
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            logger.LogInformation("After to execute the action.");
        }


    }
}
