using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Data.Models;

namespace RS1_2024_25.API.Endpoints
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController(ApplicationDbContext _db) : ControllerBase
    {
        public class CategoryRequest
        {
            public string Name { get; set; }
        }
        public class CategoryResponse
        {
            public int CategoryId { get; set; }
            public string Name { get; set; }
        }

        [HttpGet]

        public ActionResult<CategoryResponse[]> GetCategories()
        {
            var categories = _db.Categories
                .Select(x => new CategoryResponse
                {
                    CategoryId = x.CategoryId,
                    Name = x.Name,
                }).ToArray();

            return categories;
        }

        [HttpGet("{id}")]
        public ActionResult<CategoryResponse> GetCategory(int id)
        {
            var category = _db.Categories
                .Where(x => x.CategoryId == id)
                .Select(x => new CategoryResponse
                {
                    CategoryId = x.CategoryId,
                    Name = x.Name,
                }).First();

            return category;
        }

        [HttpPost]
        public ActionResult<CategoryResponse> PostCategory(CategoryRequest request)
        {
            var category = new Category
            {
                Name = request.Name,
            };

            _db.Categories.Add(category);
            _db.SaveChanges();

            var response = new CategoryResponse
            {
                CategoryId=category.CategoryId,
                Name=category.Name,
            };

            return Ok(response);
        }

        [HttpPut("{id}")]
        public ActionResult<string>PutCategory(int id,CategoryRequest request)
        {
            var categtory = _db.Categories.Find(id) ?? throw new KeyNotFoundException("Category not found");

            categtory.Name = request.Name;

            _db.SaveChanges();

            return Ok("Category updated successfully");
        }

        [HttpDelete("{id}")]
        public ActionResult<string> DeleteCategory(int id)
        {
            var categtory = _db.Categories.Find(id) ?? throw new KeyNotFoundException("Category not found");

            _db.Categories.Remove(categtory);
            _db.SaveChanges();

            return Ok("Category deleted successfully");
        }
    }
}
