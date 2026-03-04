using AutoMapper;
using FluentValidation;
using MediatR;
using PruebaTecnica.Application.Common.Exceptions;
using PruebaTecnica.Application.Contracts.Repositories;
using PruebaTecnica.Application.DTOs.Product;
using PruebaTecnica.Domain.Entities;


namespace PruebaTecnica.Application.Features.Products.Commands.Create;

public class CreateProductHandler(IUnitOfWork unitOfWork, IValidator<CreateProductRequest> validator, IMapper mapper)
    : IRequestHandler<CreateProductCommand, ProductResponse>
{
    public async Task<ProductResponse> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request.ProductRequest, cancellationToken);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage).First();
            throw new BadRequestException(errors);
        }
        if (await unitOfWork.Products.ExistsByNameAsync(request.ProductRequest.Name!))
        {
            throw new BadRequestException($"Ya existe un Producto con el nombre '{request.ProductRequest.Name}'.");
        }
            

        var productEntity = mapper.Map<Product>(request.ProductRequest);
       var product = await unitOfWork.Products.AddAsync(productEntity, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return mapper.Map<ProductResponse>(product);
    }
}
