using Newtonsoft.Json.Linq;
using ProductCatalog.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductCatalog.WebApi.DataLayer
{
    // This class is used to resolve impedience mismatch between the model (Product)
    // and raw json (JObject).
    public static class DataHelper
    {
        private static bool TryGetNameFromList(this JObject json, string[] nameProperties, out JToken token)
        {
            token = null;
            foreach (string nameProperty in nameProperties)
                if (json.TryGetValue(nameProperty, out token))
                    break;
            return (token != null);
        }

        // Convert from raw JSON with no schema to a product in the data model.
        public static Product ToProduct(this JObject json)
        {
            var product = new Product();

            JToken tokenValue;

            // There could be multiple possibilities for name in raw JSON
            string[] nameProperties = { "name", "prodname", "title", "desc"};
            if (json.TryGetNameFromList(nameProperties, out tokenValue))
                product.Name = (string)tokenValue;

            if (json.TryGetValue("price", out tokenValue))
                product.Price = (double)tokenValue;

            if (json.TryGetValue("tags", out tokenValue))
                foreach (string tag in tokenValue)
                    product.Tags.Add(tag);
            
            return product;
        }

        // Convert from a product to raw JSON...
        public static JObject ToJObject(this Product product)
        {
            var jobject = JObject.FromObject(product);

            // Add something just to illustrate we're not returning the model (Product)
            jobject.Add("TimeStamp", DateTime.UtcNow);
            return jobject;
        }
    }
}