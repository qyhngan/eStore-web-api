using BusinessObject;
using DataAccess.DTO.Product;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace eStore.Models
{
    public class CartDetails
    {
        private readonly List<CartItem> _cartItems = new List<CartItem>();
        public IReadOnlyList<CartItem> CartItems => _cartItems.AsReadOnly();

        public void AddToCart(ProductResponse product)
        {
            var existingItem = _cartItems.FirstOrDefault(item => item.Product.ProductId == product.ProductId);

            if (existingItem != null)
            {
                existingItem.Quantity++;
            }
            else
            {
                var newItem = new CartItem
                {
                    Product = product,
                    Quantity = 1
                };
                _cartItems.Add(newItem);
            }
        }

        public void UpdateQuantity(int productId, int quantity)
        {
            var existingItem = _cartItems.FirstOrDefault(item => item.Product.ProductId == productId);

            if (existingItem != null)
            {
                existingItem.Quantity = quantity;
            }
        }

        public void RemoveItem(int productId)
        {
            var item = _cartItems.FirstOrDefault(i => i.Product.ProductId == productId);

            if (item != null)
            {
                _cartItems.Remove(item);
            }
        }

        public void ClearCart()
        {
            _cartItems.Clear();
        }
    }

    public class CartItem
    {
        public ProductResponse Product { get; set; }
        [Range(1, 1000)]
        public int Quantity { get; set; }
    }
}
