using BusinessObject;
using DataAccess.DTO.Product;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class ProductDAO
    {
        public static List<Product> GetProducts(string productName = null, string unitPrice = null)
        {
            var listProducts = new List<Product>();
            try
            {
                using (var dbContext = new FStoreDBContext())
                {
                    listProducts = dbContext.Products.Include(p => p.Category).ToList();
                }
                if (productName != null)
                {
                    listProducts = listProducts.Where(x => x.ProductName.Contains(productName)).ToList();
                }
                if (unitPrice != null)
                {
                    listProducts = listProducts.Where(x => x.UnitPrice == int.Parse(unitPrice)).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return listProducts;
        }

        public static Product FindProductById(int proId)
        {
            Product product = new Product();
            try
            {
                using (var dbContext = new FStoreDBContext())
                {
                    product = dbContext.Products.Include(p => p.Category).SingleOrDefault(x => x.ProductId == proId);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return product;
        }

        public static void SaveProduct(ProductRequest product)
        {
            try
            {
                using (var dbContext = new FStoreDBContext())
                {
                    int id = 0;

                    if (dbContext.Products.Count() > 0)
                    {
                        id = dbContext.Products.Max(x => x.ProductId);
                    }

                    Product product1 = new Product()
                    {
                        ProductName = product.ProductName,
                        UnitPrice = product.UnitPrice,
                        CategoryId = product.CategoryId,
                        UnitsInStock = product.UnitsInStock,
                        Weight = product.Weight,
                        ProductId = id + 1
                    };
                    dbContext.Products.Add(product1);
                    dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static Product GetProductById(int proId)
        {
            Product product = new Product();
            try
            {
                using (var dbContext = new FStoreDBContext())
                {
                    product = dbContext.Products.SingleOrDefault(x => x.ProductId == proId);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return product;
        }

        public static void UpdateProduct(ProductResponse product)
        {
            try
            {
                using (var dbContext = new FStoreDBContext())
                {
                    Product product1 = GetProductById(product.ProductId);
                    if (product1 == null) throw new Exception("Product not found");
                    product1.ProductName = product.ProductName;
                    product1.UnitPrice = product.UnitPrice;
                    product1.CategoryId = product.CategoryId;
                    product1.UnitsInStock = product.UnitsInStock;
                    product1.Weight = product.Weight;
                    dbContext.Entry<Product>(product1).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void DeleteProduct(int proId)
        {
            try
            {
                using (var dbContext = new FStoreDBContext())
                {
                    var product = dbContext.Products.SingleOrDefault(x => x.ProductId == proId);
                    if (product == null) throw new Exception("Product not found");
                    dbContext.Products.Remove(product);
                    dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
