using DataAccess.DTO.Member;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading.Tasks;

namespace eStore.Controllers
{
    public class LoginController : Controller
    {
        private readonly HttpClient _client;
        private string _memberApi;

        public LoginController()
        {
            _client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _client.DefaultRequestHeaders.Accept.Add(contentType);
            _memberApi = "https://localhost:44371/api/Login";
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("Login")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login([Bind("Email,Password")] LoginRequest loginRequest)
        {
            var content = new StringContent(JsonConvert.SerializeObject(loginRequest), System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponseMessage = await _client.PostAsync($"{_memberApi}/Login", content);
            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                TempData["Error"] = "Email or Password wrong";
                return View("Index");
            }
            MemberResponse memberResponse = await httpResponseMessage.Content.ReadFromJsonAsync<MemberResponse>();

            string token = memberResponse.token;

            HttpContext.Session.SetString("authentication", token);
            HttpContext.Session.SetString("role", memberResponse.Role);
            HttpContext.Session.SetInt32("currentId", memberResponse.MemberId);

            if (memberResponse.Role == "admin")
            {
				return RedirectToAction("Index", "Home");
			}
			else if (memberResponse.Role == "member")
            {
                return RedirectToAction("MemberHomePage", "Home");
			}
			else
            {
				TempData["Error"] = "Email or Password wrong";
                return View("Login", "Index");
			}
        }

        [Route("Logout")]
        [Authorize]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Redirect("/Login/Index");
        }
    }
}
