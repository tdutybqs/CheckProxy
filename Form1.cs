using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Leaf.xNet;
using System.Threading.Tasks;

namespace CheckProxy
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        List<string> proxy { get; set; } = new List<string>() { };
        string Link = "https://www.google.ru/"; // Сайт, на котором будут проверяться прокси
        string writePath = @"C:\Users\Евгений\source\repos\CheckProxy\CheckProxy\ClearProxy.txt"; // Записать хорошие проекси
        string pathFile = @"C:\Users\Евгений\source\repos\CheckProxy\CheckProxy\Proxy.txt"; // Какие прокси проверить

        private void btnDnl_Click(object sender, EventArgs e)
        {
            proxy = File.ReadAllLines(pathFile).ToList();
            label1.Text = Convert.ToString(proxy.Count);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            label2.Text = "В процессе";
            for (int i = 0; i < proxy.Count; i++)
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
            // Выставлены Timeout для подключений
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