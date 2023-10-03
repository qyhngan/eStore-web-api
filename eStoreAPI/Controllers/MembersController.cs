using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repository;
using DataAccess.DTO.Member;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace eStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MembersController : ControllerBase
    {
        private readonly IMemberRepository _memberRepository = new MemberRepository();

        // GET: api/Members
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberResponse>>> GetMembers()
        {
            string role = User.FindFirst(ClaimTypes.Role).Value;

            if (role != "admin")
            {
                return Unauthorized();
            }
            List<MemberResponse> memberResponse = null;
            try
            {
                memberResponse = _memberRepository.GetAllMember().ToList();

                if (memberResponse == null)
                {
                    return NotFound("List is empty");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return Ok(memberResponse);
        }

        // GET: api/Members/Profile
        [HttpGet("Profile")]
        public async Task<ActionResult<MemberResponse>> GetCurrentMember()
        {
            string role = User.FindFirst(ClaimTypes.Role).Value;

            if (role != "member")
            {
                return Unauthorized();
            }
            int memberId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var memberResponse = _memberRepository.GetMemberById(memberId);

            if (memberResponse == null)
            {
                return NotFound("Member doesn't exist");
            }

            return Ok(memberResponse);
        }

        //admin
        // GET: api/Members/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MemberResponse>> GetMember(int id)
        {
            string role = User.FindFirst(ClaimTypes.Role).Value;

            if (role != "admin")
            {
                return Unauthorized();
            }
            var memberResponse = _memberRepository.GetMemberById(id);

            if (memberResponse == null)
            {
                return NotFound("Member doesn't exist");
            }

            return Ok(memberResponse);
        }

        // PUT: api/Members/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMember(int id, MemberResponse member)
        {
            string role = User.FindFirst(ClaimTypes.Role).Value;

            if (id != member.MemberId)
            {
                return BadRequest();
            }

            try
            {
                _memberRepository.UpdateMember(member);
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return NoContent();
        }

        // POST: api/Members
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MemberResponse>> PostMember(MemberRequest member)
        {
            string role = User.FindFirst(ClaimTypes.Role).Value;
            if (role != "admin")
            {
                return Unauthorized();
            }

            int memberId = 0;
            try
            {
                memberId = _memberRepository.AddMember(member);
                if (memberId <= 0)
                {
                    return BadRequest("Add failed");
                }
            }
            catch (DbUpdateException)
            {
                throw;
            }

            return CreatedAtAction("GetMember", new { id = memberId }, member);
        }

        // DELETE: api/Members/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMember(int id)
        {
            string role = User.FindFirst(ClaimTypes.Role).Value;
            if (role != "admin")
            {
                return Unauthorized();
            }

            var member = _memberRepository.GetMemberById(id);
            if (member == null)
            {
                return NotFound();
            }

            _memberRepository.DeleteMember(id);

            return Ok("Delete successfully");
        }

    }
}
