using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.ValidatePhoto.Providers.Exceptions
{
    public class InvalidImageException : Exception
    {
        public InvalidImageException(string message) : base(message)
        {
            Console.WriteLine("The photo does not meet the security requisites. Authentication failed!");
        }
    }
}
