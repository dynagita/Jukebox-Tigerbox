using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tigerbox.Objects;
using Tigerbox.Spec;

namespace Tigerbox.Database.Update
{
    public class DatabaseUpdateSystem : IDatabaseUpdateSystem
    {
        TigerPages _tigerboxPages;

        /// <summary>
        /// DataBaseUpdateSystem constructor
        /// </summary>
        /// <param name="_tigerboxPages">Pages who will be saved</param>
        public DatabaseUpdateSystem(TigerPages _tigerboxPages)
        {
            this._tigerboxPages = _tigerboxPages;
        }

        /// <summary>
        /// Create or Update the json database
        /// </summary>
        public void UpdateDatabase()
        {
            try
            {
                _tigerboxPages.CreateUpdateDatabase();
                _tigerboxPages.SaveDataBase();
                Console.WriteLine("Database successfully updated!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.ReadKey();
            
        }
    }
}
