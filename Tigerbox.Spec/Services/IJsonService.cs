﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tigerbox.Spec
{
    public interface IJsonService
    {
        T FileToJson<T>(string data);

        string SerializeJson(object data);
    }
}
