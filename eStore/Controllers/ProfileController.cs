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
using System.Text.Json;
using DataAccess.DTO.Member;
using Microsoft.AspNetCore.Http;
using System.Text;

namespace eStore.Controllers
{
    public class ProfileController : Controller
    {
        private readonly HttpClient _client;
        private string _memberApi;

        public ProfileController()
        {
            _client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(contentType);
            _memberApi = "https://localhost:44371/api/Members";
        }

        // GET: Profile
        public async Task<IActionResult> Index()
        {
            string token = HttpContext.Session.GetString("authentication");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var url = $"{_memberApi}/Profile";
            
            HttpResponseMessage response = await _client.GetAsync(url);
            string strData = await response.Content.ReadAsStringAsync();
            
            MemberResponse memberResponses;
            
            if (response.IsSuccessStatusCode)
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                memberResponses = JsonSerializer.Deserialize<MemberResponse>(strData, options);
            }
            else
            {
                // Handle API error response here
                return View("API request failed.");
            }

            return View(memberResponses);
        }

        // POST: Profile/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([Bind("MemberId,Email,CompanyName,City,Country")] MemberResponse member)
        {
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

    }
}
