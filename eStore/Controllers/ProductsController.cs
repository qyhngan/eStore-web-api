using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessObject;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using DataAccess.DTO.Member;
using System.Text.Json;
using DataAccess.DTO.Product;
using DataAccess.DTO.Category;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace eStore.Controllers
{
    public class ProductsController : Controller
    {
        private readonly HttpClient _client;
        private string _productApi;

        public ProductsController()
        {
            _client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _client.DefaultRequestHeaders.Accept.Add(contentType);
            _productApi = "https://localhost:44371/api/Products";

        }

        // GET: Products
        public async Task<IActionResult> Index(string searchString = null, string searchPrice = null)
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

                List<ProductResponse> members = JsonSerializer.Deserialize<List<ProductResponse>>(strData, options);
                return View(members);
            }
            else
            {
                return View("API request failed.");
            }
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            string token = HttpContext.Session.GetString("authentication");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _client.GetAsync($"{_productApi}/{id}");
            string strData = await response.Content.ReadAsStringAsync();

            ProductResponse productResponse;

            if (response.IsSuccessStatusCode)
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                productResponse = JsonSerializer.Deserialize<ProductResponse>(strData, options);

                if (productResponse == null)
                {
                    return NotFound();
                }

                return View(productResponse);
            }
            else
            {
                return View("API request failed.");
            }
        }

        // GET: Products/Create
        public async Task<IActionResult> Create()
        {
            string token = HttpContext.Session.GetString("authentication");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            string url = "https://localhost:44371/api/Categories";
            HttpResponseMessage response = await _client.GetAsync(url);
            
            if (response.IsSuccessStatusCode)
            {
                string strData = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                List<CategoryResponse> categoryResponses = JsonSerializer.Deserialize<List<CategoryResponse>>(strData, options);

                ViewData["CategoryId"] = new SelectList(categoryResponses, "CategoryId", "CategoryName");
                return View();
            }
            else
            {
                return View("API request failed.");
            }

        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryId,ProductName,Weight,UnitPrice,UnitsInStock")] ProductRequest productRequest)
        {
            if (ModelState.IsValid)
            {
                string token = HttpContext.Session.GetString("authentication");
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var content = new StringContent(JsonSerializer.Serialize(productRequest), Encoding.UTF8, "application/json");
                HttpResponseMessage response1 = await _client.PostAsync(_productApi, content);

                if (response1.IsSuccessStatusCode)
                {

                }
                else
                {
                    return View("API request failed.");
                }
                return RedirectToAction(nameof(Index));
            }

            string url = "https://localhost:44371/api/Categories";
            HttpResponseMessage response = await _client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string strData = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                List<CategoryResponse> categoryResponses = JsonSerializer.Deserialize<List<CategoryResponse>>(strData, options);

                ViewData["CategoryId"] = new SelectList(categoryResponses, "CategoryId", "CategoryName", productRequest.CategoryId);
            }
            else
            {
                return View("API request failed.");
            }
            
            return View(productRequest);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            string token = HttpContext.Session.GetString("authentication");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _client.GetAsync($"{_productApi}/{id}");
            string strData = await response.Content.ReadAsStringAsync();

            ProductResponse productResponse;

            if (response.IsSuccessStatusCode)
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                productResponse = JsonSerializer.Deserialize<ProductResponse>(strData, options);

                if (productResponse == null)
                {
                    return NotFound();
                }

                string url = "https://localhost:44371/api/Categories";
                HttpResponseMessage r = await _client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string sd = await r.Content.ReadAsStringAsync();
                    var o = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    List<CategoryResponse> categoryResponses = JsonSerializer.Deserialize<List<CategoryResponse>>(sd, o);

                    int cId = productResponse.CategoryId;
                    ViewData["CategoryId"] = new SelectList(categoryResponses, "CategoryId", "CategoryName", cId);
                }
                else
                {
                    return View("API request failed.");
                }
                return View(productResponse);
            }
            else
            {
                return View("API request failed.");
            }
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CategoryId,ProductName,Weight,UnitPrice,UnitsInStock")] ProductResponse product)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    product.ProductId = id;
                    string token = HttpContext.Session.GetString("authentication");
                    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    var url = $"{_productApi}/{id}";
                    var content = new StringContent(JsonSerializer.Serialize(product), Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await _client.PutAsync(url, content);

                    if (response.IsSuccessStatusCode)
                    {

                    }
                    else
                    {
                        // Handle API error response here
                        return View("API request failed.");
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }

                string url1 = "https://localhost:44371/api/Categories";
                HttpResponseMessage response1 = await _client.GetAsync(url1);

                if (response1.IsSuccessStatusCode)
                {
                    string strData1 = await response1.Content.ReadAsStringAsync();
                    var options1 = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    List<CategoryResponse> categoryResponses = JsonSerializer.Deserialize<List<CategoryResponse>>(strData1, options1);

                    ViewData["CategoryId"] = new SelectList(categoryResponses, "CategoryId", "CategoryName", product.CategoryId);

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return View("API request failed.");
                }
            }
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            string token = HttpContext.Session.GetString("authentication");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _client.GetAsync($"{_productApi}/{id}");
            string strData = await response.Content.ReadAsStringAsync();

            ProductResponse productResponse;

            if (response.IsSuccessStatusCode)
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                productResponse = JsonSerializer.Deserialize<ProductResponse>(strData, options);

                if (productResponse == null)
                {
                    return NotFound();
                }

                return View(productResponse);
            }
            else
            {
                return View("API request failed.");
            }
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            string token = HttpContext.Session.GetString("authentication");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _client.DeleteAsync($"{_productApi}/{id}");

            if (response.IsSuccessStatusCode)
            {

            }
            else
            {
                // Handle API error response here
                return View("API request failed.");
            }

            return RedirectToAction(nameof(Index));
        }

        //private bool ProductExists(int id)
        //{
        //    return _context.Products.Any(e => e.ProductId == id);
        //}

        [AllowAnonymous]
        public async Task<IActionResult> TestJs()
        {
            ViewBag.Token = HttpContext.Session.GetString("authentication");
            return View();
        }
    }
}
