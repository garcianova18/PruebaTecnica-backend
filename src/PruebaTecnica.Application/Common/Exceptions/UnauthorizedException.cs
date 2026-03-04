

using System.Net;

namespace PruebaTecnica.Application.Common.Exceptions
{
    public class UnauthorizedException: BaseException
    {
        public UnauthorizedException(string message):base(message, (int)HttpStatusCode.Unauthorized)
        {

        }
       
    }
}
