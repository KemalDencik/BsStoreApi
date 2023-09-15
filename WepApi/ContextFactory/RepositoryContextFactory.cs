using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Repositories.EFCore;

namespace WepApi.ContextFactory
{
    public class RepositoryContextFactory : IDesignTimeDbContextFactory<RepositoryContext>
    {
        public RepositoryContext CreateDbContext(string[] args)
        {
            //configuration appsettings a gider 
            //setbasepath çalışan klasörü hedefliyor
            //AddJsonFile dosyaya izin verdik ve build ettik
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()).
                AddJsonFile("appsettings.json").
                Build();
            //db context ise appsettings den gelen connctionstring ifadesiyle birleştirir 
            var builder = new DbContextOptionsBuilder<RepositoryContext>()
                .UseSqlServer(configuration.GetConnectionString("sqlConnection"),
                prj => prj.MigrationsAssembly("WepApi"));

            return new RepositoryContext(builder.Options);
        }
    }
}
