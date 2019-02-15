using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Tigerbox.Exceptions;
using Tigerbox.Services;
using Tigerbox.Spec;

namespace Tigerbox.Objects
{
    public delegate void UpdateTigerboxMachine(object form, TigerNetworkData tigerMediaNetwork);

    public class TigerTCPServer : IDisposable
    {
        public UpdateTigerboxMachine UpdateMethod;
        private int _port { get; set; }
        private string _host { get; set; }
        private TcpListener _server = null;
        private IJsonService _jsonService;
        private IConfigurationService _configuration;
        private object _form;
        private bool _running = false;

        public TigerTCPServer(object mainForm)
        {
            _jsonService = new JsonService();
            _configuration = new ConfigurationService();

            _port = _configuration.GetConfiguration<int>(Constants.PortConfig);
            _host = _configuration.GetConfiguration<string>(Constants.HostConfig);
            _form = mainForm;
        }

        public void StartServer()
        {

            _running = true;
            IPAddress ip = IPAddress.Parse(_host);
            _server = new TcpListener(ip, _port);
            _server.Server.ReceiveTimeout = 1800000;
            _server.Server.SendTimeout = 1800000;
            _server.Start();
            do
            {
                try
                {
                    using (TcpClient _client = _server.AcceptTcpClient())
                    {
                        string data = null;
                        byte[] buffer = new byte[1024];

                        NetworkStream stream = _client.GetStream();
                        if (stream.DataAvailable)
                        {

                            int i;

                            // Loop to receive all the data sent by the client.
                            while ((i = stream.Read(buffer, 0, buffer.Length)) != 0)
                            {
                                Encoding win1252 = Encoding.GetEncoding(1252);
                                data = win1252.GetString(buffer, 0, i);
                                data = data.Trim();
                            }

                            var tigerMedia = _jsonService.FileToJson<TigerNetworkData>(data);

                            ExecuteTigerboxAction(_form, tigerMedia);

                            stream.Close();
                            stream.Dispose();
                            stream = null;
                            _client.Client.Disconnect(false);
                            _client.Close();

                        }

                    }
                }
                catch
                {
                    //This exception was not thrown because it can't stop program when it doesn't work.
                }
            } while (_running);
        }

        private void ExecuteTigerboxAction(object form, TigerNetworkData tigerMediaNetwork)
        {
            if (UpdateMethod == null)
            {
                throw new TigerConnectionException();
            }
            UpdateMethod(form, tigerMediaNetwork);
        }

        public void Dispose()
        {
            _running = false;
            _server.Stop();
            _server = null;
        }
    }
}
