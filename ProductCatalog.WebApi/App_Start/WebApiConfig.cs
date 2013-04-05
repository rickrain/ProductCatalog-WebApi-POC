using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace ProductCatalog.WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "SearchProdIdApi",
                routeTemplate: "api/{controller}/search/prodid",
                defaults: new { action = "searchprodid" });

            config.Routes.MapHttpRoute(
                name: "SearchProdTagApi",
                routeTemplate: "api/{controller}/search/tag",
                defaults: new { action = "searchtag" });

            config.Routes.MapHttpRoute(
                name: "SearchProdNameApi",
                routeTemplate: "api/{controller}/search/name",
                defaults: new { action = "searchname" });

            config.Routes.MapHttpRoute(
                name: "ProductTagsApi",
                routeTemplate: "api/{controller}/tags",
                defaults: new { action = "tags" });

            config.Routes.MapHttpRoute(
                name: "ProductReviewsApi",
                routeTemplate: "api/{controller}/{prodId}/reviews/{reviewId}",
                defaults: new { action = "reviews", reviewId = RouteParameter.Optional });

            config.Routes.MapHttpRoute(
                name: "ProductDetailsApi",
                routeTemplate: "api/{controller}/{prodId}/details",
                defaults: new { action = "details" });

            config.Routes.MapHttpRoute(
                name: "DeleteApi",
                routeTemplate: "api/{controller}/delete",
                defaults: new { action = "delete" });

            config.Routes.MapHttpRoute(
                name: "ProductApi",
                routeTemplate: "api/{controller}/{prodId}",
                defaults: new { action = "default", prodId = RouteParameter.Optional });
                //constraints: new { prodId = @"\d+" });
        }
    }
}
