using KnowYourToolset.BackEnd.Api.Data.Entities;

namespace KnowYourToolset.BackEnd.Api.Data;
public interface IProductRepository
{
    Task<int> CreateProduct(Product product);
    Task<Product?> GetProductById(int id);
    Task<(List<Product> Products, int TotalCount)> GetProductList(CancellationToken cxl, int? pageNumber = null, int? pageSize = null, string? sortColumn = null, string? sortOrder = null, bool? isActive = true);
    Task<bool> IsProductNameUnique(int id, string? productName, CancellationToken token);
    Task<bool> IsProductNameUnique(string? productName, CancellationToken token);
    Task SoftDeleteProduct(Product product);
    Task<Product> UpdateCompany(Product product);
}