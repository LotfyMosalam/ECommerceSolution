using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.DTOs;

public class ProductDto
{
    public Guid Id { get; set; }
    public string Category { get; set; } = null!;
    public string ProductCode { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? ImagePath { get; set; }
    public decimal Price { get; set; }
    public int MinimumQuantity { get; set; }
    public decimal DiscountRate { get; set; }
}
