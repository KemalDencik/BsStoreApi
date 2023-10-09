using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public record UploadedFile
    {
        public int Id { get; init; }
        public string FileName { get; init; }
        public string Url { get; init; }
        public long Size { get; init; }
    }
}
