using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tigerbox.Objects;
using Tigerbox.Services;
using Tigerbox.Spec;

namespace Tigerbox.SiteControl.Models
{
    public class Database
    {                
        public static TigerPages Page;

        public static void LoadConfData(IConfigurationService configurationService, IJsonService jsonService)
        {
            if (Page == null)
            {
                Page = new TigerPages(configurationService, jsonService);

                Page.LoadPages();                
            }
        }
    }
}