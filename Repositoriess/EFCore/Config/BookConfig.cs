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
        //EntityTypeBuilder bize bir tip inşası yapacak
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.HasData(
                new Book { Id = 1, Title = "kemal", Price = 150 },

                new Book { Id = 2, Title = "Özlem", Price = 200 },

                new Book { Id = 3, Title = "Aile", Price = 1500 }
                );
        }
    }
}
