using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace AndroidIdsTool
{
    public partial class Authorize : Form
    {
        public bool pass = false;

        private string username = "";
        private string password = "";
        private string serverIp = "";

        public Authorize()
        {
            InitializeComponent();
        }

        private void Authorize_Load(object sender, EventArgs e)
        {
            //test
            this.serverIpTextBox.Text = "192.168.15.";
            this.usernameTextBox.Text = "Administrator";
            this.passwordTextBox.Text = "";

            Global.appKey = Common.formatStr(Common.MD5Encoding(Common.getCpu()), 32);
            Global.appIv = Common.formatStr(Common.MD5Encoding(Common.GetHDid()), 16);

            if (System.IO.File.Exists(@"auth.txt"))
            {
                String txt = "";
                StreamReader sr = new StreamReader(@"auth.txt");
                while (!sr.EndOfStream)
                {
                    string str = sr.ReadLine();
                    txt += str;
                }
                sr.Close();
                if (txt != "")
                {
                    string auth = Common.decrypt(txt, Global.appKey, Global.appIv);
                    MessageBox.Show(auth);
                    if (auth != "" && auth.IndexOf("#") != -1)
                    {
                        username = auth.Split('#')[0];
                        password = auth.Split('#')[1];
                        serverIp = auth.Split('#')[2];
                    }
                }

                if (username != "" && password != "" && serverIp !="")
                {
                    if (username == Global.jUsername && password == Global.jPassword)
                    {
                        Global.debug = true;
                        to();     
                    }
                    else
                    {
                        if (serverIp == Global.jServerIp)
                        {
                            to();
                        }
                        else
                        {
                            string url = "http://" + serverIp + "/android/auth.php";
                            string s = post(url, username, password);
                            if (s == "1")
                            {
                                to();
                            }
                        }
                    }
                }
            }
        }

        private void to()
        {
            Global.username = this.username;
            Global.serverIp = this.serverIp;
            this.pass = true;
            this.Close();
        }

        private string post(string url, string username, string password)
        {
            string retString = "";

            string param = "username=" + username + "&password=" + password;
            byte[] bs = Encoding.ASCII.GetBytes(param);

            try
            {
                HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);
                req.Method = "POST";
                req.ContentType = "application/x-www-form-urlencoded";
                req.ContentLength = bs.Length;

                using (Stream reqStream = req.GetRequestStream())
                {
                    reqStream.Write(bs, 0, bs.Length);
                }
                using (WebResponse wr = req.GetResponse())
                {
                    //在这里对接收到的页面内容进行处理
                    Stream responseStream = wr.GetResponseStream();
                    StreamReader streamReader = new StreamReader(responseStream, Encoding.UTF8);
                    retString = streamReader.ReadToEnd();
                    streamReader.Close();
                    responseStream.Close();
                }
            }catch(Exception e){
                retString = e.ToString();
            }

            return retString;
        }

        //授权文件
        private void auth()
        {
            string str = username + "#" + password + "#" + serverIp + "#" +Global.email;
            string auth = Common.encrypt(str, Global.appKey, Global.appIv);

            FileStream fs = new FileStream("auth.txt", FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            //开始写入
            sw.Write(auth);
            //清空缓冲区
            sw.Flush();
            //关闭流
            sw.Close();
            fs.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            serverIp = this.serverIpTextBox.Text;
            username = this.usernameTextBox.Text;
            password = this.passwordTextBox.Text;

            if (serverIp == "" || username == "" || password == "")
            {
                return;
            }

            if (username == Global.jUsername && password == Global.jPassword)
            {
                auth();
                Global.debug = true;
                to();
            }
            else if (username == Global.jUsername && password == Global.jTmpPassword)
            {
                MessageBox.Show("部分功能可能与虚拟机网卡冲突，请暂时禁用虚拟网卡.");
                to();
            }
            else
            {
                string url = "http://" + serverIp + "/android/auth.php";
                string s = post(url, username, password);
                if (s == "1")
                {
                    MessageBox.Show("部分功能可能与虚拟机网卡冲突，请暂时禁用虚拟网卡.");
                    auth();
                    to();
                }
                else
                {
                    MessageBox.Show("授权失败");
                }
            }
        }
    }
}
