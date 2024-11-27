using Bogus.DataSets;
using KnowYourToolset.BackEnd.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;

namespace KnowYourToolset.BackEnd.Api.Data;

public class ProductRepository(AppDbContext ctx) : IProductRepository
{
    public async Task<(List<Product> Products, int TotalCount)> GetProductList(CancellationToken cxl, int? pageNumber = null,
        int? pageSize = null, string? sortColumn = null, string? sortOrder = null, bool? isActive = true)
    {
        var query = ctx.Products
            .Where(c => isActive == null || c.IsActive == isActive);

        query = string.IsNullOrWhiteSpace(sortOrder) || sortOrder?.ToLower() == "asc"
            ? query.OrderBy(GetSortProperty(sortColumn))
            : query.OrderByDescending(GetSortProperty(sortColumn));

        if (pageNumber is not null && pageSize is not null)
        {
            var totalItemCount = await query.CountAsync(cxl);
            var items = await query.Skip((pageNumber.Value - 1) * pageSize.Value).Take(pageSize.Value).ToListAsync(cxl);

            return new(items, totalItemCount);
        }

        return new(await query.ToListAsync(), -1);
    }

    public async Task<Product?> GetProductById(int id)
    {
        return await ctx.Products.FindAsync(id);
    }

    public async Task<bool> IsProductNameUnique(string? productName, CancellationToken token)
    {
        productName = productName?.Trim() ?? "";
        return await ctx.Products.AnyAsync(a => a.Name == productName, token);
    }

    public async Task<bool> IsProductNameUnique(int id, string? productName, CancellationToken token)
    {
        productName = productName?.Trim() ?? "";
        return await ctx.Products.AnyAsync(a => a.Name == productName && a.Id != id, token);
    }

    public async Task<int> CreateProduct(Product product)
    {
        product.Name = product.Name!.Trim();
        ctx.Products.Add(product);
        await ctx.SaveChangesAsync();
        return product.Id;
    }

    public async Task<Product> UpdateCompany(Product product)
    {
        if (product.Name is not null)
            product.Name = product.Name!.Trim();

        var currentCompany = await ctx.Products.FindAsync(product.Id) ?? throw new Exception("Product not found");

        var properties = typeof(Company).GetProperties();
        foreach (var property in properties)
        {
            var newValue = property.GetValue(product);
            if (newValue != null)
            {
                property.SetValue(currentCompany, newValue);
            }
        }
        ctx.Products.Update(currentCompany);
        await ctx.SaveChangesAsync();
        return currentCompany;
    }

    public async Task SoftDeleteProduct(Product product)
    {
        ctx.Attach(product);
        ctx.Entry(product).Property(p => p.IsActive).IsModified = true;
        ctx.Entry(product).Property(p => p.UpdatedDate).IsModified = true;
        ctx.Products.Update(product);

        await ctx.SaveChangesAsync();
    }

    private static Expression<Func<Product, object>> GetSortProperty(string? sortColumn)
    {
        if (string.IsNullOrWhiteSpace(sortColumn))
            sortColumn = "Id";

        PropertyInfo? propertyInfo = typeof(Product).GetProperty(sortColumn,
            BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance) ??
            throw new ArgumentException($"No property '{sortColumn}' found on type '{typeof(Product).Name}'");

        var parameter = Expression.Parameter(typeof(Product), "product");
        var propertyAccess = Expression.Property(parameter, propertyInfo);
        var castToObject = Expression.Convert(propertyAccess, typeof(object));

        return Expression.Lambda<Func<Product, object>>(castToObject, parameter);
    }
}