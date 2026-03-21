using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PruebaTecnica.Application.Common.Models;
using PruebaTecnica.Application.DTOs.Product;
using PruebaTecnica.Application.Features.Products.Commands.Create;
using PruebaTecnica.Application.Features.Products.Commands.Delete;
using PruebaTecnica.Application.Features.Products.Commands.Update;
using PruebaTecnica.Application.Features.Products.Queries.GetAll;
using PruebaTecnica.Application.Features.Products.Queries.GetById;


namespace PruebaTecnica.Api.Controllers;

[ApiController]
[Route("api/Product")]
[Authorize]
public class ProductController : ControllerBase
{
    private readonly IMediator mediator;

    public ProductController(IMediator mediator)
    {
        this.mediator = mediator;
    }


    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Get([FromQuery] GetProductsRequest getProducts, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetProductsQuery(getProducts), cancellationToken);
        return Ok(ApiResponse<PageResults<ProductResponse>>.Success(result));
    }

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetProductByIdQuery(id), cancellationToken);
        return Ok(ApiResponse<ProductResponse>.Success(result));
    }

    [HttpPost]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Create([FromBody] CreateProductRequest createProduct, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new CreateProductCommand(createProduct), cancellationToken);
        return CreatedAtAction(nameof(GetById),new { id = result.Id }, ApiResponse<ProductResponse>.Success(result, StatusCodes.Status201Created));
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductRequest updateProduct, CancellationToken cancellationToken)
    {
        await mediator.Send(new UpdateProductCommand(id, updateProduct), cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await mediator.Send(new DeleteProductCommand(id), cancellationToken);
        return NoContent();
    }
}
