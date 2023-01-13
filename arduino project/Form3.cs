using System;
using System.Windows.Forms;
using System.IO.Ports;

namespace arduino_project
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }
        Form1 f1 = new Form1();
        string[] portlar =  SerialPort.GetPortNames();
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            button1.Enabled = true;
            
            f1.Enabled = true;
            f1.serialPort1.PortName = comboBox1.SelectedItem.ToString();               //"COM4";
            f1.serialPort1.BaudRate = 9600;
            Control.CheckForIllegalCrossThreadCalls = false;
            f1.button3.Enabled = true;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            f1.Show();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            comboBox1.Items.AddRange(portlar);
        }
    }
}
