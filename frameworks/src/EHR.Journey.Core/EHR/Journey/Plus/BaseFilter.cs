using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Users;

namespace EHR.Journey.Core
{
    public class BaseFilter : IActionFilter, ITransientDependency
    {
        private readonly ICurrentUser _currentUser;
        public BaseFilter(ICurrentUser currentUser)
        {
            _currentUser = currentUser;
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        //此处可以注入权限表，实现用权限表来管理整个系统的权限
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var routedata = context.RouteData;
            var zzzz = JsonConvert.SerializeObject(context.ActionArguments);
            var areaName = routedata.Values["area"];
            var controllerName = routedata.Values["controller"].ToString();
            var actionName = routedata.Values["action"].ToString();
            var x = _currentUser.UserName;
            try
            {
                //此处部署一个TenantService的网站，通过这个网站的结果对请求进行劫持，最终达到根据TenantId实现客制化逻辑的定制
                //context.Result = new ActionResult($"~/{controllerName}/{actionName}",true);
             //   context.Result = new RedirectToRouteResult(
             // new RouteValueDictionary
             // {
             //   {"controller", controllerName},
             //   {"action", actionName+"1"},
             //     {"area", areaName},
             // }



             //);
            
            }
            catch (Exception)
            {
            }


            //var x = 1;
            //var authoried = false;
            //if (authoried == false)
            //{
            //    context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
            //    return;
            //}
        }
    }
}
