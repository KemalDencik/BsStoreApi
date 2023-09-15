using Entities.ErrorModel;
using Entities.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Services.Contracts;
using System.Net;

namespace WepApi.Extensions
{
    public static class ExceptionMiddleWareExtensions
    {
        public static void ConfigureExceptionHandler (this WebApplication app, ILoggerService logger) 
        {
            //hata yönetimi
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();//hata olan bi şey varmı diye bakıyorum 
                    if (contextFeature is not null) 
                    {
                        //switch kulanmanın amacı dönüşüm ifadesini belirlemek için yazıldı 
                        context.Response.StatusCode = contextFeature.Error switch
                        {
                            //bunların her biri bir case
                            NotFoundException => StatusCodes.Status404NotFound,
                            _ => StatusCodes.Status500InternalServerError
                        };

                        logger.LogError($"something went wrong : {contextFeature.Error}"); //ilgili hatayı {} bunun içinde aldım
                        await context.Response.WriteAsync(new ErrorDetails() 
                        {
                            StatusCode =context.Response.StatusCode,
                            //mesajı özelleştirdik exceptionla atılan mesajı dikkate aldık
                            Message = contextFeature.Error.Message
                        }.ToString());
                    }
                }); 

                
            });
        }
    }
}
