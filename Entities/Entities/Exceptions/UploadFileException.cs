using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Exceptions
{
    public sealed class UploadFileException : NotFoundException
    {
        public UploadFileException(int id) : base($"File with id : {id} could not found")
        {

        }
    }
}
