using BusinessObject;
using DataAccess;
using DataAccess.DTO.Product;
using DataAccess.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class ProductRepository : IProductRepository
    {
        public void AddProduct(ProductRequest product) => ProductDAO.SaveProduct(product);

        public void DeleteProduct(int id) => ProductDAO.DeleteProduct(id);

        public IEnumerable<ProductResponse> GetAllProduct(string productName = null, string unitPrice = null) => ProductDAO.GetProducts(productName, unitPrice).Select(item => Mapper.MapProductToDTO(item));

        public ProductResponse GetProductById(int id) => Mapper.MapProductToDTO(ProductDAO.FindProductById(id));

        public void UpdateProduct(ProductResponse product) => ProductDAO.UpdateProduct(product);

        Product IProductRepository.FindProductById(int id) => ProductDAO.FindProductById(id);

    }
}
