using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductCatalog.WebApi.Models
{
    public class Product
    {
        public Product()
        {
            Tags = new List<string>();
        }
        public int Id { get; set; }
        public List<string> Tags { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
    }
}