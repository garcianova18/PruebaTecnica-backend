using MediatR;
using PruebaTecnica.Application.DTOs.Product;

namespace PruebaTecnica.Application.Features.Products.Queries.GetById;

public record GetProductByIdQuery(Guid Id) : IRequest<ProductResponse>;
