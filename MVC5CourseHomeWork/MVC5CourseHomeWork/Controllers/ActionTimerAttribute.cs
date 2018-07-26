using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC5CourseHomeWork.Controllers
{
    public class ActionTimerAttribute : ActionFilterAttribute
    {
        // 執行方法前呼叫
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Debug.WriteLine("執行方法前 Time: " + DateTime.Now.ToLongTimeString());
            base.OnActionExecuting(filterContext);
        }
        // 方法完成後呼叫
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            Debug.WriteLine("執行方法完成後 Time: " + DateTime.Now.ToLongTimeString());
            base.OnActionExecuted(filterContext);
        }

        // 執行結果前呼叫
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            Debug.WriteLine("執行結果前 Time: " + DateTime.Now.ToLongTimeString());
            base.OnResultExecuting(filterContext);
        }

        // 結果執行後呼叫
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            Debug.WriteLine("執行結果後 Time: " + DateTime.Now.ToLongTimeString());
            base.OnResultExecuted(filterContext);
        }
    }
}