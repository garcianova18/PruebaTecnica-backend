using PruebaTecnica.Application.Common.Models;
using PruebaTecnica.Application.DTOs.Product;
using PruebaTecnica.Domain.Entities;


namespace PruebaTecnica.Application.Contracts.Repositories
{
    public interface IProductRepository:IRepository<Product>
    {
        Task<bool> ExistsByNameAsync(string name, Guid? excludeId = null);
        Task<PageResults<Product>> Filter(GetProductsRequest productsDto, CancellationToken cancellation);
    }
}
