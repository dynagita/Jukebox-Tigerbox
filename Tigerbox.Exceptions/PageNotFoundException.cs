﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tigerbox.Exceptions
{
    public class PageNotFoundException : Exception
    {
        public PageNotFoundException() : base(ExceptionMessageConsts.PageNotFound)
        { }
    }
}
