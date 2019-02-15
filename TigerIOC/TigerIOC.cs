using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tigerbox.Spec;
using SimpleInjector;
using SimpleInjector.Lifestyles;
using Tigerbox.Forms;
using Tigerbox.Objects;
using Tigerbox.Database.Update;
using Tigerbox.Services;

namespace Tigerbox.IOC
{
    public static class TigerIOC
    {
        public static Container Container;

        /// <summary>
        /// Initialize simple injector container
        /// </summary>
        public static void InitializeContainer()
        {
            if (Container != null)
            {
                return;
            }
            Container = new Container();
            Container.Register<IConfigurationService, ConfigurationService>();
            Container.Register<IJsonService, JsonService>();
            Container.Register<IDatabaseUpdateSystem, DatabaseUpdateSystem>();
            Container.Register<TigerSharedData>();
            Container.Register<MainForm>();
        }
    }
}
