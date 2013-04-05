using Newtonsoft.Json.Linq;
using ProductCatalog.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductCatalog.WebApi.DataLayer
{
    public class ProductRepository
    {
        private int nextProductId = products.Count;
        private int nextReviewId = reviews.Count;

        internal static List<Product> products = new List<Product>
        {
            new Product { Id = 1, Tags = { "book", "database" }, Name = "NoSQL Distilled", Price = 23.49 },
            new Product { Id = 2, Tags = { "book", "database" }, Name = "Seven Databases in Seven Weeks", Price = 26.99 },
            new Product { Id = 3, Tags = { "book", "web" }, Name = "Professional ASP.NET Web API", Price = 34.99 },
            new Product { Id = 4, Tags = { "book", "azure" }, Name = "Moving Applications to the Cloud", Price = 19.99 },
            new Product { Id = 5, Tags = { "tv", "electronics" }, Name = "Sony LCD Flatscreen", Price = 259.99 }
        };

        internal static List<Review> reviews = new List<Review>
        {
            new Review { Id = 1, ProdId = 1, UserId = "User1", Rating = 4, ReviewDate = DateTime.UtcNow, Comments = "Fantastic!" },
            new Review { Id = 2, ProdId = 1, UserId = "User1", Rating = 2, ReviewDate = DateTime.UtcNow, Comments = "Not so good." },
            new Review { Id = 3, ProdId = 1, UserId = "User2", Rating = 3, ReviewDate = DateTime.UtcNow, Comments = "It was average." },
            new Review { Id = 4, ProdId = 4, UserId = "User2", Rating = 4, ReviewDate = DateTime.UtcNow, Comments = "Good read.  I recommend it." },
            new Review { Id = 5, ProdId = 5, UserId = "User2", Rating = 5, ReviewDate = DateTime.UtcNow, Comments = "Great picture and sound quality." }
        };

        public Product GetProduct(int id)
        {
            return products.Find(p => p.Id == id);
        }

        public int AddProductDetails(JObject json)
        {
            var product = json.ToProduct();
            product.Id = nextProductId++;
            products.Add(product);
            return product.Id;
        }

        public JObject GetProductDetails(int id)
        {
            var product = GetProduct(id);
            if (product != null)
                return product.ToJObject();
            else
                return null;
        }

        public IEnumerable<Review> GetProductReviews(int prodid)
        {
            IEnumerable<Review> queryReviews =
                from r in reviews
                where r.ProdId == prodid
                select r;

            return queryReviews;
        }

        public int AddProductReview(int prodId, Review review)
        {
            review.ProdId = prodId;
            review.Id = nextReviewId++;
            reviews.Add(review);
            return review.Id;
        }

        public Review GetProductReview(int prodid, int reviewid)
        {
            return reviews.Find(r => r.Id == reviewid && r.ProdId == prodid);
        }

        public int UpdateProductReview(Review review)
        {
            var prodReview = GetProductReview(review.ProdId, review.Id);
            if (prodReview == null)
                return AddProductReview(review.ProdId, review);
            else
            {
                reviews.Remove(prodReview);
                reviews.Add(review);
                return review.Id;
            }
        }

        public void DeleteProductsAndReviews()
        {
            products = new List<Product>();
            reviews = new List<Review>();
        }

        public IEnumerable<Product> GetProductListByIds(int[] prodIds)
        {
            return products.Where(
                (p) => prodIds.Contains(p.Id));
        }

        public IEnumerable<Product> GetProductListByName(string srchstr)
        {
            srchstr = srchstr.ToLower();
            return products.Where(
                (p) => p.Name.ToLower().Contains(srchstr));
        }

        public IEnumerable<Product> GetProductListByTag(string srchstr)
        {
            srchstr = srchstr.ToLower();
            return products.Where(
                (p) => p.Tags.Contains(srchstr));
        }

        public IEnumerable<string> GetAvailableTags()
        {
            List<string> Tags = new List<string>();

            foreach (Product p in products)
                foreach (string t in p.Tags)
                    Tags.Add(t);

            return Tags.Distinct();
        }
    }
}