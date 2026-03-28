using BonProfCa.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using BonProfCa.Models;
using BonProfCa.Services;

namespace BonProfCa.Controllers;

[Produces("application/json")]
[Consumes("application/json")]
[Route("[controller]")]
[ApiController]
[EnableCors]
public class ProductController(ProductService productService) : ControllerBase
{
    [HttpGet("all")]
    public async Task<ActionResult<Response<List<ProductDetails>>>> GetAllProducts()
    {
        var response = await productService.GetAllProductsAsync();

        if (response.Status == 200)
        {
            return Ok(response);
        }

        return StatusCode(response.Status, response);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Response<ProductDetails>>> GetProductById(
        [FromRoute] Guid id)
    {
        var response = await productService.GetProductByIdAsync(id);

        return StatusCode(response.Status, response);
    }

    [HttpGet("cursus/{id:guid}")]
    public async Task<ActionResult<Response<List<ProductDetails>>>> GetProductsByCursusId(
        [FromRoute] Guid id)
    {
        var response = await productService.GetProductsByCursusIdAsync(id);

        if (response.Status == 200)
        {
            return Ok(response);
        }

        return StatusCode(response.Status, response);
    }

    [HttpGet("teacher/{id:guid}")]
    public async Task<ActionResult<Response<List<ProductDetails>>>> GetProductsByTeacher(
        [FromRoute] Guid id)
    {
        var response = await productService.GetProductsByTeacherIdAsync(id);

        if (response.Status == 200)
        {
            return Ok(response);
        }

        return StatusCode(response.Status, response);
    }

    [HttpPost]
    public async Task<ActionResult<Response<ProductDetails>>> CreateProduct(
        [FromBody] ProductCreate productDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new Response<object>
            {
                Status = 400,
                Message = "Donn�es de validation invalides",
                Data = ModelState
            });
        }

        var response = await productService.CreateProductAsync(productDto, User);

        return StatusCode(response.Status, response);
    }

    [HttpPut]
    public async Task<ActionResult<Response<ProductDetails>>> UpdateProduct(
        [FromBody] ProductUpdate productDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new Response<object>
            {
                Status = 400,
                Message = "Donn�es de validation invalides",
                Data = ModelState
            });
        }

        var response = await productService.UpdateProductAsync(productDto);

        return StatusCode(response.Status, response);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<Response<object>>> DeleteProduct(
        [FromRoute] Guid id)
    {
        var response = await productService.DeleteProductAsync(id);

        return StatusCode(response.Status, response);
    }
}
