using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace client
{
    public partial class Form1 : Form
    {

        private BinaryReader reader;
        private BinaryWriter writer;
        private bool activeCall = false;

        string lastMessage = "";

        public Form1()
        {
            InitializeComponent();
        }

        private void Connect_Click(object sender, EventArgs e)
        {
            string host = tbHostAddress.Text;
            int port = System.Convert.ToInt16(nUDPort.Value);

            try
            {
                TcpClient client = new TcpClient(host, port);

                NetworkStream ns = client.GetStream();
                reader = new BinaryReader(ns);
                writer = new BinaryWriter(ns);

                writer.Write("password");
                activeCall = true;

                lbMessage.Items.Add("Nawiązano połączenie z  " + host + " na porcie: " + port);


                backgroundWorker1.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                lbMessage.Items.Add("Nie udało się nawiązać połączenia");
                activeCall = false;
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            if (activeCall)
            {
                string message = "";

                

                while (message != "END" )
                {
                    message = reader.ReadString();
                    if (message != lastMessage) { 
                    lbMessage.Invoke(new MethodInvoker(delegate { lbMessage.Items.Add(message); }));
                    lastMessage = message;}
                }
            }
        }

    
        private void send_Click(object sender, EventArgs e)
        {
            string message = "Client: " + messageBox.Text;
            writer.Write(message);
            lbMessage.Invoke(new MethodInvoker(delegate { lbMessage.Items.Add(message); }));
        }
    }
}