using MediatR;
using PruebaTecnica.Application.DTOs.Product;

namespace PruebaTecnica.Application.Features.Products.Commands.Create;

public record CreateProductCommand(CreateProductRequest ProductRequest) : IRequest<ProductResponse>;
