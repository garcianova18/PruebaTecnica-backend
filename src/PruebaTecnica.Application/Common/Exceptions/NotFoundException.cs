using System;
using System.Net;

namespace PruebaTecnica.Application.Common.Exceptions
{
    public class NotFoundException:BaseException
    {
        public NotFoundException(string message):base(message, (int)HttpStatusCode.NotFound)
        {
            
        }
    }
}
