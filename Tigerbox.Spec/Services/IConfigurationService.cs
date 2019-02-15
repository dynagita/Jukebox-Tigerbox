using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tigerbox.Spec
{
    public interface IConfigurationService
    {
        T GetConfiguration<T>(string configName);
    }
}
