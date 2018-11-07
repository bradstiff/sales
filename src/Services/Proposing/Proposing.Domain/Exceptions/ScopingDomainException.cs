using System;
using System.Collections.Generic;
using System.Text;

namespace Proposing.Domain.Exceptions
{
    public class ScopingDomainException : Exception
    {
        public ScopingDomainException()
        { }

        public ScopingDomainException(string message)
            : base(message)
        { }

        public ScopingDomainException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
