using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.EFCore.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.EFCore
{
    public class RepositoryContext : DbContext
    {
        //bir constactior inşa ettim base yapısındaki options la db context e gönderilecek
        public RepositoryContext(DbContextOptions options) :
            base(options)
        {

        }
        public DbSet<Book> Books { get; set; }
        public object Book { get; internal set; }

        //Dbcontext metodunu ovirrede ettik yani geçersiz kıldık ve ihtiyacımızdan dolayı aşağıdaki metodu uyguladık
        //override  temel sınıftan miras alınan bir üyenin uygulamasını geçersiz kılarak özelleştirilmesini sağlar
        //model oluşturulurken aşşağıdaki config ele alınacak
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new BookConfig());
        }
    }
}
