using Tigerbox.Spec;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tigerbox.Services
{
    public class ConfigurationService : IConfigurationService
    {
        /// <summary>
        /// Get config from appconfig
        /// </summary>
        /// <typeparam name="T">Type of config</typeparam>
        /// <param name="configName">Configuration key to be returned</param>
        /// <returns>Specified typed configuration</returns>
        public T GetConfiguration<T>(string configName)
        {
            var configuration = ConfigurationManager.AppSettings[configName];
            return (T)Convert.ChangeType(configuration, typeof(T));
        }
    }
}
