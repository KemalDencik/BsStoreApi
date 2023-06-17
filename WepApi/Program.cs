using Microsoft.EntityFrameworkCore;
using Repositories.EFCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//patch i�lemi i�in ikinci durumu ekledim
builder.Services.AddControllers().AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//db context ver options ver ba�lant� dizeni g�nder
//GetConnectionString bunu verince ioc ye dbcontext tan�m�n� yapm�� oluyoruz(dbcontexe ihtiya� olunca bunun somut haline ula�mam�za yaricak)
builder.Services.AddDbContext<RepositoryContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("sqlConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
