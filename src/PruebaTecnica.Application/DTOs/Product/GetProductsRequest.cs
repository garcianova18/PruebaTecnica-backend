using PruebaTecnica.Application.Common.Models;

namespace PruebaTecnica.Application.DTOs.Product
{
    public class GetProductsRequest:PageModel
    {
        public string? SearchTerm { get; set; }
    }
}
