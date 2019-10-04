using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Leaf.xNet;

namespace CheckProxy
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        List<string> proxy { get; set; } = new List<string>() { };
        string Link = "https://www.google.ru/";
        string writePath = @"C:\Users\Евгений\source\repos\CheckProxy\CheckProxy\ClearProxy.txt";
        string pathFile = @"C:\Users\Евгений\source\repos\CheckProxy\CheckProxy\Proxy.txt";

        private void btnDnl_Click(object sender, EventArgs e)
        {
            proxy = File.ReadAllLines(pathFile).ToList();
            label1.Text = Convert.ToString(proxy.Count);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            label2.Text = "В процессе";
            for (int i = 0; i < 2100; i++)
            {
                string address = proxy[i].ToString().Replace(" ", "");
                if (Get(Link, address))
                {
                    using (StreamWriter sw = new StreamWriter(writePath, true, Encoding.Default))
                    {
                        sw.Write(Environment.NewLine + proxy[i].ToString().Replace(" ", ""));
                    }
                }
                proxy.RemoveAt(i);
            }

            for (int i = 0; i < proxy.Count; i++)
            {
                using (StreamWriter sw = new StreamWriter(pathFile, false, Encoding.Default))
                {
                    sw.Write(Environment.NewLine + proxy[i].ToString());
                }
            }
            label2.Text = "Завершено!";
        }

        int bad = 0;
        int good = 0;

        public bool Get(string Link, string address)
        {
            HttpRequest request = new HttpRequest
            {
                KeepAlive = true
            };
            request.ConnectTimeout = 2000;
            request.UserAgentRandomize();
            request.Proxy = ProxyClient.Parse(ProxyType.HTTP, address);
            request.Proxy.ReadWriteTimeout = 2000;
            request.ReadWriteTimeout = 2000;
            request.Proxy.ConnectTimeout = 2000;
            bool answer = false;

            try
            {
                request.Get(Link).ToString();
                label5.Text = good++.ToString();
                answer = true;
            }
            catch
            {

                label6.Text = bad++.ToString();
                answer = false;
            }
            return answer;
        }
    }     
}