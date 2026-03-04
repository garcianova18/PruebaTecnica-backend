using AutoMapper;
using MediatR;
using PruebaTecnica.Application.Common.Exceptions;
using PruebaTecnica.Application.Contracts.Repositories;
using PruebaTecnica.Application.DTOs.Product;

namespace PruebaTecnica.Application.Features.Products.Queries.GetById;

public class GetProductByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<GetProductByIdQuery, ProductResponse>
{
    public async Task<ProductResponse> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await unitOfWork.Products.GetByIdAsync(request.Id, cancellationToken);

        if (product is null)
        {
            throw new NotFoundException($"Product with id {request.Id} not found.");
        }

        return mapper.Map<ProductResponse>(product);
    }
}
