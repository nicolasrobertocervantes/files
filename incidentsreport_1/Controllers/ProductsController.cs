using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DBProgramming_III.Models;

namespace DBProgramming_III.Controllers
{
    public class ProductsController : Controller
    {
        // GET: Products
        public ActionResult Index(string searchTerm, int top = 10, int page = 1)
        {
            int skip = ((page - 1) * top);

            var context = new TechSupportEntities();

            List<Product> products = context.Products.OrderByDescending(p => p.Name).Skip(skip).Take(top).ToList();

            ViewBag.searchTerm = searchTerm;
            int totalItems = context.Products.Count();
            ViewBag.TotalItems = totalItems;
            ViewBag.page = page;
            ViewBag.top = top;

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                products = products.Where(p => p.ProductCode.IndexOf(searchTerm) != -1 ||
                p.Name.IndexOf(searchTerm) != -1).ToList();

                ViewBag.searchCount = products.Count();
            }

            return View(products);
        }

        public ActionResult AddProduct(string id, int message = 0)
        {
            var context = new TechSupportEntities();
            Product product = context.Products.FirstOrDefault(x => x.ProductCode == id);

            if (product == null)
            {
                product = new Product();
            }

            if (message != 0)
            {
                ViewBag.SavedMessage = "Product successfully saved.";
            }

            return View(product);
        }

        [HttpPost]
        public ActionResult AddProduct(Product product)
        {
            var context = new TechSupportEntities();
            var pdCode = "";
            int message = 0;

            pdCode = product.ProductCode.Trim();
            product.ProductCode = pdCode;

            try
            {

                if (Request.Form["Version"] != null)
                {
                    product.Version = Convert.ToDecimal(Request.Form["Version"]);
                }
                else
                {
                    product.Version = 0;
                }

                context.Products.AddOrUpdate(product);
                context.SaveChanges();

                message = 1;
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }

            return Redirect("/Products/AddProduct/" + pdCode + "/" + message);
        }
    }
}