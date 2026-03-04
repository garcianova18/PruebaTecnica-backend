
using System.Net;

namespace PruebaTecnica.Application.Common.Exceptions
{
    public class ValidationException: BaseException
    {
        public ValidationException(string message):base(message, (int)HttpStatusCode.UnprocessableContent)
        {
        }
    }
}
