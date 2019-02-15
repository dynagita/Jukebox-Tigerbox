using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Tigerbox.Exceptions;
using Tigerbox.Spec;

namespace Tigerbox.Objects
{
    public class TigerTCPClient
    {
        private int _port;
        private string _host;
        private IConfigurationService _configurationService;
        private IJsonService _jsonService;


        public TigerTCPClient(IConfigurationService configurationService, IJsonService jsonService)
        {
            _configurationService = configurationService;
            _jsonService = jsonService;

            _host = _configurationService.GetConfiguration<string>(Constants.HostConfig);
            _port = _configurationService.GetConfiguration<int>(Constants.PortConfig);
        }

        private byte[] Get1252Buffer(string data)
        {
            Encoding win1252 = Encoding.GetEncoding(1252);
            Encoding utf8 = Encoding.UTF8;
            byte[] buffer = utf8.GetBytes(data);
            buffer = Encoding.Convert(utf8, win1252, buffer);

            return buffer;
        }

        public void SendData(TigerNetworkData networkData)
        {
            try
            {
                using (TcpClient _client = new TcpClient(_host, _port))
                {
                    string data = _jsonService.SerializeJson(networkData);

                    byte[] buffer = Get1252Buffer(data);

                    var stream = _client.GetStream();

                    stream.Write(buffer, 0, buffer.Length);
                    stream.Close();
                    stream = null;
                    _client.Close();
                }
            }
            catch (TigerConnectionException ex)
            {

                throw ex;
            }

        }
    }
}
