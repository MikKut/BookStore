using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace BookStore.Infrastructure.Filters
{
    public class LogActionFilterAttribute<T> : IActionFilter where T : ControllerBase
    {
        private readonly ILogger<T> _logger;

        public LogActionFilterAttribute(ILogger<T> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation("Entered {ActionName} action method of {ControllerName}",
                context.ActionDescriptor.DisplayName, typeof(T).Name);
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception != null)
            {
                LogException(context);
                return;
            }

            switch (context.Result)
            {
                case ObjectResult objectResult:
                    LogObjectResult(context, objectResult);
                    break;
                case StatusCodeResult statusCodeResult:
                    LogStatusCodeResult(context, statusCodeResult);
                    break;
                default:
                    LogDefaultResult(context);
                    break;
            }
        }

        private void LogException(ActionExecutedContext context)
        {
            _logger.LogError(context.Exception, "Exception occurred in {ActionName} action method of {ControllerName}",
                context.ActionDescriptor.DisplayName, typeof(T).Name);
        }

        private void LogObjectResult(ActionExecutedContext context, ObjectResult objectResult)
        {
            var statusCode = objectResult.StatusCode;

            if (!statusCode.HasValue)
            {
                _logger.LogWarning("Returned ObjectResult without status code from {ActionName} action method of {ControllerName}",
                    context.ActionDescriptor.DisplayName, typeof(T).Name);
                return;
            }

            switch (statusCode.Value)
            {
                case int n when (n >= 200 && n < 300):
                    _logger.LogInformation("Returned {StatusCode} from {ActionName} action method of {ControllerName}",
                        statusCode, context.ActionDescriptor.DisplayName, typeof(T).Name);
                    break;
                case 400:
                    _logger.LogError("Bad request in {ActionName} of {ControllerName}: {Value}",
                        context.ActionDescriptor.DisplayName, typeof(T).Name, objectResult.Value);
                    break;
                case int n when (n >= 500 && n < 600):
                    _logger.LogError("Server error {StatusCode} in {ActionName} of {ControllerName}: {Value}",
                        statusCode, context.ActionDescriptor.DisplayName, typeof(T).Name, objectResult.Value);
                    break;
                default:
                    _logger.LogWarning("Returned unexpected status code {StatusCode} from {ActionName} action method of {ControllerName}",
                        statusCode, context.ActionDescriptor.DisplayName, typeof(T).Name);
                    break;
            }
        }

        private void LogStatusCodeResult(ActionExecutedContext context, StatusCodeResult statusCodeResult)
        {
            var statusCode = statusCodeResult.StatusCode;

            switch (statusCode)
            {
                case 404:
                    _logger.LogInformation("Not found from {ActionName} action method of {ControllerName}",
                        context.ActionDescriptor.DisplayName, typeof(T).Name);
                    break;
                case int n when (n >= 500 && n < 600):
                    _logger.LogError("Server error {StatusCode} in {ActionName} action method of {ControllerName}",
                        statusCode, context.ActionDescriptor.DisplayName, typeof(T).Name);
                    break;
                default:
                    _logger.LogWarning("Returned status code {StatusCode} from {ActionName} action method of {ControllerName}",
                        statusCode, context.ActionDescriptor.DisplayName, typeof(T).Name);
                    break;
            }
        }

        private void LogDefaultResult(ActionExecutedContext context)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(context.ModelState);
                _logger.LogError("Model state in {ActionName} of {ControllerName} is invalid",
                    context.ActionDescriptor.DisplayName, typeof(T).Name);
            }
            else
            {
                _logger.LogInformation("Returned default response from {ActionName} action method of {ControllerName}",
                    context.ActionDescriptor.DisplayName, typeof(T).Name);
            }
        }
    }
}
