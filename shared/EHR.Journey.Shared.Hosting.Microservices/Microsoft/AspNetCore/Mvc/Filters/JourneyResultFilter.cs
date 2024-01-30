using System.Text.Json;

namespace Microsoft.AspNetCore.Mvc.Filters;

public class JourneyResultFilter : IResultFilter, ITransientDependency
{
    public void OnResultExecuting(ResultExecutingContext context)
    {
        // 如果是page 直接return
        if (context.ActionDescriptor.IsPageAction()) return;

        var controllerHasDontWrapResultAttribute =
            context.ActionDescriptor.AsControllerActionDescriptor().ControllerTypeInfo.GetCustomAttributes(typeof(WrapResultAttribute), true).Any();
        var controllerActionHasDontWrapResultAttribute = context.ActionDescriptor.GetMethodInfo().GetCustomAttributes(typeof(WrapResultAttribute), true).Any();
        if (controllerHasDontWrapResultAttribute || controllerActionHasDontWrapResultAttribute)
        {
            context.HttpContext.Response.StatusCode = 200;
            var result = new WrapResult<object>();
            if (context.Result is not EmptyResult)
            {
                result.SetSuccess(((ObjectResult)context.Result).Value);
            }

            var jsonSerializer = context.GetService<IJsonSerializer>();

            context.Result = new ContentResult()
            {
                StatusCode = (int)HttpStatusCode.OK,
                ContentType = "application/json;charset=utf-8",
                Content = jsonSerializer.Serialize(result)
            };
        }
    }

    public void OnResultExecuted(ResultExecutedContext context)
    {
    }
}