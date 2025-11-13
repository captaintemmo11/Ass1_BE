using Microsoft.AspNetCore.Mvc;
using Ass1_BE.Models;
using Ass1_BE.Services;

namespace Ass1_BE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly FirestoreProductService _service;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(FirestoreProductService service, ILogger<ProductsController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// Lấy danh sách tất cả sản phẩm.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var products = await _service.GetAllAsync();
                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching all products");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Lấy thông tin sản phẩm theo ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            try
            {
                var item = await _service.GetByIdAsync(id);
                if (item == null) return NotFound(new { message = $"Product with id {id} not found" });
                return Ok(item);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while fetching product {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Tạo mới một sản phẩm.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Product p)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var created = await _service.CreateAsync(p);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating product");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Cập nhật thông tin sản phẩm theo ID.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] Product p)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var ok = await _service.UpdateAsync(id, p);
                if (!ok) return NotFound(new { message = $"Product with id {id} not found" });

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while updating product {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Xóa sản phẩm theo ID.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var ok = await _service.DeleteAsync(id);
                if (!ok) return NotFound(new { message = $"Product with id {id} not found" });

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while deleting product {id}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
