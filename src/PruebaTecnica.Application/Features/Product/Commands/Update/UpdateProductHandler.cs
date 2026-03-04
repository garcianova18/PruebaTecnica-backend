using AutoMapper;
using FluentValidation;
using MediatR;
using PruebaTecnica.Application.Common.Exceptions;
using PruebaTecnica.Application.Contracts.Repositories;
using PruebaTecnica.Application.DTOs.Product;

namespace PruebaTecnica.Application.Features.Products.Commands.Update;

public class UpdateProductHandler(IUnitOfWork unitOfWork, IMapper mapper, IValidator<UpdateProductRequest> validator)
    : IRequestHandler<UpdateProductCommand, ProductResponse>
{
    public async Task<ProductResponse> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request.ProductRequest, cancellationToken);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage).First();
            throw new BadRequestException(errors);
        }

        var product = await unitOfWork.Products.GetByIdAsync(request.Id, cancellationToken);

        if (product is null)
        {
            throw new NotFoundException($"Product with id '{request.Id}' was not found.");
        }
        var nameExists = await unitOfWork.Products.ExistsByNameAsync(request.ProductRequest.Name!, request.Id);
        if (nameExists)
        {
            throw new BadRequestException($"Ya existe otro Producto con el nombre '{request.ProductRequest.Name}'.");
        }


        mapper.Map(request.ProductRequest, product);

        product.UpdatedAt = DateTime.UtcNow;

        await unitOfWork.Products.UpdateAsync(product);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return mapper.Map<ProductResponse>(product);
    }
}
