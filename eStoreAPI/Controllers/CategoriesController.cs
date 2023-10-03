using Microsoft.AspNetCore.Mvc;
using Repository;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using DataAccess.DTO.Category;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace eStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository = new CategoryRepository();

        // GET: api/Categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryResponse>>> GetCategories()
        {
            string role = User.FindFirst(ClaimTypes.Role).Value;

            if (role != "admin")
            {
                return Unauthorized();
            }
            List<CategoryResponse> categoryResponses = null;
            try
            {
                categoryResponses = _categoryRepository.GetAllCategory().ToList();

                if (categoryResponses == null)
                {
                    return NotFound("List is empty");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return Ok(categoryResponses);
        }
    }
}
