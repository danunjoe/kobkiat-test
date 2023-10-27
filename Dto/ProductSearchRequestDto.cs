using System.ComponentModel.DataAnnotations;

namespace codingtest;

public class ProductSearchRequestDto
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal? FromPrice { get; set; }
    public decimal? ToPrice { get; set; }
}
