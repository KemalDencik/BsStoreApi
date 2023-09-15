using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Mvc;
using NLog;
using Services;
using Services.Contracts;
using WebApi.Extensions;
using WepApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

//NLog konfigürasyonu ekleme
LogManager.LoadConfiguration(String.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));

// Add services to the container.
//patch iþlemi için ikinci durumu ekledim
builder.Services.AddControllers(config =>
{
    //içerik pazarlýðýna sebebiyet veriyor içerik pazarlýðý:farklý formatta çýktýlar
    config.RespectBrowserAcceptHeader = true;
    //isteði kabul edip etmediðimizi bildirecez
    //istemediðimiz formatta bir taleple karþýlaþýyorsak bu ifadeyi truya çekerek istemciyle paylaþtýk formatý kabul etmediðini görüntüledik 
    config.ReturnHttpNotAcceptable = true;
    config.CacheProfiles.Add("5mins",new CacheProfile() { Duration=300});
})
//xml formatýnda çýktý verecek 
.AddXmlDataContractSerializerFormatters()
//CSV FORMATI 
.AddCustomCsvFormatter()
.AddApplicationPart(typeof(Presentation.AssemblyReference).Assembly);
//.AddNewtonsoftJson();


builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

//CONTROLLER YAPILARI ALTTAAKÝ KOD ÝLE KEÞFEDÝLÝYOR
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//serviceExtensions dan sonra eklendi servisi kalýtýmla alamayacaðýmýz için static sýnýf açtýk ve buraya sýnýfýmýzý verdik
//kayýtlar
builder.Services.ConfigureSqlContext(builder.Configuration);
builder.Services.ConfigureRepositoryManager();
builder.Services.ConfigureServiceManager();
builder.Services.ConfigureLoggerService();
//referans adýmýzý yani reflection ý parantez içinde yansýtmýþ olduk  
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.ConfigureActionFilters();
builder.Services.ConfigureCors();
builder.Services.ConfigureDataShaper();
builder.Services.AddCustomMediaType();
builder.Services.AddScoped<IBookLinks,BookLinks>();
builder.Services.ConfigureVersioning();
builder.Services.ConfigureResponseCaching();
builder.Services.ConfigureHttpCacheHeaders();
//hýz sýnýrlama
builder.Services.AddMemoryCache();
builder.Services.ConfigureRateLimitingOptions();
builder.Services.AddHttpContextAccessor();
builder.Services.ConfigureIdentity();
builder.Services.ConfigureJwt(builder.Configuration); 


var app = builder.Build();

var logger = app.Services.GetService<ILoggerService>();
app.ConfigureExceptionHandler(logger);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (app.Environment.IsProduction())
{
    app.UseHsts();
}

app.UseHttpsRedirection();
//her hangi bir istemci herhangi bir headersla vs istek atabilir

//rate limit

app.UseIpRateLimiting();
app.UseCors("CorsPolicy");
//caching
app.UseResponseCaching();
app.UseHttpCacheHeaders();

app.UseAuthentication();//kullanýcý adý þifreyle önce iþlem gerçekleþecek sonra alttaki kýsýmla yetkilendirme olacak 
app.UseAuthorization();//yetkilendirme

app.MapControllers();

app.Run();
