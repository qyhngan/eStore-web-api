using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessObject;
using System.Net.Http.Headers;
using System.Net.Http;
using DataAccess.DTO.Product;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using DataAccess.DTO.Order;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.IdentityModel.Tokens.Jwt;

namespace eStore.Controllers
{
    public class OrdersController : Controller
    {
        private readonly HttpClient _client;
        private string _orderApi;

        public OrdersController()
        {
            _client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _client.DefaultRequestHeaders.Accept.Add(contentType);
            _orderApi = "https://localhost:44371/api/Orders";

        }

        // GET: Orders
        public async Task<IActionResult> Index(DateTime from, DateTime to)
        {
            ViewBag.fromDate = from;
            ViewBag.toDate = to;

            string token = HttpContext.Session.GetString("authentication");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response;
            if (from != DateTime.MinValue || to != DateTime.MinValue)
            {
                response = await _client.GetAsync($"{_orderApi}?from={from}&to={to}");
            }
            else
            {
                response = await _client.GetAsync($"{_orderApi}");
            }

            string strData = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                List<OrderResponse> members = JsonSerializer.Deserialize<List<OrderResponse>>(strData, options);
                return View(members);
            }
            else
            {
                return View("API request failed.");
            }
        }

        // GET: Orders
        public async Task<IActionResult> History(DateTime from, DateTime to)
        {
            ViewBag.fromDate = from;
            ViewBag.toDate = to;

            string token = HttpContext.Session.GetString("authentication");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response;
            if (from != DateTime.MinValue || to != DateTime.MinValue)
            {
                response = await _client.GetAsync($"{_orderApi}/History?from={from}&to={to}");
            }
            else
            {
                response = await _client.GetAsync($"{_orderApi}/History");
            }

            string strData = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                List<OrderResponse> members = JsonSerializer.Deserialize<List<OrderResponse>>(strData, options);
                return View(members);
            }
            else
            {
                return View("API request failed.");
            }
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            string token = HttpContext.Session.GetString("authentication");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            string role = HttpContext.Session.GetString("role");
            ViewData["Role"] = role;

            HttpResponseMessage response = await _client.GetAsync($"{_orderApi}/{id}");

            string strData = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                List<OrderDetailResponse> members = JsonSerializer.Deserialize<List<OrderDetailResponse>>(strData, options);
                return View(members);
            }
            else
            {
                return View("API request failed.");
            }
        }

        // GET: Orders/Create
        //public IActionResult Create()
        //{
        //    ViewData["MemberId"] = new SelectList(_context.Members, "MemberId", "City");
        //    return View();
        //}

        //// POST: Orders/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("OrderId,MemberId,OrderDate,RequiredDate,ShippedDate,Freight")] Order order)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(order);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["MemberId"] = new SelectList(_context.Members, "MemberId", "City", order.MemberId);
        //    return View(order);
        //}

        //// GET: Orders/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var order = await _context.Orders.FindAsync(id);
        //    if (order == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["MemberId"] = new SelectList(_context.Members, "MemberId", "City", order.MemberId);
        //    return View(order);
        //}

        //// POST: Orders/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("OrderId,MemberId,OrderDate,RequiredDate,ShippedDate,Freight")] Order order)
        //{
        //    if (id != order.OrderId)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(order);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!OrderExists(order.OrderId))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["MemberId"] = new SelectList(_context.Members, "MemberId", "City", order.MemberId);
        //    return View(order);
        //}

        //// GET: Orders/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var order = await _context.Orders
        //        .Include(o => o.Member)
        //        .FirstOrDefaultAsync(m => m.OrderId == id);
        //    if (order == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(order);
        //}

        //// POST: Orders/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var order = await _context.Orders.FindAsync(id);
        //    _context.Orders.Remove(order);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool OrderExists(int id)
        //{
        //    return _context.Orders.Any(e => e.OrderId == id);
        //}
    }
}
