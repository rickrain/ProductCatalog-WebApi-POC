using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace ProductCatalog.WebApi.Tests
{
    [TestClass]
    public class ProductTest
    {
        // Chage the base address to your local instance.  Port # is likely different.
        private static HttpClient GetHttpClient()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:38096");
            return client;
        }

        [TestMethod]
        public void DeleteProductCatalog()
        {
            //HttpClient client = new HttpClient();
            //client.BaseAddress = baseAddress;
            //HttpResponseMessage response = client.DeleteAsync("/api/product/delete").Result;
            //Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);
        }
        
        private HttpStatusCode _AddProductDetails(JObject json)
        {

            var client = GetHttpClient();
            HttpResponseMessage response = client.PostAsJsonAsync("/api/product", json).Result;
            return response.StatusCode;
        }

        [TestMethod]
        public void AddProductDetails()
        {
            JObject json = JObject.Parse(
@"{
""prodId"" : 101,
""tags"" : [ ""Book"", ""Paperback"" ],
""title"" : ""NoSQL Distilled"",
""desc"" : ""A brief guide to the emerging world of polyglot persistence."",
""price"" : 24.99,
""isbn"" : ""978-0-321-82662-612301-18"",
""pages"" : 164,
""authors"" : [ ""Pramod Sadalage"", ""Martin Fowler"" ],
""imageUrl"": ""/productimages/101.png""
}");

            var responseStatusCode = _AddProductDetails(json);
            Assert.IsTrue(responseStatusCode == HttpStatusCode.Created);
            
            json = JObject.Parse(
@"{
""prodId"" : 91201,
""tags"" : [ ""TV"", ""Electronics""],
""brand"" : ""Sony"",
""model"" : ""Bravia"",
""desc"" : ""LCD Flat Screen TV"",
""price"" : 749.99,
""serialNo"" : ""sb129412305-2381"",
""size"" : ""52 inches"",
""manufDate"" : ""2013-01-24"",
""weight"" : ""47 lbs"",
""shipWeight"" : ""64.56 lbs"",
""imageUrl"": ""/productimages/101.png""
}");

            responseStatusCode = _AddProductDetails(json);
            Assert.IsTrue(responseStatusCode == HttpStatusCode.Created);
        }

        [TestMethod]
        public void GetProduct()
        {
            var client = GetHttpClient();
            HttpResponseMessage response = client.GetAsync("/api/product/1").Result;
            Assert.IsTrue(response.IsSuccessStatusCode);

            // Make sure the data is valid.
            JObject json = response.Content.ReadAsAsync<JObject>().Result;
            Assert.IsNotNull(json);
        }

        [TestMethod]
        public void GetProductDetails()
        {
            var client = GetHttpClient();
            HttpResponseMessage response = client.GetAsync("/api/product/1/details").Result;
            Assert.IsTrue(response.IsSuccessStatusCode);

            // Make sure the data is valid.
            JObject json = response.Content.ReadAsAsync<JObject>().Result;
            Assert.IsNotNull(json);
        }

        [TestMethod]
        public void AddProductReview()
        {
            JObject json = JObject.Parse(
@"{
  ""userId"" : ""user3"",
  ""comments"" : ""Great product!"",
  ""rating"" : 5,
  ""reviewDate"" : ""2013-03-19""
}");

            // Post the review
            var client = GetHttpClient();
            HttpResponseMessage response = client.PostAsJsonAsync("/api/product/1/reviews", json).Result;
            Assert.IsTrue(response.StatusCode == HttpStatusCode.Created);

            // Test the location header value
            response = client.GetAsync(response.Headers.Location).Result;
            Assert.IsTrue(response.IsSuccessStatusCode);

            // Make sure the data is valid.
            json = response.Content.ReadAsAsync<JObject>().Result;
            Assert.IsNotNull(json);
        }

        [TestMethod]
        public void GetProductReview()
        {
            // Get a specified review
            var client = GetHttpClient();
            HttpResponseMessage response = client.GetAsync("/api/product/1/reviews/2").Result;
            Assert.IsTrue(response.IsSuccessStatusCode);

            // Make sure the data is valid.
            var json = response.Content.ReadAsAsync<JObject>().Result;
            Assert.IsNotNull(json);
        }

        [TestMethod]
        public void GetProductReviewForUnknownProdId()
        {
            // Try to get a product review for a product that doesn't exist.
            var client = GetHttpClient();
            HttpResponseMessage response = client.GetAsync("/api/product/28282/reviews/2").Result;
            Assert.IsTrue(response.StatusCode == HttpStatusCode.NotFound);
        }

        [TestMethod]
        public void SearchProductsById()
        {
            // Perform search for multiple product Id's
            var client = GetHttpClient();
            HttpResponseMessage response = client.GetAsync("/api/product/search/prodid?key=1&key=3").Result;
            Assert.IsTrue(response.IsSuccessStatusCode);

            // Make sure the data is valid (an array of JSON objects)
            var json = response.Content.ReadAsAsync<JObject[]>().Result;
            Assert.IsNotNull(json);
        }

        [TestMethod]
        public void SearchProductsByTag()
        {
            // Perform search by tag
            var client = GetHttpClient();
            HttpResponseMessage response = client.GetAsync("/api/product/search/tag?key=book").Result;
            Assert.IsTrue(response.IsSuccessStatusCode);

            // Make sure the data is valid (an array of JSON objects)
            var json = response.Content.ReadAsAsync<JObject[]>().Result;
            Assert.IsNotNull(json);
        }

        [TestMethod]
        public void SearchProductsByName()
        {
            // Perform search by name
            var client = GetHttpClient();
            HttpResponseMessage response = client.GetAsync("/api/product/search/name?key=sql").Result;
            Assert.IsTrue(response.IsSuccessStatusCode);

            // Make sure the data is valid (an array of JSON objects)
            var json = response.Content.ReadAsAsync<JObject[]>().Result;
            Assert.IsNotNull(json);
        }

        [TestMethod]
        public void SearchProductsByNameWithPagination()
        {
            int pageSize = 2;  // Only want at most 2 items returned
            
            // Perform search for products with 'o' in the name
            var client = GetHttpClient();
            string requestUri = string.Format("/api/product/search/name?key=o&pageSize={0}", pageSize);
            HttpResponseMessage response = client.GetAsync(requestUri).Result;
            Assert.IsTrue(response.IsSuccessStatusCode);

            // Make sure the data is valid (an array of JSON objects)
            var json = response.Content.ReadAsAsync<JObject[]>().Result;
            Assert.IsNotNull(json);

            // Make sure only the number of items requested were returned.
            Assert.IsTrue(json.Length <= pageSize);            
        }

        [TestMethod]
        public void GetProductReviews()
        {
            // Get product reviews for a product
            var client = GetHttpClient();
            HttpResponseMessage response = client.GetAsync("/api/product/1/reviews").Result;
            Assert.IsTrue(response.IsSuccessStatusCode);

            // Make sure the data is valid (an array of JSON objects)
            var json = response.Content.ReadAsAsync<JObject[]>().Result;
            Assert.IsNotNull(json);
        }
    }
}
