using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using MockApiIntegration.Models;

namespace MockApiIntegration.Services;

public interface IProductService
{
    Task<IEnumerable<Product>> GetProductsAsync(string? name, int page, int pageSize);
    Task<Product> CreateProductAsync(Product product);
    Task DeleteProductAsync(string id);
}

public class ProductService : IProductService
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;
    private readonly ILogger<ProductService> _logger;
    private const string BaseUrl = "https://api.restful-api.dev/objects";

    public ProductService(HttpClient httpClient, ILogger<ProductService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        _httpClient.BaseAddress = new Uri(BaseUrl);
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    public async Task<IEnumerable<Product>> GetProductsAsync(string? name, int page, int pageSize)
    {
        try
        {
            var response = await _httpClient.GetAsync("objects");
            response.EnsureSuccessStatusCode();

            var products = await response.Content.ReadFromJsonAsync<List<Product>>();

            // Apply name filter if provided
            if (!string.IsNullOrEmpty(name))
            {
                products = products?
                    .Where(p => p.Name?.Contains(name, StringComparison.OrdinalIgnoreCase) ?? false)
                    .ToList();
            }

            // Apply paging
            products = products?
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return products ?? new List<Product>();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error fetching products from mock API");
            throw;
        }
    }

    public async Task<Product> CreateProductAsync(Product product)
    {
        try
        {
            var content = new StringContent(
                JsonSerializer.Serialize(product),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PostAsync("objects", content);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<Product>();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error creating product in mock API");
            throw;
        }
    }

    public async Task DeleteProductAsync(string id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"objects/{id}");
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"Error deleting product with ID {id} from mock API");
            }
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, $"Error deleting product with ID {id} from mock API");
            throw;
        }
    }
}