using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;

namespace ECommerce.Application.DTOs
{
    public class ProductCreateRequest
{
    public string Category { get; set; } = null!;
    public string ProductCode { get; set; } = null!;
    public string Name { get; set; } = null!;
    public IFormFile? Image { get; set; }
    public decimal Price { get; set; }
    public int MinimumQuantity { get; set; }
    public decimal DiscountRate { get; set; }
}

}
