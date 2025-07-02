using LinePayDemo.Api.Helpers;
using LinePayDemo.Product.Services;
using LinePayDemo.Transaction.Services;
using Microsoft.AspNetCore.Mvc;

namespace LinePayDemo.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController(IProductService productService, ITransactionService transactionService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Products([FromQuery] Guid userId)
    {
        var products = await productService.GetAllProductsAsync();
        var userBalance = await transactionService.GetUserBalanceAsync(userId);

        return Ok(new ResponseModel
        {
            Message = "success",
            Data = new { Products = products, UserBalance = userBalance },
        });
    }
}