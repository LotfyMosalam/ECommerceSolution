using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core.Entities;

public class User
{
    public Guid Id { get; private set; } = Guid.NewGuid();

    // Unique username
    public string Username { get; set; } = null!;

    // Unique email
    public string Email { get; set; } = null!;

    // Store hashed password
    public string PasswordHash { get; set; } = null!;

    public DateTime? LastLoginAt { get; set; }
}

