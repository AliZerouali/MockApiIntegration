using System.ComponentModel.DataAnnotations;

namespace MockApiIntegration.Models;

public class Product
{
    public string Id { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public decimal Price { get; set; }

    [StringLength(500)]
    public string? Description { get; set; }
}