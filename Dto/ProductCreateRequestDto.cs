using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace codingtest;

public class ProductCreateRequestDto
{
    [Required]
    [RegularExpression(@"^[0-9a-zA-Z''-'\s]{1,40}$", ErrorMessage = "special characters are not allowed.")]
    [Description("Name of product")]
    public string? Name { get; set; }
    
    public string? Description { get; set; }

    [Required(ErrorMessage = "Price is required.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
    public decimal? Price { get; set; }

    // public int? page { get; set; }
    // public int? limit { get; set; }
    // public string? sort { get; set; }
    // public string? dir { get; set; }
}
