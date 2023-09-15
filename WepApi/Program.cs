using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Mvc;
using NLog;
using Services;
using Services.Contracts;
using WebApi.Extensions;
using WepApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

//NLog konfig�rasyonu ekleme
LogManager.LoadConfiguration(String.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));

// Add services to the container.
//patch i�lemi i�in ikinci durumu ekledim
builder.Services.AddControllers(config =>
{
    //i�erik pazarl���na sebebiyet veriyor i�erik pazarl���:farkl� formatta ��kt�lar
    config.RespectBrowserAcceptHeader = true;
    //iste�i kabul edip etmedi�imizi bildirecez
    //istemedi�imiz formatta bir taleple kar��la��yorsak bu ifadeyi truya �ekerek istemciyle payla�t�k format� kabul etmedi�ini g�r�nt�ledik 
    config.ReturnHttpNotAcceptable = true;
    config.CacheProfiles.Add("5mins",new CacheProfile() { Duration=300});
})
//xml format�nda ��kt� verecek 
.AddXmlDataContractSerializerFormatters()
//CSV FORMATI 
.AddCustomCsvFormatter()
.AddApplicationPart(typeof(Presentation.AssemblyReference).Assembly);
//.AddNewtonsoftJson();


builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

//CONTROLLER YAPILARI ALTTAAK� KOD �LE KE�FED�L�YOR
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//serviceExtensions dan sonra eklendi servisi kal�t�mla alamayaca��m�z i�in static s�n�f a�t�k ve buraya s�n�f�m�z� verdik
//kay�tlar
builder.Services.ConfigureSqlContext(builder.Configuration);
builder.Services.ConfigureRepositoryManager();
builder.Services.ConfigureServiceManager();
builder.Services.ConfigureLoggerService();
//referans ad�m�z� yani reflection � parantez i�inde yans�tm�� olduk  
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.ConfigureActionFilters();
builder.Services.ConfigureCors();
builder.Services.ConfigureDataShaper();
builder.Services.AddCustomMediaType();
builder.Services.AddScoped<IBookLinks,BookLinks>();
builder.Services.ConfigureVersioning();
builder.Services.ConfigureResponseCaching();
builder.Services.ConfigureHttpCacheHeaders();
//h�z s�n�rlama
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

app.UseAuthentication();//kullan�c� ad� �ifreyle �nce i�lem ger�ekle�ecek sonra alttaki k�s�mla yetkilendirme olacak 
app.UseAuthorization();//yetkilendirme

app.MapControllers();

app.Run();
