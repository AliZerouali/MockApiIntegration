using Microsoft.AspNetCore.Mvc;
using MockApiIntegration.Models;
using MockApiIntegration.Services;

namespace MockApiIntegration.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts([FromQuery] string? name, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        try
        {
            var products = await _productService.GetProductsAsync(name, page, pageSize);
            return Ok(products);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error retrieving products: {ex.Message}");
        }
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdProduct = await _productService.CreateProductAsync(product);
            return CreatedAtAction(nameof(GetProducts), new { id = createdProduct.Id }, createdProduct);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error creating product: {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(string id)
    {
        try
        {
            await _productService.DeleteProductAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error deleting product: {ex.Message}");
        }
    }
}