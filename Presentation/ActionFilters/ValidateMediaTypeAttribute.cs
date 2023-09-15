using Entities.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.ActionFilters
{
    public class ValidateMediaTypeAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            //accept varmı yokmu kontrol et
            var acceptHeaderPresent = context.HttpContext
                .Request
                .Headers
                .ContainsKey("Accept");//accept varmı 

            //Accept yoksa
            if (!acceptHeaderPresent) 
            {
                context.Result = new BadRequestObjectResult($"Accept header is missing!");
                return;
            }

            //varsa  bizim desteklediğimiz formatta  mo onu kontrol edecez
            var mediaType = context.HttpContext
                .Request
                .Headers["Accept"]
                .FirstOrDefault();
            //out sözcüğü hangi parametresi gönderirsen gönder outMedia metod içinde düzenlenir 
            if(!MediaTypeHeaderValue.TryParse(mediaType, out MediaTypeHeaderValue? outMediaType))
            {
                context.Result = new BadRequestObjectResult($"Media type not present" + $"Please Add Accept header with required media type");
                return;
            }

            //destekledinğe yapılacak işlem

            context.HttpContext.Items.Add("AcceptHeaderMediaType", outMediaType);
        }
    }
}
