using AutoMapper;
using MediatR;
using PruebaTecnica.Application.Common.Models;
using PruebaTecnica.Application.Contracts.Repositories;
using PruebaTecnica.Application.DTOs.Product;

namespace PruebaTecnica.Application.Features.Products.Queries.GetAll;

public class GetProductsHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<GetProductsQuery, PageResults<ProductResponse>>
{
    public async Task<PageResults<ProductResponse>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
      
        var result =await unitOfWork.Products.Filter(request.ProductsRequest, cancellationToken);

        return mapper.Map<PageResults<ProductResponse>>(result);
    }
}
