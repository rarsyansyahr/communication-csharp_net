using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO.Ports;

namespace Praktikum_2___Communication
{
    public partial class Serial_Communication : Form
    {
        //private SerialPort port = new SerialPort("COM1", 9600, Parity.None, 8, StopBits.One);
        //[STAThread]

        public Serial_Communication()
        {
            InitializeComponent();
        }

        private void Serial_Communication_Load(object sender, EventArgs e)
        {
            string[] ports = SerialPort.GetPortNames();

            if (ports.Length > 0)
            {
                this.cbxPort.Items.AddRange(ports);
                this.cbxPort.SelectedIndex = 0;
            }

            this.btnClose.Enabled = false;
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            this.btnOpen.Enabled = false;
            this.btnClose.Enabled = true;

            try
            {
                this.serialPort1.PortName = this.cbxPort.Text;
                this.serialPort1.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.btnOpen.Enabled = true;
            this.btnClose.Enabled = false;

            try
            {
                this.serialPort1.Close();
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.serialPort1.IsOpen)
                {
                    this.serialPort1.WriteLine(this.txtSend.Text + Environment.NewLine);
                    this.txtSend.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Mrssage", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnReceive_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.serialPort1.IsOpen)
                {
                    this.txtReceive.Text = this.serialPort1.ReadExisting();
                    //this.serialPort1.WriteLine(this.txtSend.Text + Environment.NewLine);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Mrssage", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Serial_Communication_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.serialPort1.IsOpen)
            {
                this.serialPort1.Close();
            }
        }
    }
}
