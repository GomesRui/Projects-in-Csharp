using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.ValidatePhoto.Providers.Exceptions
{
    public class JsonBuildingException : Exception
    {
        public JsonBuildingException()
        {
            Console.WriteLine("Json Building Exception triggered! Please contact support team to debug the logs!");
        }
    }
}
