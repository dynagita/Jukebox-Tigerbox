using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tigerbox.Forms;
using static Tigerbox.Objects.Enums;
using Tigerbox.Objects;
using System.Net.Sockets;
using System.Net;
using Tigerbox.Spec;
using Tigerbox.Services;

namespace Tigerbox.Mangers
{
    public class FormThreadManager : BaseThreadManager
    {
        private MainForm _form;
        private TigerTCPServer _tigerServer;

        public FormThreadManager(MainForm form)
        {
            _form = form;
            _tigerServer = new TigerTCPServer(_form);
        }

        private void ExecuteTigerboxAction(object form, TigerNetworkData media)
        {
            Action action = () =>
            {
                if (media.Action != TigerActions.Undefined)
                {
                    ((MainForm)form).ExecuteFormActions(media.Action);
                }
                else
                {
                    ((MainForm)form).SelectRemoteMedia(media.Media);
                }
            };

            ((MainForm)form).BeginInvoke(action);
        }

        public override void ExecuteThread()
        {
            
            _tigerServer.UpdateMethod = ExecuteTigerboxAction;
            _tigerServer.StartServer();
        }

        private void ApplyActionToTigerbox(TigerNetworkData mediaNetworkReceived)
        {
            _form.BeginInvoke((Action)(() =>
            {
                if (mediaNetworkReceived.Action != TigerActions.Undefined)
                {
                    this._form.ExecuteFormActions(mediaNetworkReceived.Action);
                }
                else
                {
                    this._form.SelectRemoteMedia(mediaNetworkReceived.Media);
                }
            }));
        }

        public override void Stop()
        {
            try
            {
                _tigerServer.Dispose();
            }
            catch (SocketException)
            {
                //Exception expected, iterrupted connection
            }
            
            base.Stop();

        }
    }
}
