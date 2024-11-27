using KnowYourToolset.BackEnd.Api.ApiModels;
using KnowYourToolset.BackEnd.Api.BusinessLogic;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace KnowYourToolset.BackEnd.Api.Controllers;

[ApiController]
[ApiVersion("2.0")]
[Route("v{version:apiVersion}/products")]
public class ProductsController_v2(IProductBusinessLogic logic) : ControllerBase
{
    [HttpGet]
    [SwaggerOperation("Gets a list of all products.")]
    [SwaggerResponse(200, type: typeof(PaginatedList<ProductModel>))]
    public async Task<ActionResult<PaginatedList<ProductListModel>>> GetProducts(
        [FromQuery, Required, Range(1, int.MaxValue)] int pageNumber, 
        [FromQuery, Required, Range(1, int.MaxValue)] int pageSize, 
        string? sortColumn, string? sortOrder, bool? isActive = true, CancellationToken cxl = default)
    {
        var (ProductPage, TotalProducts) = await logic.GetProductListPaged(pageNumber, pageSize, sortColumn, sortOrder, isActive, cxl);
        return PaginatedList<ProductListModel>.CreateAsync(ProductPage, TotalProducts, pageNumber, pageSize, 
            $"{Request.GetDisplayUrl()}");
    }
}