using LinePayDemo.Api.Helpers;
using LinePayDemo.Product.Services;
using Microsoft.AspNetCore.Mvc;

namespace LinePayDemo.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController(IProductService productService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Products()
    {
        var products = await productService.GetAllProductsAsync();

        return Ok(new ResponseModel
        {
            Message = "success",
            Data = new { Products = products },
        });
    }
}