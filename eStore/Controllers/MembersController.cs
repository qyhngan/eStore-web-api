using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessObject;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using DataAccess.DTO.Member;
using Microsoft.AspNetCore.Http;
using System.Text;

namespace eStore.Controllers
{
    public class MembersController : Controller
    {
        private readonly HttpClient _client;
        private string _memberApi;

        public MembersController()
        {
            _client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(contentType);
            _memberApi = "https://localhost:44371/api/Members";

        }

        // GET: Members
        public async Task<IActionResult> Index()
        {
            string token = HttpContext.Session.GetString("authentication");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            
            HttpResponseMessage response = await _client.GetAsync(_memberApi);
            string strData = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                List<MemberResponse> members = JsonSerializer.Deserialize<List<MemberResponse>>(strData, options);
                return View(members);
            }
            else
            {
                return View("API request failed.");
            }
        }

        // GET: Members/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            string token = HttpContext.Session.GetString("authentication");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _client.GetAsync($"{_memberApi}/{id}");
            string strData = await response.Content.ReadAsStringAsync();

            MemberResponse memberResponse;

            if (response.IsSuccessStatusCode)
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                memberResponse = JsonSerializer.Deserialize<MemberResponse>(strData, options);

                if (memberResponse == null)
                {
                    return NotFound();
                }

                return View(memberResponse);
            }
            else
            {
                return View("API request failed.");
            }
        }

        // GET: Members/Create
        public IActionResult Create()
        {
            return View();
        }

        //POST: Members/Create
        //To protect from overposting attacks, enable the specific properties you want to bind to.
        //For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Email,CompanyName,City,Country,Password")] MemberRequest member)
        {
            if (ModelState.IsValid)
            {
                string token = HttpContext.Session.GetString("authentication");
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var content = new StringContent(JsonSerializer.Serialize(member), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _client.PostAsync(_memberApi, content);

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
            return View(member);
        }

        // GET: Members/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            string token = HttpContext.Session.GetString("authentication");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _client.GetAsync($"{_memberApi}/{id}");
            string strData = await response.Content.ReadAsStringAsync();

            MemberResponse memberResponse;

            if (response.IsSuccessStatusCode)
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                memberResponse = JsonSerializer.Deserialize<MemberResponse>(strData, options);

                if (memberResponse == null)
                {
                    return NotFound();
                }

                return View(memberResponse);
            }
            else
            {
                return View("API request failed.");
            }
        }

        // POST: Members/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MemberId,Email,CompanyName,City,Country")] MemberResponse member)
        {
            if (id != member.MemberId)
            {
                return NotFound();
            }

            string token = HttpContext.Session.GetString("authentication");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var url = $"{_memberApi}/{member.MemberId}";

            if (ModelState.IsValid)
            {
                try
                {
                    var content = new StringContent(JsonSerializer.Serialize(member), Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await _client.PutAsync(url, content);

                    if (response.IsSuccessStatusCode)
                    {
                        var options = new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        };
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
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }

        // GET: Members/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            string token = HttpContext.Session.GetString("authentication");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _client.GetAsync($"{_memberApi}/{id}");
            string strData = await response.Content.ReadAsStringAsync();

            MemberResponse memberResponse;

            if (response.IsSuccessStatusCode)
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                memberResponse = JsonSerializer.Deserialize<MemberResponse>(strData, options);

                if (memberResponse == null)
                {
                    return NotFound();
                }

                return View(memberResponse);
            }
            else
            {
                return View("API request failed.");
            }
        }

        // POST: Members/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            string token = HttpContext.Session.GetString("authentication");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _client.DeleteAsync($"{_memberApi}/{id}");

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

    }
}
