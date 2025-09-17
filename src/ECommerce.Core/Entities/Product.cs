using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core.Entities;

public class Product
{
    public Guid Id { get; private set; } = Guid.NewGuid();

    public string Category { get; set; } = null!;

    // Unique code e.g. P01, P02
    public string ProductCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    // Local path of stored image, actual file will be on disk
    public string? ImagePath { get; set; }

    public decimal Price { get; set; }

    public int MinimumQuantity { get; set; }

    // Represented as fraction (0.1 = 10%)
    public decimal DiscountRate { get; set; }
}
