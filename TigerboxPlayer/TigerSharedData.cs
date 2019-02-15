using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tigerbox.Exceptions;
using Tigerbox.Spec;

namespace Tigerbox.Objects
{
    public class TigerSharedData
    {
        private int _credits;

        public int Credits
        {
            get
            {
                return _credits;
            }
        }

        private IConfigurationService _configuration;

        private IJsonService _jsonService;

        public TigerSharedData(IConfigurationService configuration, IJsonService service)
        {
            this._mutexList = new Mutex();

            this._mutexCredit = new Mutex();

            this._configuration = configuration;

            this._jsonService = service;

            this.LoadSharedData();
        }

        private IList<TigerMedia> _sharedList;

        public IList<TigerMedia> SharedList
        {
            get
            {
                return _sharedList;
            }
        }

        private Mutex _mutexList;

        private Mutex _mutexCredit;

        public void InsertNewMedia<V>(V media)
        {
            _mutexList.WaitOne();

            object auxiliar = media;

            _sharedList.Add((TigerMedia)auxiliar);

            _mutexList.ReleaseMutex();
        }

        public V GetFirstMedia<V>()
        {           
            object auxiliar = null;

            if (_sharedList.Count > 0)
            {
                _mutexList.WaitOne();

                auxiliar = _sharedList.ElementAt(0);

                _sharedList.RemoveAt(0);

                _mutexList.ReleaseMutex();

            }

            try
            {
                if (auxiliar==null)
                {
                    throw new NoMediaSelectedException();
                }
                return (V)auxiliar;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Increase shared credit
        /// </summary>
        public void IncreaseCredit()
        {
            _mutexCredit.WaitOne();
            _credits++;
            _mutexCredit.ReleaseMutex();
        }

        /// <summary>
        /// Decrease shared credit
        /// </summary>
        public void DecreaseCredit()
        {
            if (_credits == 0)
            {
                throw new NoCreditsException();
            }

            _mutexCredit.WaitOne();
            _credits--;
            _mutexCredit.ReleaseMutex();
        }

        /// <summary>
        /// Save shared data into a json
        /// </summary>
        public void SaveSharedData()
        {
            string dataPath = GetSharedDataPath();

            try
            {
                var sharedData = new Dictionary<string, object>();
                sharedData.Add("Credits", _credits);
                sharedData.Add("SharedList", _sharedList);

                string data = _jsonService.SerializeJson(sharedData);

                File.WriteAllText(dataPath, data);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Load shared data from json
        /// </summary>
        private void LoadSharedData()
        {
            string dataPath = GetSharedDataPath();
            if (File.Exists(dataPath))
            {
                var data = File.ReadAllText(dataPath);
                var sharedData = _jsonService.FileToJson<Dictionary<string, object>>(data);
                _credits = Convert.ToInt32(sharedData["Credits"]);
                _sharedList = ((Newtonsoft.Json.Linq.JArray)sharedData["SharedList"]).ToObject<List<TigerMedia>>();               
            }
            else
            {
                _credits = 0;
                _sharedList = new List<TigerMedia>();
            }
        }

        /// <summary>
        /// Get's the SharedDataPath setted into appConfig
        /// </summary>
        /// <returns>path</returns>
        private string GetSharedDataPath()
        {
            string dataPath = _configuration.GetConfiguration<string>(Constants.SharedListPath);
            if (string.IsNullOrWhiteSpace(dataPath))
            {
                throw new NoSharedListPathFound();
            }
            return dataPath;
        }
    }
}
