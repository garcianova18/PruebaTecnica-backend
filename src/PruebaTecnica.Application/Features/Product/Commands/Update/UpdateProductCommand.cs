using MediatR;
using PruebaTecnica.Application.DTOs.Product;

namespace PruebaTecnica.Application.Features.Products.Commands.Update;

public record UpdateProductCommand(Guid Id, UpdateProductRequest ProductRequest) : IRequest<ProductResponse>;
