using BusinessObject;
using DataAccess.DTO.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IProductRepository
    {
        IEnumerable<ProductResponse> GetAllProduct(string productName = null, string unitPrice = null);
        ProductResponse GetProductById(int id);
        void AddProduct(ProductRequest product);
        void DeleteProduct(int id);
        Product FindProductById(int id);
        void UpdateProduct(ProductResponse product);
    }
}
