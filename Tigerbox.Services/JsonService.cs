using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tigerbox.Spec;

namespace Tigerbox.Services
{
    public class JsonService : IJsonService
    {
        /// <summary>
        /// Método responsável por deserializar um arquivo Json em um Objeto
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns>Objeto deserializado do Json</returns>
        public T FileToJson<T>(string data)
        {
            T result = JsonConvert.DeserializeObject<T>(data);
            return result;
        }

        /// <summary>
        /// Método responsável por serializar dados em uma arquivo json
        /// </summary>
        /// <param name="data">Dados a serem serializados</param>
        /// <returns>string contendo o texto json</returns>
        public string SerializeJson(object data)
        {
            return JsonConvert.SerializeObject(data);
        }
    }
}
