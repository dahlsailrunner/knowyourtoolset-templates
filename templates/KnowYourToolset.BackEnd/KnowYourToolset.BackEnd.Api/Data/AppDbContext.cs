using KnowYourToolset.BackEnd.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace KnowYourToolset.BackEnd.Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products { get; set; } = null!;
}
