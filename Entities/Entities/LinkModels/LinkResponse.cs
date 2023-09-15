using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.LinkModels
{
    public class LinkResponse
    {
        public bool HasLinks { get; set; } //link varmı yok mu
        public List<Entity> ShapedEntities { get; set; } //şekillendirilmiş ifadeleri tutacam 
        public LinkCollectionWrapper<Entity> LinkedEntities { get; set; }

        public LinkResponse()
        {
            ShapedEntities = new List<Entity>();
            LinkedEntities = new LinkCollectionWrapper<Entity>();
        }

    }
}
