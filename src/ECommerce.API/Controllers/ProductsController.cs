using ECommerce.Application.Interfaces;
using ECommerce.Core.Entities;
using ECommerce.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;

[ApiController]
[Route("api/[controller]")]
//[Authorize]
public class ProductsController : ControllerBase
{
    private readonly IProductRepository _productRepo;
    private readonly IWebHostEnvironment _env;

    public ProductsController(IProductRepository productRepo, IWebHostEnvironment env)
    {
        _productRepo = productRepo;
        _env = env;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products = await _productRepo.GetAllAsync();
        var dtos = products.Select(p => new ProductDto
        {
            Id = p.Id,
            Name = p.Name,
            Category = p.Category,
            ProductCode = p.ProductCode,
            Price = p.Price,
            MinimumQuantity = p.MinimumQuantity,
            DiscountRate = p.DiscountRate,
            ImagePath = p.ImagePath
        });
        return Ok(dtos);
    }



    [HttpPost]
    [RequestSizeLimit(10_000_000)] // limit 10MB for images
    public async Task<IActionResult> Create([FromForm] ProductCreateRequest request)
    {
        // Handle image upload
        string? imagePath = null;
        if (request.Image != null && request.Image.Length > 0)
        {
            var uploadsFolder = Path.Combine(_env.ContentRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var fileName = Guid.NewGuid() + Path.GetExtension(request.Image.FileName);
            var filePath = Path.Combine(uploadsFolder, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await request.Image.CopyToAsync(stream);

            imagePath = $"uploads/{fileName}";
        }

        var product = new Product
        {
            Name = request.Name,
            Category = request.Category,
            ProductCode = request.ProductCode,
            Price = request.Price,
            MinimumQuantity = request.MinimumQuantity,
            DiscountRate = request.DiscountRate,
            ImagePath = imagePath
        };

        await _productRepo.AddAsync(product);
        return Ok(new { message = "Product created successfully", productId = product.Id });
    }




    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var product = await _productRepo.GetByIdAsync(id);
        if (product == null)
            return NotFound();

        var dto = new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Category = product.Category,
            ProductCode = product.ProductCode,
            Price = product.Price,
            MinimumQuantity = product.MinimumQuantity,
            DiscountRate = product.DiscountRate,
            ImagePath = product.ImagePath
        };

        return Ok(dto);
    }




    [HttpGet("code/{productCode}")]
    public async Task<IActionResult> GetByCode(string productCode)
    {
        var product = await _productRepo.GetByCodeAsync(productCode);
        if (product == null)
            return NotFound();

        var dto = new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Category = product.Category,
            ProductCode = product.ProductCode,
            Price = product.Price,
            MinimumQuantity = product.MinimumQuantity,
            DiscountRate = product.DiscountRate,
            ImagePath = product.ImagePath
        };

        return Ok(dto);
    }






    [HttpPut("{id:guid}")]
    [RequestSizeLimit(10_000_000)] // limit 10MB for images
    public async Task<IActionResult> Update(Guid id, [FromForm] ProductCreateRequest request)
    {
        var product = await _productRepo.GetByIdAsync(id);
        if (product == null)
            return NotFound();

        product.Name = request.Name;
        product.Category = request.Category;
        product.ProductCode = request.ProductCode;
        product.Price = request.Price;
        product.MinimumQuantity = request.MinimumQuantity;
        product.DiscountRate = request.DiscountRate;

        // Handle image upload
        if (request.Image != null && request.Image.Length > 0)
        {
            var uploadsFolder = Path.Combine(_env.ContentRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var fileName = Guid.NewGuid() + Path.GetExtension(request.Image.FileName);
            var filePath = Path.Combine(uploadsFolder, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await request.Image.CopyToAsync(stream);

            product.ImagePath = $"uploads/{fileName}";
        }

        await _productRepo.UpdateAsync(product);
        return Ok(new { message = "Product updated successfully" });
    }





    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var product = await _productRepo.GetByIdAsync(id);
        if (product == null)
            return NotFound();

        await _productRepo.DeleteAsync(id);
        return Ok(new { message = "Product deleted successfully" });
    }


}

