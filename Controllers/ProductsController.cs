using Microsoft.AspNetCore.Mvc;
using ABMProductos.Models.DTOs.Requests;
using ABMProductos.Models.DTOs.Responses;
using ABMProductos.Services.Interfaces;

namespace ABMProductos.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService service)
    {
        _productService = service;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        List<ProductForReadDto> products = _productService.GetAllProducts();

        return Ok(products);
    }

    [HttpGet("search")]
    public IActionResult Search([FromQuery] string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return BadRequest("Hay que indicar un texto para buscar.");
        }

        List<ProductForReadDto> products = _productService.SearchProductsByName(name);

        return Ok(products);
    }

    [HttpGet("stats")]
    public IActionResult GetStats()
    {
        ProductStatsDto stats = _productService.GetStats();

        return Ok(stats);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        ProductForReadDto? product = _productService.GetProductById(id);

        if (product == null)
        {
            return NotFound();
        }

        return Ok(product);
    }

    [HttpPost]
    public IActionResult Create([FromBody] ProductForCreateDto dto)
    {
        if (_productService.NameIsTaken(dto.Name))
        {
            return Conflict("Ya existe un producto con ese nombre.");
        }

        ProductForReadDto created = _productService.CreateProduct(dto);

        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] ProductForUpdateDto dto)
    {
        if (_productService.GetProductById(id) == null)
        {
            return NotFound();
        }

        _productService.UpdateProduct(id, dto);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        if (_productService.GetProductById(id) == null)
        {
            return NotFound();
        }

        _productService.DeleteProduct(id);

        return NoContent();
    }
}
