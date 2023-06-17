using Microsoft.EntityFrameworkCore;
using Repositories.EFCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//patch iþlemi için ikinci durumu ekledim
builder.Services.AddControllers().AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//db context ver options ver baðlantý dizeni gönder
//GetConnectionString bunu verince ioc ye dbcontext tanýmýný yapmýþ oluyoruz(dbcontexe ihtiyaç olunca bunun somut haline ulaþmamýza yaricak)
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
