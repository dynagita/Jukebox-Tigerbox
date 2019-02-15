using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tigerbox.Exceptions
{
    public class NoCreditsException : Exception
    {
        public NoCreditsException() :base(ExceptionMessageConsts.NoCreditsFound)
        {

        }
    }
}
