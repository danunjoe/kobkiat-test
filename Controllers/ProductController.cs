using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace codingtest.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{
    private readonly ILogger<ProductController> _logger;
    private readonly DBContext _dbcontext;

    public ProductController(ILogger<ProductController> logger, DBContext dbcontext)
    {
        _logger = logger;
        _dbcontext = dbcontext;
    }

    [HttpPost("Create")]
    public async Task<ActionResult> CreateProduct([FromBody] ProductCreateRequestDto item)
    {
        try
        {
            _logger.LogInformation("Start create product : " + JsonSerializer.Serialize(item));

            ProductModel productModel = new ProductModel();
            productModel.Name = item.Name;
            productModel.Description = item.Description;
            productModel.Price = item.Price;
            productModel.CreatedAt = DateTime.Now;
            productModel.UpdatedAt = DateTime.Now;

            _dbcontext.Products.Add(productModel);
            await _dbcontext.SaveChangesAsync();

            _logger.LogInformation("Create product completed");

            return StatusCode(200, new
            {
                product = productModel
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet("Retrieve")]
    public async Task<ActionResult> RetrieveAllProduct([FromQuery] ProductSearchRequestDto search)
    {
        List<ProductResponseDto> productList = new List<ProductResponseDto>();
        long total = 0;

        var query = _dbcontext.Products.Where(x => x.DeletedAt == null);

        #region filter
        if (search.Name != null)
        {
            query = query.Where(x => x.Name != null && (x.Name.Contains(search.Name) || search.Name.Contains(x.Name)));
        }

        if (search.FromPrice != null)
        {
            query = query.Where(x => x.Price >= search.FromPrice);
        }

        if (search.ToPrice != null)
        {
            query = query.Where(x => x.Price <= search.ToPrice);
        }
        #endregion

        var resProducts = await query.ToListAsync();
        if (resProducts != null && resProducts.Count > 0)
        {
            total = resProducts.Count;

            // filter data and hide some fields
            foreach (var item in resProducts)
            {
                productList.Add(new ProductResponseDto
                {
                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description,
                    Price = item.Price,
                    CreatedAt = item.CreatedAt,
                    UpdatedAt = item.UpdatedAt,
                });
            }
        }

        return StatusCode(200, new
        {
            products = productList,
            total = total
        });
    }

    [HttpGet("Retrieve/{productId}")]
    public async Task<ActionResult> RetrieveAllProduct(int productId)
    {
        ProductModel? resProduct = await _dbcontext.Products.Where(t => t.Id == productId).FirstOrDefaultAsync();
        if (resProduct == null)
        {
            return StatusCode(500, "Product is not found");
        }

        return StatusCode(200, resProduct);
    }

    [HttpPut("Update/{productId}")]
    public async Task<ActionResult> UpdateProduct(int productId, ProductUpdateRequestDto item)
    {
        ProductModel? resProduct = await _dbcontext.Products.Where(t => t.Id == productId).FirstOrDefaultAsync();
        if (resProduct != null)
        {
            if (!string.IsNullOrEmpty(item.Name))
                resProduct.Name = item.Name;

            if (!string.IsNullOrEmpty(item.Description))
                resProduct.Description = item.Description;

            if (item.Price != null)
                resProduct.Price = item.Price;

            resProduct.UpdatedAt = new DateTime();

            _dbcontext.Products.Update(resProduct);

            var test = await _dbcontext.SaveChangesAsync();

            return StatusCode(200, new
            {
                product = resProduct
            });
        }
        else
        {
            return StatusCode(500, "Product is not found");
        }
    }

    [HttpDelete("Delete/{productId}")]
    public async Task<ActionResult> DeleteProduct(int productId)
    {
        ProductModel? resProduct = await _dbcontext.Products.Where(t => t.Id == productId).FirstOrDefaultAsync();
        if (resProduct != null)
        {
            resProduct.DeletedAt = DateTime.Now;

            _dbcontext.Products.Update(resProduct);
            // _dbcontext.Products.Remove(resProduct);

            await _dbcontext.SaveChangesAsync();

            return StatusCode(200, "Product has been removed");
        }
        else
        {
            return StatusCode(500, "Product is not found");
        }
    }
}
