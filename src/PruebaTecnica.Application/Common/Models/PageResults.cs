
namespace PruebaTecnica.Application.Common.Models;

public record PageResults<T>(IEnumerable<T> Items,int TotalRecords);

