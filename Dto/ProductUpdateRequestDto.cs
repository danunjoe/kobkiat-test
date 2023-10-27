using System.ComponentModel.DataAnnotations;

namespace codingtest;

public class ProductUpdateRequestDto
{
    [Required]
    [RegularExpression(@"^[0-9a-zA-Z''-'\s]{1,40}$", ErrorMessage = "special characters are not allowed.")]
    public string? Name { get; set; }
    
    public string? Description { get; set; }

    [Required(ErrorMessage = "Price is required.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
    public decimal? Price { get; set; }
}
