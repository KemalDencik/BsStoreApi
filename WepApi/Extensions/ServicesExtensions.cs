using AspNetCoreRateLimit;
using Entities.DataTransferObject;
using Entities.Models;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Presentation.ActionFilters;
using Presentation.Controllers;
using Repositories.Contracts;
using Repositories.EFCore;
using Services;
using Services.Contracts;
using System.Text;

namespace WepApi.Extensions
{
    public static class ServicesExtensions
    {
        //hangi tipi genişletmek istiyorsak this ifadesiyle buraya parametresini verecez 
        //db context ver options ver bağlantı dizeni gönder
        //GetConnectionString bunu verince ioc ye dbcontext tanımını yapmış oluyoruz(dbcontexe ihtiyaç olunca bunun somut haline ulaşmamıza yaricak)
        //configuration ifadesine ihtiyaç var diye ekledik 
        public static void ConfigureSqlContext(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddDbContext<RepositoryContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("sqlConnection")));
        }
        public static void ConfigureRepositoryManager(this IServiceCollection services)
        {
           services.AddScoped<IRepositoryManager, RepositoryManager>();
        }
        public static void ConfigureServiceManager(this IServiceCollection services)
        {
            services.AddScoped<IServiceManager, ServiceManager >();
        }
        public static void ConfigureLoggerService(this IServiceCollection services) =>
            services.AddSingleton<ILoggerService, LoggerManager>();
        public static void ConfigureActionFilters(this IServiceCollection services)
        {
            //her kullanıcı için farklı bir nesne üreteccek şekilde addscoped kullandık
            services.AddScoped<ValidationFilterAttribute>(); //ıoc kaydını uyguladık 
            services.AddSingleton<LogFilterAttribute>();
            services.AddScoped<ValidateMediaTypeAttribute>();
        }
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",builder =>
                //bütün köknelere izin ver
                builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .WithExposedHeaders("X-Pagination"));
            });
        }
        public static void ConfigureDataShaper(this IServiceCollection services)
        {
            services.AddScoped<IDataShaper<BookDto>, DataShaper<BookDto>>();
        }
        public static void AddCustomMediaType(this IServiceCollection services)
        {
            services.Configure<MvcOptions>(config
                =>
            {
                var systemTypeJsonOutputFormatter = config
                .OutputFormatters
                .OfType<SystemTextJsonOutputFormatter>()?.FirstOrDefault();//link sorgusu bir filtreleme yapmak amaçlı

                if (systemTypeJsonOutputFormatter is not null)
                {
                    systemTypeJsonOutputFormatter.SupportedMediaTypes
                    .Add("application/vnd.WepApi.hateoas+json");

                    systemTypeJsonOutputFormatter.SupportedMediaTypes
                    .Add("application/vnd.WepApi.apiroot+json");
                }

                var xmlOutputFormatter = config
                .OutputFormatters
                .OfType<XmlDataContractSerializerOutputFormatter>()?.FirstOrDefault();

                if (xmlOutputFormatter is not null)
                {
                    xmlOutputFormatter.SupportedMediaTypes
                    .Add("application/vnd.WepApi.hateoas+xml");

                    xmlOutputFormatter.SupportedMediaTypes
                    .Add("application/vnd.WepApi.apiroot+xml");
                }
            });
        }     
        public static void ConfigureVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(opt =>
            {
                opt.ReportApiVersions = true; //apinin version bilgisini response header kısmına ekliyoruz istemci burdan versiyon bilgisini kontrol ediyor
                opt.AssumeDefaultVersionWhenUnspecified = true; //kullanıcı herhangi bir versiyon talep etmezse apini default bir versiyon bilgisi var bununla dönüş yapacak
                opt.DefaultApiVersion = new ApiVersion(1,0); //major version default version budur
                opt.ApiVersionReader = new HeaderApiVersionReader(("api-version"));

                opt.Conventions.Controller<BooksController>()
                .HasApiVersion(new ApiVersion(1, 0));

                opt.Conventions.Controller<BooksV2Controller>()
                .HasDeprecatedApiVersion(new ApiVersion(2, 0));
            });
        }
        //ön belleğe alma
        public static void ConfigureResponseCaching(this IServiceCollection services) =>
            services.AddResponseCaching();  
        //ön belleğe alma
        public static void ConfigureHttpCacheHeaders(this IServiceCollection services) =>
            services.AddHttpCacheHeaders(expirationOpt =>
            {
                expirationOpt.MaxAge = 90;
                expirationOpt.CacheLocation = CacheLocation.Public;
            },
                validationOpt =>
                {
                    validationOpt.MustRevalidate = false; //yeniden validate etme 
                });
        //hız sınırlama
        public static void ConfigureRateLimitingOptions(this IServiceCollection services)
        {
            //limitleri tutan bir liste tanımı
            var rateLimitsRules = new List<RateLimitRule>()
            {
                new RateLimitRule()
                {
                    Endpoint = "*",
                    Limit = 60,
                    Period= "1m"
                }
            };

            services.Configure<IpRateLimitOptions>(opt =>
            {
                opt.GeneralRules = rateLimitsRules; //genel kurallar olarak configure ettik
            }
            );
            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
        }
        public static void ConfigureIdentity(this IServiceCollection services)
        {
            var builder = services.AddIdentity<User, IdentityRole>(opts =>
            {
                opts.Password.RequireDigit = true;
                opts.Password.RequireLowercase = true;
                opts.Password.RequireUppercase = true;
                opts.Password.RequireNonAlphanumeric = true;
                opts.Password.RequiredLength = 8;

                opts.User.RequireUniqueEmail = true;
                opts.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+çÇğĞıİöÖşŞüÜ"; // İzin verilen karakterler
                
            })
                .AddEntityFrameworkStores<RepositoryContext>()
                .AddDefaultTokenProviders();//jwt token için 
        }
        public static void ConfigureJwt(this IServiceCollection services,IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("JwtSettings"); //appsettings e erişim
            var secretKey = jwtSettings["secretKey"]; //okuma işlemi

            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    //doğrulama parametrelerimiz
                    ValidateIssuer = true,//keyi kullananı doğrula
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["validIssuer"],
                    ValidAudience = jwtSettings["validAudience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                }
            ); //midleware ifadesi

        }
    }
}
