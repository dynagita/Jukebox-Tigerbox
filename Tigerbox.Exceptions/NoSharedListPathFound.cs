using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tigerbox.Exceptions
{
    public class NoSharedListPathFound : Exception
    {

        public NoSharedListPathFound() : base(ExceptionMessageConsts.SharedListPathNotFound)
        {
        }
    }
}
