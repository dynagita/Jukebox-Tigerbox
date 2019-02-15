using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tigerbox.Exceptions
{
    public static class ExceptionMessageConsts
    {
        public const string PageAlreadyCreatedMessage = "You can't insert a page who is already inserted.";
        public const string ConfigPathNameNotSetted = "You haven't setted the key 'MainPathNames' in App.config file.";
        public const string DatabaseNotExists = "You haven't created a database. Use TigerboxDatabaseUpdate.exe to create it!";
        public const string PageNotFound = "The page your are looking for does not exists.";
        public const string SharedListPathNotFound = "You haven't setted the key 'SharedListPath' in App.config file.";
        public const string NoCreditsFound = "Please, insert credits to go on!";
        public const string ConnectionException = "Sorry, it was impossible connect to app.";
    }
}
