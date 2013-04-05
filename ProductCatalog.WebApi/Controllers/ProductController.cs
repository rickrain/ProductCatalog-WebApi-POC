using Newtonsoft.Json.Linq;
using ProductCatalog.WebApi.DataLayer;
using ProductCatalog.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ProductCatalog.WebApi.Controllers
{
    public class ProductController : ApiController
    {
        const int maxPageSize = 30;

        private ProductRepository repository = null;
        public ProductController()
        {
            repository = new ProductRepository();
        }

        // DELETE api/product
        [HttpDelete]
        [ActionName("delete")]
        public HttpResponseMessage DeleteProductsAndReviews()
        {
            repository.DeleteProductsAndReviews();
            var responseMsg = Request.CreateResponse(HttpStatusCode.OK);
            return responseMsg;
        }

        // POST api/product
        [HttpPost]
        [ActionName("default")]
        public HttpResponseMessage AddProductDetails([FromBody]JObject value)
        {
            var prodId = repository.AddProductDetails(value);
            var responseMsg = Request.CreateResponse(HttpStatusCode.Created);
            responseMsg.Headers.Location = new Uri(
                Request.RequestUri + "/" + (prodId).ToString());
            return responseMsg;
        }

        // GET api/product/{prodId}
        [HttpGet]
        [ActionName("default")]
        public HttpResponseMessage GetProduct(int prodId)
        {
            var product = repository.GetProduct(prodId);
            if (product != null)
                return Request.CreateResponse<Product>(HttpStatusCode.OK, product);
            else
                return Request.CreateErrorResponse(
                    HttpStatusCode.NotFound,
                    string.Format("Product Id {0} not found.", prodId));
        }

        // GET api/product/{prodId}/details
        [HttpGet]
        [ActionName("details")]
        public HttpResponseMessage GetProductDetails(int prodId)
        {
            var product = repository.GetProductDetails(prodId);

            if (product != null)
                return Request.CreateResponse<JObject>(
                    HttpStatusCode.OK, product);
            else
                return Request.CreateErrorResponse(
                    HttpStatusCode.NotFound,
                    string.Format("Product Id {0} not found.", prodId));
        }

        // POST api/product/{prodId}/reviews
        [HttpPost]
        [ActionName("reviews")]
        public HttpResponseMessage AddProductReview(int prodId, [FromBody]Review review)
        {
            if (repository.GetProduct(prodId) != null)
            {
                var reviewId = repository.AddProductReview(prodId, review);
                var responseMsg = Request.CreateResponse(HttpStatusCode.Created);
                responseMsg.Headers.Location = new Uri(
                    Request.RequestUri + "/" + (reviewId).ToString());
                return responseMsg;
            }
            else
                return Request.CreateErrorResponse(
                    HttpStatusCode.NotFound,
                    string.Format("Product Id {0} not found.", prodId));
        }

        // GET api/product/{prodId}/reviews
        [HttpGet]
        [ActionName("reviews")]
        public HttpResponseMessage GetProductReviews(int prodId, [FromUri]int pageIndex = 0, [FromUri]int pageSize = 10)
        {
            var reviews = repository.GetProductReviews(prodId).Skip(pageIndex * pageSize).Take(pageSize);

            if (reviews != null)
                return Request.CreateResponse<IEnumerable<Review>>(
                    HttpStatusCode.OK, reviews);
            else
                return Request.CreateErrorResponse(
                    HttpStatusCode.NotFound,
                    string.Format("Product Id {0} not found.", prodId));
        }

        // GET api/product/{prodId}/reviews/{reviewId}
        [HttpGet]
        [ActionName("reviews")]
        public HttpResponseMessage GetProductReview(int prodId, int reviewid)
        {
            var review = repository.GetProductReview(prodId, reviewid);

            if (review != null)
                return Request.CreateResponse<Review>(
                    HttpStatusCode.OK, review);
            else
                return Request.CreateErrorResponse(
                    HttpStatusCode.NotFound,
                    string.Format(
                    "No matching record found for Product Id {0} and Review Id {1}.",
                    prodId, reviewid));
        }

        // PUT api/product/{prodId}/reviews/{reviewId}
        [HttpPut]
        [ActionName("reviews")]
        public HttpResponseMessage UpdateProductReview(int prodId, int reviewid, [FromBody]Review review)
        {
            var reviewId = repository.UpdateProductReview(review);
            var responseMsg = Request.CreateResponse(HttpStatusCode.Created);
            responseMsg.Headers.Location = Request.RequestUri;
            return responseMsg;
        }

        // GET api/product/search/prodid?key=1000&key=1001
        [HttpGet]
        [ActionName("searchprodid")]
        public HttpResponseMessage GetProductListByIds([FromUri] int[] key)
        {
            var products = repository.GetProductListByIds(key);
            return Request.CreateResponse<List<Product>>(HttpStatusCode.OK, products.ToList());
        }

        //GET /api/product/search/tag?key=book&pageIndex=0&pageSize=10
        [HttpGet]
        [ActionName("searchtag")]
        public HttpResponseMessage GetProductListByTag(string key, int pageIndex = 0, int pageSize = 10)
        {
            var products = repository.GetProductListByTag(key).Skip(pageIndex * pageSize).Take(pageSize);
            var response = Request.CreateResponse<List<Product>>(HttpStatusCode.OK, products.ToList());
            return response;
        }

        //GET /api/product/search/name?key=sql&pageIndex=0&pageSize=10
        [HttpGet]
        [ActionName("searchname")]
        public HttpResponseMessage GetProductListByName(string key, int pageIndex = 0, int pageSize = 10)
        {
            if (pageSize <= maxPageSize)
            {
                var products = repository.GetProductListByName(key).Skip(pageIndex * pageSize).Take(pageSize);
                return Request.CreateResponse<IEnumerable<Product>>(HttpStatusCode.OK, products);
            }
            else
                return Request.CreateErrorResponse(
                    HttpStatusCode.BadRequest,
                    string.Format(
                    "pageSize of {0} is too large.  Max pageSize supported is {1}.", pageSize, maxPageSize));
        }

        // GET api/product/tags
        [HttpGet]
        [ActionName("tags")]
        public HttpResponseMessage GetTags()
        {
            var tags = repository.GetAvailableTags();
            return Request.CreateResponse<IEnumerable<string>>(HttpStatusCode.OK, tags);
        }
    }
}
