using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using PruebaTecnica.Application.Common.Models;
using PruebaTecnica.Application.Contracts.Repositories;
using PruebaTecnica.Application.DTOs.Product;
using PruebaTecnica.Application.Extensions;
using PruebaTecnica.Domain.Entities;


namespace PruebaTecnica.Infrastructure.Persistence.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext context;

        public ProductRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }
        public async Task<bool> ExistsByNameAsync(string name, Guid? excludeId = null)
        {
            return await context.Products.AnyAsync(p =>
                p.Name.ToLower() == name.ToLower() &&
                (!excludeId.HasValue || p.Id != excludeId.Value));
        }

        public async Task<PageResults<Product>> Filter(GetProductsRequest productsDto, CancellationToken cancellation)
        {
            var query = context.Products.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(productsDto.SearchTerm))
            {
                query = query.Where(p => p.Name.Contains(productsDto.SearchTerm)
              || (p.Description != null
              && p.Description.Contains(productsDto.SearchTerm)));
            }

            var totalRecords = await query.CountAsync(cancellation);

            var result = await query
                .OrderBy(x => x.Name)
                .ThenBy(x => x.Id)
                .Page(productsDto.PageIndex, productsDto.PageSize)
                .ToListAsync(cancellation);

            return new PageResults<Product>(result, totalRecords);
        }

    }
}
