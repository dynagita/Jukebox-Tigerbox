﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tigerbox.Forms;

namespace Jukebox_Tigerbox
{
    static class Program
    {
        /// <summary>
        /// Ponto de entrada principal para o aplicativo.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();

            Application.SetCompatibleTextRenderingDefault(false);

            Tigerbox.IOC.TigerIOC.InitializeContainer();

            var mainForm = Tigerbox.IOC.TigerIOC.Container.GetInstance<MainForm>();

            Application.Run(mainForm);
        }        
    }
}
