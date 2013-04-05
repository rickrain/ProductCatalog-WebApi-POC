using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductCatalog.WebApi.Models
{
    public class Review
    {
        public int Id { get; set; }
        public int ProdId { get; set; }
        public string UserId { get; set; }
        public DateTime ReviewDate { get; set; }
        public int Rating { get; set; }
        public string Comments { get; set; }
    }
}