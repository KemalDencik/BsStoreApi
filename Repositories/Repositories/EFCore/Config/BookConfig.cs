using Entities.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.EFCore.Config
{
    public class BookConfig : IEntityTypeConfiguration<Book>
    {
        //Hard Code ile veri konfigürasyonu - ilk veriler
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.HasData
                (
                    new Book { Id = 1, Title = "Suç ve Ceza", Price = 25 },
                    new Book { Id = 2, Title = "II. Dünya Savaşı", Price = 35 },
                    new Book { Id = 3, Title = "Sarıkamış", Price = 45 }
                );
        }
    }
}
