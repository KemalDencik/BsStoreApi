using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.ActionFilters
{
    //dtonun boş olup olmadığını kontrol ediyor
    //doğrulama işlemlerini bir attribute yardımıyla yapacak flutter daki widget mantığı 
    public class ValidationFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var controller = context.RouteData.Values["controller"];
            var action = context.RouteData.Values["action"];

            //dto yakalamk için parametre

            var param = context.ActionArguments.SingleOrDefault(p => p.Value.ToString().Contains("Dto")).Value;

            if(param is null) 
            {
                context.Result = new BadRequestObjectResult($"Dto is null. " +
                    $"Controller : {controller} " +
                    $"Action : {action} "
                    ); //400 kodu boş ise
                return;
            }
            if (!context.ModelState.IsValid)
                context.Result = new UnprocessableEntityObjectResult(context.ModelState); //422 kodu nesne geçersiz ise 
        }
    }
}
