using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.ValidatePhoto.Providers.Exceptions
{
    public class ImageNotFoundException : Exception
    {
        public ImageNotFoundException(string path)
        {
            Console.WriteLine("The file {0} doesn't exist!", path);
        }
    }
}
