using DataAccess.DTO.Member;
using eStoreAPI.JWT;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository;

namespace eStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IMemberRepository _memberRepository = new MemberRepository();
        private readonly IJwtAuth _jwtAuth;

        public LoginController(IJwtAuth jwtAuth)
        {
            this._jwtAuth = jwtAuth;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("Login")]
        public IActionResult Login([FromBody] LoginRequest loginRequest)
        {
            MemberResponse memberResponse = null;
            try
            {
                memberResponse = _memberRepository.Login(loginRequest.Email, loginRequest.Password);

                if (memberResponse == null)
                {
                    memberResponse = _memberRepository.LoginAdmin(loginRequest.Email, loginRequest.Password);
                    if (memberResponse == null)
                    {
                        return BadRequest("Email or Password wrong");
                    }

                    var token = _jwtAuth.AuthenticateToken(memberResponse.Email, memberResponse.Role, memberResponse.MemberId.ToString());
                    if (token == null)
                    {
                        return Unauthorized();
                    }
                    memberResponse.token = token;
                    return Ok(memberResponse);
                }
                else
                {
                    var token = _jwtAuth.AuthenticateToken(memberResponse.Email, memberResponse.Role, memberResponse.MemberId.ToString());
                    if (token == null)
                    {
                        return Unauthorized();
                    }
                    memberResponse.token = token;
                    return Ok(memberResponse);
                }
            }
            catch (System.Exception ex)
            {
                throw new System.Exception(ex.Message);
            }
        }
    }
}
