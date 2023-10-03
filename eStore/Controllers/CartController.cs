using BusinessObject;
using DataAccess.DTO.Order;
using DataAccess.DTO.Product;
using eStore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace eStore.Controllers
{
    public class CartController : Controller
    {
        private readonly HttpClient _client;
        private string _productApi;
        private string _orderApi;
        private readonly CartDetails _cartDetails;

        public CartController(CartDetails cartDetails)
        {
            _client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _client.DefaultRequestHeaders.Accept.Add(contentType);
            _productApi = "https://localhost:44371/api/Products";
            _orderApi = "https://localhost:44371/api/Orders";
            _cartDetails = cartDetails;
        }

        public async Task<IActionResult> Product(string searchString = null, string searchPrice = null)
        {
            ViewData["CurrentFilter"] = searchString;
            ViewData["CurrentFilter1"] = searchPrice;

            string token = HttpContext.Session.GetString("authentication");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response;
            if (searchPrice != null || searchString != null)
            {
                response = await _client.GetAsync($"{_productApi}?searchString={searchString}&searchPrice={searchPrice}");
            }
            else
            {
                response = await _client.GetAsync($"{_productApi}");
            }

            string strData = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                List<ProductResponse> productResponses = JsonSerializer.Deserialize<List<ProductResponse>>(strData, options);
                return View(productResponses);
            }
            else
            {
                return View("API request failed.");
            }
        }

        public IActionResult Details()
        {
            return View(_cartDetails);
        }

        public async Task<IActionResult> AddToCart(int id)
        {
			string token = HttpContext.Session.GetString("authentication");
			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

			HttpResponseMessage response = await _client.GetAsync($"{_productApi}/{id}");
            if (response.IsSuccessStatusCode)
            {
                string strData = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };
                ProductResponse product = JsonSerializer.Deserialize<ProductResponse>(strData, options);

                if (product != null)
                {
                    _cartDetails.AddToCart(product);
                }
            }

            return RedirectToAction("Product");
        }

        [HttpPost]
        public IActionResult UpdateQuantity(int productId, int quantity)
        {
            _cartDetails.UpdateQuantity(productId, quantity);
            return RedirectToAction("Details");
        }

        [HttpPost]
        public IActionResult RemoveItem(int productId)
        {
            _cartDetails.RemoveItem(productId);
            return RedirectToAction("Details");
        }

        public async Task<IActionResult> Checkout()
        {
			string token = HttpContext.Session.GetString("authentication");
			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

			// Get the list of items in the cart
			var cartItems = _cartDetails.CartItems;
            int? currentUserId = HttpContext.Session.GetInt32("currentId");

            Random random = new Random();
            var orderDetails = cartItems.Select(cartItem => new OrderDetailRequest
            {
                ProductId = cartItem.Product.ProductId,
                UnitPrice = cartItem.Product.UnitPrice,
                Quantity = cartItem.Quantity,
                Discount = 0,
            }).ToList();

            OrderRequest orderRequest = new OrderRequest
            {
                OrderDate = DateTime.Now,
                RequiredDate = DateTime.Now,
                MemberId = currentUserId,
                Freight = random.Next(10000, 90001),
                OrderDetails = orderDetails,
            };

            // Serialize the DTO to JSON
            var jsonContent = new StringContent(JsonSerializer.Serialize(orderRequest), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PostAsync(_orderApi, jsonContent);

            if (response.IsSuccessStatusCode)
            {
                _cartDetails.ClearCart();
                TempData["Success"] = "Order created successfully.";
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Error creating order.");
                TempData["Error"] = "Order created failed.";
            }

            return RedirectToAction("Details","Cart");
        }


    }
}
