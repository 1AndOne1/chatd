using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.Sockets;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private TcpClient tcpClient;
        private NetworkStream stream;
        public Form1()
        {
            InitializeComponent();
        }
        private async void button1_Click(object sender, EventArgs e)
        {
            var port = 12345;
            string ip = textBox1.Text;
            tcpClient = new TcpClient(ip, port);
            stream = tcpClient.GetStream();
            textBox2.AppendText("Пользователь" + " " + ip + " " + "подключился к чату");
            textBox2.AppendText(Environment.NewLine);
        }
        public async Task SendMessage(string message) 
        {
                try
                {
                    byte[] data = Encoding.UTF8.GetBytes(message);
                    await stream.WriteAsync(data, 0, data.Length);

                    string formattedMessage = $"{message}\n";

                    textBox2.AppendText(formattedMessage);
                    textBox2.AppendText(Environment.NewLine);
                    textBox2.ScrollToCaret();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("err: " + ex.Message);
                }
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(textBox1.Text))
                {
                    string message = $"{textBox1.Text}: {textBox3.Text}\n";
                    SendMessage(message);
                    textBox1.Clear();
                }
            }
            catch (Exception)
            {
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            string message = textBox3.Text;
            string ip = textBox1.Text;
            string formattedMessage = $"{message}\n";
            for (int i = 0; i < message.Length; i++)
            {
                if (i == message.Length)
                {
                    textBox2.AppendText(formattedMessage);
                    textBox2.AppendText(Environment.NewLine);
                    textBox2.ScrollToCaret();
                }
            }
            
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
