using MediatR;
using PruebaTecnica.Application.Common.Exceptions;
using PruebaTecnica.Application.Contracts.Repositories;

namespace PruebaTecnica.Application.Features.Products.Commands.Delete;

public class DeleteProductHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteProductCommand, Unit>
{
    public async Task<Unit> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await unitOfWork.Products.GetByIdAsync(request.Id, cancellationToken);
        if (product is null)
        {
            throw new NotFoundException($"Product with id '{request.Id}' was not found.");
        }

        await unitOfWork.Products.DeleteAsync(product);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
