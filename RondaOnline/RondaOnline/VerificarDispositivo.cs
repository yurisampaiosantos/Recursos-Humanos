using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.NetworkInformation;

namespace RondaOnline
{
    public partial class VerificarDispositivo : Form
    {
        public VerificarDispositivo()
        {
            InitializeComponent();
        }


        public void validarOnline()
        {
            Dispositivo dispositivo = new Dispositivo();

            foreach (Dispositivo disp in dispositivo.ListaDispositivo())
            {

                Ping ping = new Ping();
                PingReply reply = ping.Send(disp.Ip, 1000);
                if (reply.Status.ToString() != "Success")
                {
                    dispositivo.InseriorOffLine(disp.Id);
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            validarOnline();
            Application.Exit();            
        }

    }
}
