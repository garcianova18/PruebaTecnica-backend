using MediatR;
using PruebaTecnica.Application.Common.Models;
using PruebaTecnica.Application.DTOs.Product;

namespace PruebaTecnica.Application.Features.Products.Queries.GetAll;

public record GetProductsQuery(GetProductsRequest ProductsRequest) : IRequest<PageResults<ProductResponse>>;
