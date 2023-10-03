using DataAccess;
using DataAccess.DTO.Category;
using DataAccess.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        public IEnumerable<CategoryResponse> GetAllCategory()
        {
            var categories = CategoryDAO.GetCategories();
            if (categories == null)
            {
                return null;
            }
            List<CategoryResponse> categoryResponse = new List<CategoryResponse>();
            foreach (var item in categories)
            {
                categoryResponse.Add(Mapper.MapCategoryToDTO(item));
            }
            return categoryResponse;
        }
    }
}
