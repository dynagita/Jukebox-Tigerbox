using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tigerbox.Exceptions
{
    public class DatabaseNotExistsException : Exception
    {
        public DatabaseNotExistsException() : base(ExceptionMessageConsts.DatabaseNotExists)
        {
        }
    }
}
