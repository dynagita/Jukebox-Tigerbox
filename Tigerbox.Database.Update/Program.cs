using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tigerbox.Spec;

namespace Tigerbox.Database.Update
{
    class Program
    {
        static void Main(string[] args)
        {
             Tigerbox.IOC.TigerIOC.InitializeContainer();

            var system = Tigerbox.IOC.TigerIOC.Container.GetInstance<IDatabaseUpdateSystem>();

            system.UpdateDatabase();
            
        }
    }
}
