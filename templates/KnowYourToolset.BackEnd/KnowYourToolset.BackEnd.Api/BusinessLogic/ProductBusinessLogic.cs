using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using KnowYourToolset.BackEnd.Api.ApiModels;
using KnowYourToolset.BackEnd.Api.Data;
using KnowYourToolset.BackEnd.Api.Data.Entities;

namespace KnowYourToolset.BackEnd.Api.BusinessLogic;

public class ProductBusinessLogic(IProductRepository repo, IMapper mapper,
    ProductValidator validator, ILogger<ProductBusinessLogic> logger) : IProductBusinessLogic
{
    public async Task<List<ProductListModel>> GetProductList(CancellationToken cxl, bool? isActive = true)
    {
        var (allItems, _) = await GetProductListPaged(null, null, null, null, isActive, cxl);
        return allItems;
    }

    public async Task<(List<ProductListModel> ProductPage, int TotalProducts)> GetProductListPaged(
        int? pageNumber, int? pageSize, string? sortColumn, string? sortOrder, bool? isActive = true, CancellationToken cxl = default)
    {
        var procResults = await repo.GetProductList(cxl, pageNumber, pageSize, sortColumn, sortOrder, isActive);

        logger.LogInformation("Mapping models after the repo call.");

        return mapper.Map<(List<ProductListModel>, int)>(procResults);
    }

    public async Task<ProductModel?> GetProductById(int id)
    {
        Product? result;
        if (id >= 500)
        {
            var ex = new Exception("Simulated error occurred in repo call!!");
            ex.Data.Add("Id", id.ToString());
            throw ex;
        }
        result = await repo.GetProductById(id);

        return mapper.Map<ProductModel>(result);
    }

    public async Task<int> CreateProduct(ProductModel product)
    {
        var validationResult = await validator.ValidateAsync(product);
        if (!validationResult.IsValid)
            throw new ValidationException("Validation errors occurred",
                validationResult.Errors.Select(a => new ValidationFailure
                {
                    PropertyName = a.PropertyName,
                    ErrorMessage = a.ErrorMessage
                }).ToList());

        var newProduct = mapper.Map<Product>(product);
        return await repo.CreateProduct(newProduct);
    }

    public async Task<ProductModel> UpdateProduct(ProductModel product)
    {
        var validationResult = await validator.ValidateAsync(product);
        if (!validationResult.IsValid)
            throw new ValidationException("Validation errors occurred",
                validationResult.Errors.Select(a => new ValidationFailure
                {
                    PropertyName = a.PropertyName,
                    ErrorMessage = a.ErrorMessage
                }).ToList());

        var productToUpdate = mapper.Map<Product>(product);
        var updatedProduct = await repo.UpdateCompany(productToUpdate);
        updatedProduct.UpdatedDate = DateTime.Now;
        return mapper.Map<ProductModel>(updatedProduct);
    }

    public async Task DeleteProduct(int id)
    {
        var product = await repo.GetProductById(id);
        if (product is null || (!product.IsActive))
        {
            throw new ValidationException("Active product does not exist to be soft-deleted.",
                [new ValidationFailure { ErrorMessage = "Product not found or already inactive." }]);
        }
        product.IsActive = false;
        product.UpdatedDate = DateTime.Now;
        await repo.SoftDeleteProduct(product);
    }
}
