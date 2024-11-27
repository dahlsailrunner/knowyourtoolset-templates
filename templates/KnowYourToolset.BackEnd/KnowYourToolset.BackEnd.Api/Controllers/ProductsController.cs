using KnowYourToolset.BackEnd.Api.ApiModels;
using KnowYourToolset.BackEnd.Api.BusinessLogic;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace KnowYourToolset.BackEnd.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("v{version:apiVersion}/[controller]")]
public class ProductsController(IProductBusinessLogic logic) : ControllerBase
{
    [HttpGet]
    [SwaggerOperation("Gets a list of all products (no paging).",
        "Optional `isActive` parameter which defaults to null and will return all products. " +
        "Use `true` to return only active products, and `false` to return inactive products.")]
    [SwaggerResponse(200, type: typeof(List<ProductModel>))]
    public async Task<ActionResult<List<ProductListModel>>> GetProducts(CancellationToken cxl, bool? isActive = null)
    {
        var result = await logic.GetProductList(cxl, isActive);
        return result;
    }

    [HttpGet("{id}")]
    [SwaggerOperation("Gets a single product on its ID.", "Use a value greater than 500 to see an error being thrown.")]
    [SwaggerResponse(200, type: typeof(ProductModel))]
    [SwaggerResponse(404)]
    public async Task<ActionResult<ProductModel>> GetProductById(
        [SwaggerParameter("ID value of the product", Required = true)]
        int id)
    {
        var product = await logic.GetProductById(id);
        return product != null ? Ok(product) : NotFound();
    }

    [HttpPost]
    [SwaggerOperation("Creates a single product.",
        "Do not set the ID property - any provided value is ignored and one is assigned during creation.")]
    [SwaggerResponse(201, type: typeof(int))]
    public async Task<ActionResult<int>> CreateProduct([FromBody] ProductModel newProduct)
    {
        var id = await logic.CreateProduct(newProduct);
        var uri = Request.Path.Value + $"/{id}";
        return Created(uri, id);
    }

    [HttpPut]
    [SwaggerOperation("Updates a single product.", "Updates single product.")]
    [SwaggerResponse(201, type: typeof(int))]
    public async Task<ActionResult<int>> UpdateProduct([FromBody] ProductModel product)
    {
        var updatedProduct = await logic.UpdateProduct(product);
        return Ok(updatedProduct);
    }

    [HttpDelete]
    [SwaggerOperation("(Soft) Deletes a single product.",
        "The product will be set to inactive.")]
    [SwaggerResponse(200)]
    public async Task<ActionResult<int>> DeleteProduct([FromBody] int productId)
    {
        await logic.DeleteProduct(productId);
        return Ok();
    }
}
