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

namespace Praktikum_2___Communication
{
    public partial class Form1 : Form
    {
        private TcpClient client;
        public StreamReader STR;
        public StreamWriter STW;
        public string receive;
        public string textSend;

        public Form1()
        {
            InitializeComponent();

            IPAddress[] localIP = Dns.GetHostAddresses(Dns.GetHostName());

            foreach (IPAddress address in localIP)
            {
                if (address.AddressFamily == AddressFamily.InterNetwork)
                {
                    this.txtIPC.Text = address.ToString();
                }
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            TcpListener listener = new TcpListener(IPAddress.Any, int.Parse(this.txtPortS.Text));
            listener.Start();

            this.client = listener.AcceptTcpClient();
            this.STR = new StreamReader(this.client.GetStream());
            this.STW = new StreamWriter(this.client.GetStream());
            this.STW.AutoFlush = true;

            this.backgroundWorker1.RunWorkerAsync();
            this.backgroundWorker2.WorkerSupportsCancellation = true;
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            this.client = new TcpClient();
            IPEndPoint IP_End = new IPEndPoint(IPAddress.Parse(this.txtIPC.Text), int.Parse(this.txtPortC.Text));

            try
            {
                this.client.Connect(IP_End);

                if (this.client.Connected)
                {
                    this.txtChat.AppendText("Connected to Server.." + Environment.NewLine);

                    this.STW = new StreamWriter(this.client.GetStream());
                    this.STR = new StreamReader(this.client.GetStream());
                    this.STW.AutoFlush = true;

                    this.backgroundWorker1.RunWorkerAsync();
                    this.backgroundWorker2.WorkerSupportsCancellation = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (this.txtPesan.Text != "")
            {
                this.textSend = this.txtPesan.Text;
                this.backgroundWorker2.RunWorkerAsync();

                this.txtPesan.Clear();
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            while (this.client.Connected)
            {
                try
                {
                    this.receive = this.STR.ReadLine();

                    this.txtChat.Invoke(new MethodInvoker(delegate()
                    {
                        this.txtChat.AppendText("Anda : " + this.receive + Environment.NewLine);
                    }));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            if (this.client.Connected)
            {
                this.STW.WriteLine(this.textSend);

                this.txtChat.Invoke(new MethodInvoker(delegate()
                {
                    this.txtChat.AppendText("Saya : " + this.textSend.ToString() + Environment.NewLine);
                }));
            }
            else
            {
                MessageBox.Show("Send Failed !");
            }

            this.backgroundWorker2.CancelAsync();
        }

        private void btnSerialCom_Click(object sender, EventArgs e)
        {
            Serial_Communication sm = new Serial_Communication();
            sm.Show();
        }
    }
}
