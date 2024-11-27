using KnowYourToolset.BackEnd.Api.ApiModels;

namespace KnowYourToolset.BackEnd.Api.BusinessLogic;
public interface IProductBusinessLogic
{
    Task<int> CreateProduct(ProductModel product);
    Task DeleteProduct(int id);
    Task<ProductModel?> GetProductById(int id);
    Task<List<ProductListModel>> GetProductList(CancellationToken cxl, bool? isActive = true);
    Task<(List<ProductListModel> ProductPage, int TotalProducts)> GetProductListPaged(int? pageNumber, int? pageSize, 
        string? sortColumn, string? sortOrder, bool? isActive = true, CancellationToken cxl = default);
    Task<ProductModel> UpdateProduct(ProductModel product);
}